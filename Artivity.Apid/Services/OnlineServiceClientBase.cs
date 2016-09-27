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
// Copyright (c) Semiodesk GmbH 2015

using Artivity.Apid.Protocols.Authentication;
using Artivity.DataModel;
using Nancy;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Accounts
{
    public abstract class OnlineServiceClientBase : IOnlineServiceClient
    {
        #region Members

        /// <summary>
        /// Gets the Uniform Resource Identifier.
        /// </summary>
        public Uri Uri { get; protected set; }

        /// <summary>
        /// Gets the title of the online service.
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// Gets the URL of the online service.
        /// </summary>
        public Uri ServiceUrl { get; protected set; }

        /// <summary>
        /// Gets a list of supported client features.
        /// </summary>
        public List<Resource> Features { get; set; }

        /// <summary>
        /// Gets a list of HTTP authentication parameter presets.
        /// </summary>
        public List<HttpAuthenticationParameterSet> Presets { get; protected set; }

        /// <summary>
        /// Gets the selected set of HTTP authentication parameters.
        /// </summary>
        [JsonIgnore]
        public HttpAuthenticationParameterSet SelectedPreset { get; protected set; }

        /// <summary>
        /// Gets a list of supported HTTP authentication methods.
        /// </summary>
        public List<IHttpAuthenticationClient> SupportedAuthenticationClients { get; protected set; }

        /// <summary>
        /// Get information about the current client operations.
        /// </summary>
        public object State { get; protected set; }

        #endregion

        #region Constructors

        public OnlineServiceClientBase(Uri uri)
        {
            Uri = uri;
            Presets = new List<HttpAuthenticationParameterSet>();
            Features = new List<Resource>();
            SupportedAuthenticationClients = new List<IHttpAuthenticationClient>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializeses additional HTTP request query parameters from a preset ID which is provided via a query parameter.
        /// </summary>
        /// <param name="request">Nancy HTTP request.</param>
        public void InitializeQueryParametersFromPreset(Request request)
        {
            if (!string.IsNullOrEmpty(request.Query["presetId"]))
            {
                string presetId = request.Query["presetId"];

                SelectedPreset = Presets.FirstOrDefault(p => p.Id == presetId);

                if (SelectedPreset != null)
                {
                    foreach (string key in SelectedPreset.Parameters.Keys)
                    {
                        if(string.IsNullOrEmpty(request.Query[key]))
                        {
                            request.Query[key] = SelectedPreset.Parameters[key];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the provides HTTP request query parameters are sane and may correct errors.
        /// </summary>
        /// <param name="request">Nancy HTTP request.</param>
        public virtual void SanitizeQueryParameters(Request request)
        {
        }

        /// <summary>
        /// Return a HTTP authentication client with a given state.
        /// </summary>
        /// <param name="state">A HTTP authentication client state.</param>
        /// <returns>The first client with the given state, <c>null</c> otherwise.</returns>
        public IHttpAuthenticationClient TryGetAuthenticationClient(HttpAuthenticationClientState state)
        {
            return SupportedAuthenticationClients.FirstOrDefault(a => a.ClientState == state);
        }

        /// <summary>
        /// Return a HTTP authentication client with a given state.
        /// </summary>
        /// <typeparam name="T">A subclass of HttpAuthenticationClient.</typeparam>
        /// <param name="state">A HTTP authentication client state.</param>
        /// <returns>The first client with the given state, <c>null</c> otherwise.</returns>
        public T TryGetAuthenticationClient<T>(HttpAuthenticationClientState state) where T : class
        {
            return TryGetAuthenticationClient(state) as T;
        }

        /// <summary>
        /// Return a HTTP authentication client from the 'authType'-query parameter the given HTTP request.
        /// </summary>
        /// <param name="state">A Nancy HTTP request.</param>
        /// <returns>The first client with the suitable URI, <c>null</c> otherwise.</returns>
        public IHttpAuthenticationClient TryGetAuthenticationClient(Request request)
        {
            string authType = request.Query.authType;

            return !string.IsNullOrEmpty(authType) ? SupportedAuthenticationClients.FirstOrDefault(a => a.Uri.AbsoluteUri == authType) : null;
        }

        /// <summary>
        /// Return a HTTP authentication client from the 'authType'-query parameter the given HTTP request.
        /// </summary>
        /// <typeparam name="T">A subclass of HttpAuthenticationClient.</typeparam>
        /// <param name="state">A Nancy HTTP request.</param>
        /// <returns>The first client with the suitable URI, <c>null</c> otherwise.</returns>
        public T TryGetAuthenticationClient<T>(Request request) where T : class
        {
            return TryGetAuthenticationClient(request) as T;
        }

        /// <summary>
        /// Install an authenticated online account into the given model.
        /// </summary>
        /// <param name="model">The model in which the account should be created.</param>
        /// <returns>A newly created instance of the <c>OnlineAccount</c> class.</returns>
        public OnlineAccount InstallAccount(IModel model)
        {
            if(model == null)
            {
                Logger.LogError("Cannot create account because model is not set.");

                return null;
            }

            // Create the online account.
            UriRef accountUri = new UriRef(GetAccountUri());

            OnlineAccount account = model.CreateResource<OnlineAccount>(accountUri);
            account.Id = accountUri.AbsoluteUri;
            account.Title = GetAccountTitle();
            account.Description = GetAccountDescription();

            if(accountUri == ServiceUrl)
            {

            }

            // Set the creation and modification date.
            DateTime now = DateTime.UtcNow;

            account.CreationTime = now;
            account.LastModificationTime = now;

            // Save the account credentials.
            IHttpAuthenticationClient auth = TryGetAuthenticationClient(HttpAuthenticationClientState.Authorized);

            if (auth != null)
            {
                account.ServiceClient = new Resource(Uri);
                account.ServiceUrl = new Resource(ServiceUrl);
                account.AuthenticationProtocol = new HttpAuthenticationProtocol(auth.Uri);

                foreach(KeyValuePair<string, string> parameter in auth.GetPersistableAuthenticationParameters())
                {
                    HttpAuthenticationParameter p = model.CreateResource<HttpAuthenticationParameter>();
                    p.Name = parameter.Key;
                    p.Value = parameter.Value;
                    p.Commit();

                    account.AuthenticationParameters.Add(p);
                }
            }

            // Check if there is already an account with the given ID.
            ISparqlQuery query = new SparqlQuery(@"ask where { ?account foaf:accountName @id }");
            query.Bind("@id", account.Id);

            ISparqlQueryResult result = model.ExecuteQuery(query);

            if (result.GetAnwser())
            {
                // We do not need to install the account twice.
                Logger.LogError("There is already an account with id {0}", account.Id);

                return null;
            }

            Person user = model.GetResources<Person>().FirstOrDefault();

            if (user == null)
            {
                Logger.LogError("Unable to retrieve user agent.");

                return null;
            }

            if(!account.IsSynchronized)
            {
                account.Commit();
            }

            user.Accounts.Add(account);
            user.Commit();

            Logger.LogInfo("Installed account with id: {0}", account.Id);

            return account;
        }

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        /// <remarks>
        /// This method is called when an authenticated account is being persisted.
        /// </remarks>
        protected abstract UriRef GetAccountUri();

        /// <summary>
        /// Gets the account title.
        /// </summary>
        /// <remarks>
        /// This method is called when an authenticated account is being persisted.
        /// </remarks>
        protected abstract string GetAccountTitle();

        /// <summary>
        /// Gets a description of the account.
        /// </summary>
        /// <remarks>
        /// This method is called when an authenticated account is being persisted.
        /// </remarks>
        protected virtual string GetAccountDescription()
        {
            IHttpAuthenticationClient authenticator = TryGetAuthenticationClient(HttpAuthenticationClientState.Authorized);

            if (authenticator != null)
            {
                return new Uri(authenticator.AuthorizeUrl).Authority;
            }

            return null;
        }

        #endregion
    }
}
