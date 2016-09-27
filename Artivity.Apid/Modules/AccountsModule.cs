using Artivity.Apid;
using Artivity.Apid.Accounts;
using Artivity.Apid.IO;
using Artivity.Apid.Platforms;
using Artivity.Apid.Plugin;
using Artivity.Apid.Protocols.Atom;
using Artivity.Apid.Protocols.Authentication;
using Artivity.DataModel;
using Nancy;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Modules
{
    /// <summary>
    /// Handles the authorization and installation of online service accounts.
    /// </summary>
    public class AccountsModule : ModuleBase
    {
        #region Members

        /// <summary>
        /// Maps a session id to an issued authentication request for querying the status.
        /// </summary>
        private static readonly Dictionary<string, IOnlineServiceClient> _sessions = new Dictionary<string, IOnlineServiceClient>();

        #endregion

        #region Constructors

        public AccountsModule(PluginChecker checker, IModelProvider modelProvider, IPlatformProvider platform)
            : base("/artivity/api/1.0/accounts", modelProvider, platform)
        {
            // Get a list of all installed online accounts.
            Get["/"] = parameters =>
            {
                if (!string.IsNullOrEmpty(Request.Query.featureUri))
                {
                    string featureUri = Request.Query.featureUri;

                    if (!IsUri(featureUri))
                    {
                        return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }

                    return GetAccountsWithFeature(new Uri(featureUri));
                }
                else
                {
                    return GetAccounts();
                }
            };

            // Get a list of all supported online account types.
            Get["/clients"] = parameters =>
            {
                if(!string.IsNullOrEmpty(Request.Query.featureUri))
                {
                    string featureUri = Request.Query.featureUri;

                    if(!IsUri(featureUri))
                    {
                        return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }

                    return GetServiceClientsWithFeature(new Uri(featureUri));
                }
                else if(!string.IsNullOrEmpty(Request.Query.clientUri))
                {
                    string clientUri = Request.Query.clientUri;

                    if(!IsUri(clientUri))
                    {
                        return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }

                    return GetServiceClient(new Uri(clientUri));
                }
                else
                {
                    return GetServiceClients();
                }
            };

            // Gets the current status of a online service client from a session id (use to check the auth progress).
            Get["/clients/status"] = parameters =>
            {
                return GetServiceClientStatus();
            };

            // Begin an authorization request for a online service account (needs to be called *before* installing).
            // Returns session id.
            Get["/authorize"] = parameters =>
            {
                return AuthorizeAccount();
            };

            // Send an OAuth 2.0 access token to a remote server.
            Get["/authorize/oauth2/token"] = parameters =>
            {
                return SendOAuth2AccessToken();
            };

            // Install a account from a auth session id.
            Get["/install"] = parameters =>
            {
                return InstallAccount();
            };

            // Uninstall a account with a specific id.
            Get["/uninstall"] = parameters =>
            {
                return UninstallAccount();
            };

            // Upload content into an account.
            Get["/upload"] = parameters =>
            {
                return UploadArchive();
            };
        }

        #endregion

        #region Methods

        private Response GetAccounts()
        {
            List<OnlineAccount> accounts = ModelProvider.GetAgents().GetResources<OnlineAccount>().ToList();

            return ResponseAsJsonSync(accounts);
        }

        private Response GetAccountsWithFeature(Uri featureUri)
        {
            HashSet<string> clients = new HashSet<string>();

            foreach(IOnlineServiceClient client in OnlineServiceClientFactory.GetRegisteredClients())
            {
                if(client.Features.Any(f => f.Uri == featureUri))
                {
                    clients.Add(client.Uri.AbsoluteUri);
                }
            }

            List<OnlineAccount> accounts = new List<OnlineAccount>();

            foreach(OnlineAccount account in ModelProvider.GetAgents().GetResources<OnlineAccount>())
            {
                if(clients.Contains(account.ServiceClient.Uri.AbsoluteUri))
                {
                    accounts.Add(account);
                }
            }

            return ResponseAsJsonSync(accounts);
        }

        private Response GetServiceClients()
        {
            return Response.AsJson(OnlineServiceClientFactory.GetRegisteredClients());
        }

        private Response GetServiceClientsWithFeature(Uri featureUri)
        {
            List<IOnlineServiceClient> clients = OnlineServiceClientFactory.GetRegisteredClients().ToList();

            return Response.AsJson(clients.Where(c => c.Features.Any(f => f.Uri == featureUri)));
        }

        private Response GetServiceClient(Uri providerUri)
        {
            try
            {
                return Response.AsJson(OnlineServiceClientFactory.TryGetClient(providerUri));
            }
            catch (KeyNotFoundException)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        private Response GetServiceClientStatus()
        {
            string sessionId = Request.Query["sessionId"];

            if (string.IsNullOrEmpty(sessionId))
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            if (!_sessions.ContainsKey(sessionId))
            {
                return Logger.LogError(HttpStatusCode.NotFound, "Session with token {0} not found.", sessionId);
            }

            IOnlineServiceClient client = _sessions[sessionId];

            return Response.AsJson(client);
        }

        private Response SendOAuth2AccessToken()
        {
            string sessionId = Request.Query["sessionId"];

            if (string.IsNullOrEmpty(sessionId))
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            IOnlineServiceClient client = _sessions[sessionId];

            if (client == null)
            {
                return HttpStatusCode.NotFound;
            }

            OAuth2AuthenticationClient authenticator = client.TryGetAuthenticationClient<OAuth2AuthenticationClient>(HttpAuthenticationClientState.Processing);

            if (authenticator != null)
            {
                authenticator.SendAuthorizationToken(Request, sessionId);

                return HttpStatusCode.Accepted;
            }

            return HttpStatusCode.MethodNotAllowed;
        }

        private Response AuthorizeAccount()
        {
            IOnlineServiceClient client = OnlineServiceClientFactory.TryGetClient(Request);

            if (client == null)
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }
            else
            {
                client.InitializeQueryParametersFromPreset(Request);
                client.SanitizeQueryParameters(Request);
            }

            IHttpAuthenticationClient authenticator = client.TryGetAuthenticationClient(Request);

            if (authenticator == null)
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            // Remove any previous sessions.
            string sessionId = _sessions.FirstOrDefault(s => s.Value == client).Key;

            if (!string.IsNullOrEmpty(sessionId))
            {
                _sessions.Remove(sessionId);
            }

            // Generate an session handle for the current request.
            Dictionary<string, object> sessionHandle = GetSessionHandle(client);

            sessionId = sessionHandle["id"].ToString();

            // Only execute the request if there are no successful previous requests.
            authenticator.HandleRequestAsync(Request, sessionId);

            return Response.AsJson(sessionHandle);
        }

        private Response InstallAccount()
        {
            string sessionId = Request.Query.sessionId;

            if (string.IsNullOrEmpty(sessionId))
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            IOnlineServiceClient client = _sessions[sessionId];

            if (client == null)
            {
                return Logger.LogRequest(HttpStatusCode.NotFound, Request);
            }

            IHttpAuthenticationClient authenticator = client.TryGetAuthenticationClient(HttpAuthenticationClientState.Authorized);

            if (authenticator == null)
            {
                return Logger.LogRequest(HttpStatusCode.Unauthorized, Request);
            }

            IModel model = ModelProvider.GetAgents();

            OnlineAccount account = client.InstallAccount(model);

            return account != null ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        }

        private Response UninstallAccount()
        {
            if (!IsUri(Request.Query.accountUri))
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            IModel model = ModelProvider.GetAgents();

            Person user = model.GetResources<Person>().FirstOrDefault();

            if (user == null)
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, "Unable to retrieve user agent.");
            }

            Uri accountUri = new Uri(Request.Query.accountUri);

            OnlineAccount account = model.GetResource<OnlineAccount>(accountUri);

            if (account == null)
            {
                return Logger.LogInfo(HttpStatusCode.BadRequest, "Unknown account: {0}", accountUri);
            }

            model.DeleteResource(account);

            if (user.Accounts.Contains(account))
            {
                user.Accounts.Remove(account);
                user.Commit();
            }

            return Logger.LogInfo(HttpStatusCode.OK, "Uninstalled account: {0}", accountUri);
        }

        public Response UploadArchive()
        {
            IModel model = ModelProvider.GetAll();

            if(!IsUri(Request.Query.entityUri))
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            UriRef entityUri = new UriRef(Request.Query.entityUri);

            if (!model.ContainsResource(entityUri) || !IsUri(Request.Query.accountUri))
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            UriRef accountUri = new UriRef(Request.Query.accountUri);

            if(!model.ContainsResource(accountUri))
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            // 1. Get the file name and temp file path.
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?label
                WHERE
                {
                  @entity nie:isStoredAs / rdfs:label ?label .
                }");

            query.Bind("@entity", entityUri);

            IEnumerable<BindingSet> bindings = model.GetBindings(query);
            
            if(!bindings.Any())
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, "No file data object found for entity:", entityUri);
            }

            string archiveName = Path.GetFileNameWithoutExtension(bindings.First()["label"].ToString()) + ".arta";
            string tempFile = Path.Combine(PlatformProvider.TempFolder, archiveName);

            try
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }

                // 2. Create archive
                ArchiveWriter archiveWriter = new ArchiveWriter(PlatformProvider, ModelProvider);
                archiveWriter.Write(entityUri, tempFile, DateTime.MinValue);

                // 3. Get target account and authorization parameters
                OnlineAccount account = model.GetResource<OnlineAccount>(accountUri);

                Uri clientUri = account.ServiceClient.Uri;
                Uri serviceUrl = account.ServiceUrl.Uri;

                IOnlineServicePublishingClient client = OnlineServiceClientFactory.TryGetClient<IOnlineServicePublishingClient>(clientUri);

                if(client != null)
                {
                    client.UploadArchive(Request, serviceUrl, tempFile);
                }

                Dictionary<string, object> sessionHandle = GetSessionHandle(client);

                return Response.AsJson(sessionHandle);
            }
            catch(Exception ex)
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, ex);
            }
            finally
            {
                if(File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        private Dictionary<string, object> GetSessionHandle(IOnlineServiceClient client)
        {
            string sessionId = Guid.NewGuid().ToString();

            // Prepare the JSON result dictionary.
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["id"] = sessionId;

            // Store the authenticator in the chache *before* sending the request.
            _sessions[sessionId] = client;

            return result;
        }

        #endregion
    }
}
