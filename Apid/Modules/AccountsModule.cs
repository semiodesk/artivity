// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2016

using Artivity.Api;
using Artivity.Api.IO;
using Artivity.Api.Modules;
using Artivity.Api.Platform;
using Artivity.Apid;
using Artivity.Apid.Accounts;
using Artivity.Apid.Plugins;
using Artivity.Apid.Protocols.Authentication;
using Artivity.DataModel;
using Nancy;
using Newtonsoft.Json.Linq;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Artivity.Apid.Modules
{
    /// <summary>
    /// Handles the authorization and installation of online service accounts.
    /// </summary>
    public class AccountsModule : ModuleBase
    {
        #region Members

        /// <summary>
        /// Maps a session id to a pending authentication request for querying the status.
        /// </summary>
        private static readonly Dictionary<string, OnlineServiceClientSession> _sessions = new Dictionary<string, OnlineServiceClientSession>();

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
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
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
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }

                    return GetServiceClientsWithFeature(new Uri(featureUri));
                }
                else if(!string.IsNullOrEmpty(Request.Query.clientUri))
                {
                    string clientUri = Request.Query.clientUri;

                    if(!IsUri(clientUri))
                    {
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
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
            Post["/upload"] = parameters =>
            {
                return UploadArchive();
            };
        }

        #endregion

        #region Methods

        private Response GetAccounts()
        {
            User user = ModelProvider.GetAgents().GetResource<User>(new UriRef(PlatformProvider.Config.Uid));

            return Response.AsJsonSync(user.Accounts);
        }

        private Response GetAccountsWithFeature(Uri featureUri)
        {
            HashSet<string> clients = new HashSet<string>();

            foreach(IOnlineServiceClient client in OnlineServiceClientFactory.GetRegisteredClients())
            {
                if(client.ClientFeatures.Any(f => f.Uri == featureUri))
                {
                    clients.Add(client.Uri.AbsoluteUri);
                }
            }

            List<OnlineAccount> accounts = new List<OnlineAccount>();

            foreach(OnlineAccount account in ModelProvider.GetAgents().GetResources<OnlineAccount>())
            {
                if(account.ServiceClient != null && clients.Contains(account.ServiceClient.Uri.AbsoluteUri))
                {
                    accounts.Add(account);
                }
            }

            return Response.AsJsonSync(accounts);
        }

        private Response GetServiceClients()
        {
            return Response.AsJsonSync(OnlineServiceClientFactory.GetRegisteredClients());
        }

        private Response GetServiceClientsWithFeature(Uri featureUri)
        {
            List<IOnlineServiceClient> clients = OnlineServiceClientFactory.GetRegisteredClients().ToList();

            return Response.AsJsonSync(clients.Where(c => c.ClientFeatures.Any(f => f.Uri == featureUri)));
        }

        private Response GetServiceClient(Uri providerUri)
        {
            try
            {
                return Response.AsJsonSync(OnlineServiceClientFactory.TryGetClient(providerUri));
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
                return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            if (!_sessions.ContainsKey(sessionId))
            {
                return PlatformProvider.Logger.LogError(HttpStatusCode.NotFound, "Session with token {0} not found.", sessionId);
            }

            OnlineServiceClientSession session = _sessions[sessionId];

            return Response.AsJsonSync(session);
        }

        private Response SendOAuth2AccessToken()
        {
            string sessionId = Request.Query["sessionId"];

            if (string.IsNullOrEmpty(sessionId))
            {
                return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            OnlineServiceClientSession session = _sessions[sessionId];

            if (session == null)
            {
                return HttpStatusCode.NotFound;
            }

            OAuth2AuthenticationClient authenticator = session.Client.TryGetAuthenticationClient<OAuth2AuthenticationClient>(HttpAuthenticationClientState.Processing);

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
                return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }
            else
            {
                client.InitializeQueryParametersFromPreset(Request);
                client.SanitizeQueryParameters(Request);
            }

            IHttpAuthenticationClient authenticator = client.TryGetAuthenticationClient(Request);

            if (authenticator == null)
            {
                return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            // Remove any previous sessions.
            string sessionId = _sessions.FirstOrDefault(s => s.Value == client).Key;

            if (!string.IsNullOrEmpty(sessionId))
            {
                _sessions.Remove(sessionId);
            }

            // Generate an session handle for the current request.
            OnlineServiceClientSession session = GetSessionHandle(client);

            // Only execute the request if there are no successful previous requests.
            authenticator.HandleRequestAsync(Request, session.Id);

            return Response.AsJsonSync(session);
        }

        private Response InstallAccount()
        {
            string sessionId = Request.Query.sessionId;

            if (string.IsNullOrEmpty(sessionId))
            {
                return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            OnlineServiceClientSession session = _sessions[sessionId];

            if (session == null)
            {
                return PlatformProvider.Logger.LogRequest(HttpStatusCode.NotFound, Request);
            }

            IHttpAuthenticationClient authenticator = session.Client.TryGetAuthenticationClient(HttpAuthenticationClientState.Authorized);

            if (authenticator == null)
            {
                return PlatformProvider.Logger.LogRequest(HttpStatusCode.Unauthorized, Request);
            }

            IModel model = ModelProvider.GetAgents();

            // Associate the account with the user.
            User user = model.GetResource<User>(new UriRef(PlatformProvider.Config.Uid));

            if (user == null)
            {
                return PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, "Unable to retrieve user agent.");
            }

            // Run any client specific install tasks.
            OnlineAccount account = session.Client.InstallAccount(model);

            // Add the account to the user.
            if(!user.Accounts.Any(a => a.Id == account.Id))
            {
                user.Accounts.Add(account);
                user.Commit();
            }

            return account != null ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        }

        private Response UninstallAccount()
        {
            if (!IsUri(Request.Query.accountUri))
            {
                return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            IModel model = ModelProvider.GetAgents();

            User user = model.GetResource<User>(new UriRef(PlatformProvider.Config.Uid));

            if (user == null)
            {
                return PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, "Unable to retrieve user agent.");
            }

            Uri accountUri = new Uri(Request.Query.accountUri);

            OnlineAccount account = model.GetResource<OnlineAccount>(accountUri);

            if (account == null)
            {
                return PlatformProvider.Logger.LogInfo(HttpStatusCode.BadRequest, "Unknown account: {0}", accountUri);
            }

            model.DeleteResource(account);

            if (user.Accounts.Contains(account))
            {
                user.Accounts.Remove(account);
                user.Commit();
            }

            return PlatformProvider.Logger.LogInfo(HttpStatusCode.OK, "Uninstalled account: {0}", accountUri);
        }

        public Response UploadArchive()
        {
            IModel model = ModelProvider.GetAll();

            if(!IsUri(Request.Query.entityUri))
            {
                return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            UriRef entityUri = new UriRef(Request.Query.entityUri);

            if (!model.ContainsResource(entityUri) || !IsUri(Request.Query.accountUri))
            {
                return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            UriRef accountUri = new UriRef(Request.Query.accountUri);

            if(!model.ContainsResource(accountUri))
            {
                return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            // Get the file name and temp file path.
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
                return PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, "No file data object found for entity:", entityUri);
            }

            // Read the JSON content that provides additional metadata about the archive.
            ArchiveManifest manifest = null;

            if (Request.Body.Length > 0)
            {
                try
                {
                    using (var reader = new StreamReader(Request.Body))
                    {
                        string data = reader.ReadToEnd();

                        JObject metadata = JObject.Parse(data);

                        manifest = new ArchiveManifest();

                        JToken token = null;

                        if (metadata.TryGetValue("title", out token))
                        {
                            manifest.Title = token.Value<string>();
                        }

                        if (metadata.TryGetValue("description", out token))
                        {
                            manifest.Description = token.Value<string>();
                        }

                        foreach(JToken creator in metadata["creators"])
                        {
                            JToken name = creator["name"];
                            JToken email = creator["email"];

                            if(name != null)
                            {
                                ArchiveManifestCreator c = new ArchiveManifestCreator();
                                c.Name = name.Value<string>();

                                if(email != null)
                                {
                                    c.EmailAddress = email.Value<string>();
                                }

                                manifest.Creators.Add(c);
                            }
                        }

                        if (metadata.TryGetValue("license", out token))
                        {
                            manifest.License = token.Value<string>();
                        }
                    }
                }
                catch(Exception ex)
                {
                    PlatformProvider.Logger.LogError(HttpStatusCode.BadRequest, ex);
                }
            }

            string archiveName = Path.GetFileNameWithoutExtension(bindings.First()["label"].ToString()) + ".arta";
            string tempFile = Path.Combine(PlatformProvider.TempFolder, archiveName);

            try
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }

                // Get target account and authorization parameters
                OnlineAccount account = model.GetResource<OnlineAccount>(accountUri);

                Uri userUri = new UriRef(PlatformProvider.Config.Uid);
                Uri clientUri = account.ServiceClient.Uri;
                Uri serviceUrl = account.ServiceUrl.Uri;

                IOnlineServicePublishingClient publishingClient = OnlineServiceClientFactory.TryGetClient<IOnlineServicePublishingClient>(clientUri);

                if(publishingClient != null)
                {
                    ArchiveWriterBase archiveWriter = new EntityArchiveWriter(PlatformProvider, ModelProvider);

                    OnlineServiceClientSession session = GetSessionHandle(publishingClient);

                    // The publishing client state is a multi-task progress info.
                    session.Progress.Tasks.Add(archiveWriter.Progress);
                    session.Progress.Tasks.Add(publishingClient.TaskProgress);
                    session.Progress.Reset();

                    // Run the export and upload tasks asynchronously.
                    Task.Run(() =>
                    {
                        session.Progress.CurrentTask = session.Progress.Tasks[0];

                        archiveWriter.Write(userUri, entityUri, tempFile, DateTime.MinValue);

                        session.Progress.CurrentTask = session.Progress.Tasks[1];

                        publishingClient.UploadArchive(Request, serviceUrl, tempFile, manifest);

                        // TODO: Remove the temp file. Implement a completed handler on the publishing client.
                    });

                    return Response.AsJsonSync(session);
                }
                else
                {
                    return PlatformProvider.Logger.LogError(HttpStatusCode.NotFound, new KeyNotFoundException(clientUri.AbsoluteUri));
                }
            }
            catch(Exception ex)
            {
                return PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, ex);
            }
            finally
            {
                if(File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        private OnlineServiceClientSession GetSessionHandle(IOnlineServiceClient client)
        {
            OnlineServiceClientSession session = new OnlineServiceClientSession(client);

            // Store the authenticator in the chache *before* sending the request.
            _sessions[session.Id] = session;

            return session;
        }

        #endregion
    }
}
