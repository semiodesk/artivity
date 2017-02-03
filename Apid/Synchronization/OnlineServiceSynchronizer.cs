using Artivity.Api;
using Artivity.Api.Platform;
using Artivity.DataModel;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Synchronization
{
    public sealed class OnlineServiceSynchronizer : IOnlineServiceSynchronizationProvider
    {
        #region Members

        private readonly IPlatformProvider _platformProvider;

        private readonly IModelProvider _modelProvider;

        // Maps service client URIs to corresponding handlers for retrieving synchronization changesets from remote services.
        private readonly Dictionary<UriRef, IOnlineServiceSynchronizationClient> _clients = new Dictionary<UriRef, IOnlineServiceSynchronizationClient>();

        #endregion

        #region Constructors

        public OnlineServiceSynchronizer(IModelProvider modelProvider, IPlatformProvider platformProvider)
        {
            _platformProvider = platformProvider;
            _modelProvider = modelProvider;
        }

        #endregion

        #region Methods

        public void RegisterServiceClient(IOnlineServiceSynchronizationClient client)
        {
            if(!_clients.ContainsKey(client.Uri))
            {
                _platformProvider.Logger.LogInfo("Registered synchronization client {0}", client.Uri);

                _clients[client.Uri] = client;
            }
            else
            {
                string msg = string.Format("Synchronization client already registered: {0}", client.Uri);

                throw new Exception(msg);
            }
        }

        public List<OnlineAccountSynchronizationState> TrySynchronize()
        {
            List<OnlineAccountSynchronizationState> result = new List<OnlineAccountSynchronizationState>();

            IModel model = _modelProvider.GetAgents();

            Person user = model.GetResource<Person>(new UriRef(_platformProvider.Config.Uid));

            foreach (OnlineAccount account in user.Accounts.Where(a => _clients.ContainsKey(a.ServiceClient.Uri)))
            {
                OnlineAccountSynchronizationState state = TrySynchronize(account);

                if (state != null)
                {
                    result.Add(state);
                }
            }

            user = model.GetResource<Person>(new UriRef(_platformProvider.Config.Uid));

            foreach (OnlineAccount account in user.Accounts.Where(a => _clients.ContainsKey(a.ServiceClient.Uri)))
            {
                OnlineAccountSynchronizationState state = account.SynchronizationState;
            }

            return result;
        }

        /// <summary>
        /// Begin synchronizing the resources associated with the given online account.
        /// </summary>
        /// <param name="account">An authorized online account.</param>
        /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
        public OnlineAccountSynchronizationState TrySynchronize(OnlineAccount account)
        {
            if (account.SynchronizationState == null)
            {
                // If the account has not been synchronized yet, initialize the sync state.
                account.SynchronizationState = account.Model.CreateResource<OnlineAccountSynchronizationState>();
                account.SynchronizationState.Commit();
                account.Commit();
            }

            OnlineAccountSynchronizationState accountState = account.SynchronizationState;

            try
            {
                // Asynchronously get the changes which are newer than the last pull counter (download).
                SynchronizationChangeset serverChangeset = TryGetChangesetServer(account);

                // The remote server was not available or some other error occured.
                if (serverChangeset != null)
                {
                    _platformProvider.Logger.LogInfo("Server changeset with {0} item(s) at #{1}", serverChangeset.Items.Count(), serverChangeset.Counter);

                    // Update the server update counter for the online account.
                    if (serverChangeset.Counter > accountState.ServerUpdateCounter)
                    {
                        accountState.ServerUpdateCounter = serverChangeset.Counter;
                        accountState.Commit();
                    }

                    // Asynchronously get the changes which are newer than the last push counter (upload).
                    SynchronizationChangeset clientChangeset = GetChangesetClient(account);

                    _platformProvider.Logger.LogInfo("Client changeset with {0} item(s) at #{1}", clientChangeset.Items.Count(), clientChangeset.Counter);

                    // Merge the two changesets and resolve conflicts.
                    SynchronizationChangeset merged = MergeChangesets(clientChangeset, serverChangeset);

                    // Execute the changeset and sync the resources either with the client or the server.
                    return TryExecuteChangeset(account, merged);
                }
            }
            catch (Exception ex)
            {
                _platformProvider.Logger.LogError(ex);
            }

            return null;
        }

        /// <summary>
        /// Retrieves a synchronization changeset from the local database, for all resources which are synchronized with the given account.
        /// </summary>
        /// <param name="account">An authorized online account.</param>
        /// <returns>A synchronization changeset on success.</returns>
        private SynchronizationChangeset GetChangesetClient(OnlineAccount account)
        {
            // The constructor sets the accounts _current_ update counter (the one of the client).
            SynchronizationChangeset result = new SynchronizationChangeset(account.SynchronizationState);

            // Query the database for all resource that have a push counter greater than the last push.
            ISparqlQuery query = new SparqlQuery(@"
                    SELECT ?resource ?resourceType WHERE
                    {
                        ?resource a ?resourceType ; arts:synchronizationState [
                            arts:lastUpdateCounter '-1'^^xsd:long
                        ] .
                    }
                ");

            // Synchronize agents and non-browsing activites only.
            IModelGroup model = _modelProvider.CreateModelGroup(_modelProvider.Agents, _modelProvider.Activities);

            foreach (BindingSet b in model.GetBindings(query))
            {
                Uri resource = b["resource"] as Uri;
                Uri resourceType = b["resourceType"] as Uri;

                if (resource != null && resourceType != null)
                {
                    SynchronizationChangesetItem item = new SynchronizationChangesetItem();
                    item.ActionType = SynchronizationActionType.Push;
                    item.ResourceUri = resource;
                    item.ResourceType = resourceType;
                    item.Counter = result.Counter;

                    result.Add(item);
                }
                else
                {
                    _platformProvider.Logger.LogDebug("Invalid bindings: {0} {1}", resource, resourceType);
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves a synchronization changeset from the remote service which is associated with the given account.
        /// </summary>
        /// <param name="account">An authorized online account.</param>
        /// <returns>A synchronization changeset on success.</returns>
        private SynchronizationChangeset TryGetChangesetServer(OnlineAccount account)
        {
            IOnlineServiceSynchronizationClient client = TryGetSynchronizationClient(account);

            if (client != null)
            {
                // Asynchronously get the changes which are newer than the last client update counter (download).
                long counter = account.SynchronizationState.ServerUpdateCounter;

                Task<SynchronizationChangeset> changesetTask = client.TryGetChangesetAsync(account);

                changesetTask.Wait();

                return changesetTask.Result;
            }
            else
            {
                // Let the task fail. The reson is being logged by TryGetSynchronizationClient.
                throw new NullReferenceException("client");
            }
        }

        /// <summary>
        /// Executes the actions in the synchronization changeset.
        /// </summary>
        /// <param name="changeset">A synchronization changeset.</param>
        /// <returns>The number of successfully executed actions. Equals the number of items in the changeset if all actions were successfully executed.</returns>
        private OnlineAccountSynchronizationState TryExecuteChangeset(OnlineAccount account, SynchronizationChangeset changeset)
        {
            IOnlineServiceSynchronizationClient client = TryGetSynchronizationClient(account);

            if (client != null)
            {
                OnlineAccountSynchronizationState accountState = account.SynchronizationState;

                int itemsCount = changeset.Items.Count();
                int serverCounter = accountState.ServerUpdateCounter;
                int clientCounter = accountState.ClientUpdateCounter + 1;

                _platformProvider.Logger.LogInfo("Applying changeset with {0} item(s):", itemsCount);

                foreach (SynchronizationChangesetItem item in changeset.Items)
                {
                    try
                    {
                        ResourceSynchronizationState state = null;

                        switch (item.ActionType)
                        {
                            case SynchronizationActionType.Pull:
                            {
                                Task<ResourceSynchronizationState> pullTask = client.TryPullAsync(account, item.ResourceUri, item.ResourceType);

                                pullTask.Wait();

                                state = pullTask.Result;

                                if(state != null)
                                {
                                    // Set the last update counter to the current value of the server.
                                    state.LastUpdateCounter = serverCounter;
                                    state.Commit();
                                }

                                break;
                            }
                            case SynchronizationActionType.Push:
                            {
                                Task<ResourceSynchronizationState> pushTask = client.TryPushAsync(account, item.ResourceUri, item.ResourceType);

                                pushTask.Wait();

                                state = pushTask.Result;

                                if(state != null)
                                {
                                    // Set the last update counter to the current value of the client.
                                    if (state.LastUpdateCounter == -1)
                                    {
                                        state.LastUpdateCounter = clientCounter;
                                        state.Commit();
                                    }

                                    // In case of a successful push also update the accounts client counter.
                                    if(clientCounter > accountState.ClientUpdateCounter)
                                    {
                                        accountState.ClientUpdateCounter = clientCounter;
                                        accountState.Commit();
                                    }
                                }

                                break;
                            }
                        }

                        if(state != null)
                        {
                            _platformProvider.Logger.LogInfo("{0}: <{1}> <{2}> at #{3}", item.ActionType, item.ResourceType, item.ResourceUri, state.LastUpdateCounter);
                        }
                        else
                        {
                            _platformProvider.Logger.LogError("{0}: <{1}> <{2}>", item.ActionType, item.ResourceType, item.ResourceUri);
                        }
                    }
                    catch (Exception ex)
                    {
                        _platformProvider.Logger.LogError("Caught exception when trying to {0} <{1}> <{2}>:", item.ActionType, item.ResourceUri, item.ResourceType);
                        _platformProvider.Logger.LogError(ex);
                    }
                }

                return accountState;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Merges a local and a remote synchronization changeset into one. Optionally a conflict resolution strategy can be specified.
        /// </summary>
        /// <param name="local">Local database changeset.</param>
        /// <param name="remote">Remote service changeset.</param>
        /// <param name="priority">Indicates which changes to prefer in case of conflicts.</param>
        /// <returns>A merged synchronization changeset.</returns>
        private SynchronizationChangeset MergeChangesets(SynchronizationChangeset local, SynchronizationChangeset remote, SynchronizationChangesetMergePriority priority = SynchronizationChangesetMergePriority.Remote)
        {
            SynchronizationChangeset result;

            switch (priority)
            {
                default:
                {
                    result = new SynchronizationChangeset(remote);

                    foreach (SynchronizationChangesetItem item in local.Items)
                    {
                        // We give changes in remote data priority over local changes.
                        if (!result.Resources.ContainsKey(item.ResourceUri))
                        {
                            result.Add(item);
                        }
                    }

                    break;
                }
                case SynchronizationChangesetMergePriority.Local:
                {
                    result = new SynchronizationChangeset(local);

                    foreach (SynchronizationChangesetItem item in remote.Items)
                    {
                        // We give changes in remote data priority over local changes.
                        if (!result.Resources.ContainsKey(item.ResourceUri))
                        {
                            result.Add(item);
                        }
                    }

                    break;
                }

            }

            return result;
        }

        private IOnlineServiceSynchronizationClient TryGetSynchronizationClient(OnlineAccount account)
        {
            UriRef clientUri = account.ServiceClient.Uri;

            if(_clients.ContainsKey(clientUri))
            {
                return _clients[clientUri];
            }
            else
            {
                _platformProvider.Logger.LogError("No synchronization client registered with id {0}", clientUri);

                return null;
            }
        }

        #endregion
    }

    public delegate Task<bool> ResourceSynchronizationDelegate(OnlineAccount account, Uri uri);

    public delegate Task<SynchronizationChangeset> SynchronizationChangesetProviderDelegate(OnlineAccount account);

    internal enum SynchronizationChangesetMergePriority { Remote, Local };
}
