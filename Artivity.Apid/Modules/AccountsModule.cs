using Artivity.Apid;
using Artivity.Apid.Accounts;
using Artivity.Apid.Platforms;
using Artivity.Apid.Plugin;
using Artivity.Apid.Protocols.Authentication;
using Artivity.DataModel;
using Nancy;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly Dictionary<string, IOnlineServiceConnector> _sessions = new Dictionary<string, IOnlineServiceConnector>();

        #endregion

        #region Constructors

        public AccountsModule(PluginChecker checker, IModelProvider modelProvider, IPlatformProvider platform)
            : base("/artivity/api/1.0/accounts", modelProvider, platform)
        {
            // Get a list of all installed online accounts.
            Get["/"] = parameters =>
            {
                return GetAccounts();
            };

            // Get a list of all supported online account types.
            Get["/connectors"] = parameters =>
            {
                string connectorUri = Request.Query["connectorUri"];

                if (string.IsNullOrEmpty(connectorUri))
                {
                    return GetServiceConnectors();
                }
                else if (IsUri(connectorUri))
                {
                    return GetServiceConnector(new Uri(connectorUri));
                }
                else
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };

            // Gets the current status of a online service connector from a session id (use to check the auth progress).
            Get["/connectors/status"] = parameters =>
            {
                return GetServiceConnectorStatus();
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
        }

        #endregion

        #region Methods

        private Response GetAccounts()
        {
            List<OnlineAccount> accounts = ModelProvider.GetAgents().GetResources<OnlineAccount>(true).ToList();

            return Response.AsJson(accounts);
        }

        private Response GetServiceConnectors()
        {
            return Response.AsJson(OnlineServiceConnectorFactory.GetRegisteredConnectors());
        }

        private Response GetServiceConnector(Uri providerUri)
        {
            try
            {
                return Response.AsJson(OnlineServiceConnectorFactory.TryGetServiceConnector(providerUri));
            }
            catch (KeyNotFoundException)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        private Response GetServiceConnectorStatus()
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

            IOnlineServiceConnector connector = _sessions[sessionId];

            return Response.AsJson(connector);
        }

        private Response SendOAuth2AccessToken()
        {
            string sessionId = Request.Query["sessionId"];

            if (string.IsNullOrEmpty(sessionId))
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            IOnlineServiceConnector connector = _sessions[sessionId];

            if (connector == null)
            {
                return HttpStatusCode.NotFound;
            }

            OAuth2AuthenticationClient authenticator = connector.TryGetAuthenticationClient<OAuth2AuthenticationClient>(HttpAuthenticationClientState.Processing);

            if (authenticator != null)
            {
                authenticator.SendAuthorizationToken(Request, sessionId);

                return HttpStatusCode.Accepted;
            }

            return HttpStatusCode.MethodNotAllowed;
        }

        private Response AuthorizeAccount()
        {
            IOnlineServiceConnector connector = OnlineServiceConnectorFactory.TryGetServiceConnector(Request);

            if (connector == null)
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }
            else
            {
                connector.InitializePresetQueryParameters(Request);
            }

            IHttpAuthenticationClient authenticator = connector.TryGetAuthenticationClient(Request);

            if (authenticator == null)
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            string sessionId;

            switch(authenticator.ClientState)
            {
                case HttpAuthenticationClientState.Processing:
                {
                    return Logger.LogRequest(HttpStatusCode.Processing, Request);
                }
                case HttpAuthenticationClientState.Authorized:
                {
                    // Return the session token from the previous successful request.
                    sessionId = _sessions.FirstOrDefault(s => s.Value == connector).Key;

                    break;
                }
                default:
                {
                    // Generate an authorization token for the current request.
                    sessionId = Guid.NewGuid().ToString();

                    // Only execute the request if there are no successful previous requests.
                    authenticator.HandleRequestAsync(Request, sessionId);

                    break;
                }
            }

            // Prepare the JSON result dictionary.
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["id"] = sessionId;

            // Store the authenticator in the chache *before* sending the request.
            _sessions[sessionId] = connector;

            return Response.AsJson(result);
        }

        private Response InstallAccount()
        {
            string sessionId = Request.Query.sessionId;

            if (string.IsNullOrEmpty(sessionId))
            {
                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            }

            IOnlineServiceConnector connector = _sessions[sessionId];

            if (connector == null)
            {
                return Logger.LogRequest(HttpStatusCode.NotFound, Request);
            }

            IHttpAuthenticationClient authenticator = connector.TryGetAuthenticationClient(HttpAuthenticationClientState.Authorized);

            if (authenticator == null)
            {
                return Logger.LogRequest(HttpStatusCode.Unauthorized, Request);
            }

            IModel model = ModelProvider.GetAgents();

            OnlineAccount account = connector.InstallAccount(model);

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

        #endregion
    }
}
