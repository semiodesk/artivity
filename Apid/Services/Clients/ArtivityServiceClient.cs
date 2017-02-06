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
//  Sebastian Faubel <sebastian@semiodesk.com>
//  Moritz Eberl <moritz@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2017

using Artivity.Api.IO;
using Artivity.Api.Platform;
using Artivity.Apid.Protocols.Atom;
using Artivity.Apid.Protocols.Authentication;
using Artivity.Apid.Synchronization;
using Artivity.DataModel;
using Nancy;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Accounts
{
    /// <summary>
    /// A helper class for installing an authenticated Artivity online account.
    /// </summary>
    public class ArtivityServiceClient : OnlineServiceClientBase, IArtivityServiceSynchronizationClient
    {
        #region Members

        private const string _path = "/api/1.0/auth/login";

        #endregion

        #region Constructors

        public ArtivityServiceClient(IModelProvider modelProvider, IPlatformProvider platformProvider, IArtivityServiceSynchronizationProvider syncProvider)
            : base(new UriRef("http://artivity.online"), modelProvider, platformProvider)
        {
            Title = "Artivity";

            ClientFeatures.Add(artf.SynchronizeUser);
            ClientFeatures.Add(artf.SynchronizeActivities);

            if (syncProvider != null)
            {
                syncProvider.RegisterClient(this);
            }

            SupportedAuthenticationClients.Add(new JwtAuthenticationClient());

            #if DEBUG
            HttpAuthenticationParameterSet localhost = new HttpAuthenticationParameterSet();
            localhost.Id = "localhost:8080";
            localhost.Parameters["authType"] = "http://localhost:8272/artivity/api/1.0/auth/jwt";
            localhost.Parameters["url"] = "http://localhost:8080" + _path;

            Presets.Add(localhost);

            SelectedPreset = localhost;
            #endif
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the provides HTTP request query parameters are sane and may correct errors.
        /// </summary>
        /// <param name="request">Nancy HTTP request.</param>
        public override void SanitizeQueryParameters(Request request)
        {
            base.SanitizeQueryParameters(request);

            string url = request.Query.url;

            // Save the service endpoint address.
            ServiceUrl = new Uri(url);

            // Issue the request against the Artivity specific /api/1.0/login path.
            if (!url.EndsWith(_path))
            {
                url.TrimEnd('/');
                url += _path;

                request.Query.url = url;
            }
        }

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        /// <remarks>
        /// This method is called when an authenticated account is being persisted.
        /// </remarks>
        protected override UriRef GetAccountUri()
        {
            BasicAuthenticationClient authenticator = TryGetAuthenticationClient<BasicAuthenticationClient>(HttpAuthenticationClientState.Authorized);

            if (authenticator != null)
            {
                UriBuilder uriBuilder = new UriBuilder(ServiceUrl);
                uriBuilder.UserName = authenticator.Username;
                uriBuilder.Path = uriBuilder.Path.TrimEnd('/') + _path;

                return new UriRef(uriBuilder.ToString());
            }

            return null;
        }

        /// <summary>
        /// Gets the account title.
        /// </summary>
        /// <remarks>
        /// This method is called when an authenticated account is being persisted.
        /// </remarks>
        protected override string GetAccountTitle()
        {
            return "Artivity";
        }

        public async Task<SynchronizationChangeset> TryGetChangesetAsync(Person user, OnlineAccount account)
        {
            if(account.ServiceUrl != null)
            {
                IModelSynchronizationState state = ModelProvider.GetModelSynchronizationState(user);

                int revision = state.LastRemoteRevision;
                string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

                Uri url = new Uri(baseUrl + "/api/1.0/sync/" + revision);

                using (HttpClient client = GetHttpClient(account))
                {
                    System.Net.HttpStatusCode status = System.Net.HttpStatusCode.ServiceUnavailable;

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);

                        status = response.StatusCode;

                        if (response.IsSuccessStatusCode)
                        {
                            string json = await response.Content.ReadAsStringAsync();

                            dynamic data = JsonConvert.DeserializeObject(json);

                            if (data != null)
                            {
                                SynchronizationChangeset result = new SynchronizationChangeset((int)data.counter);

                                foreach (var row in data.changes)
                                {
                                    SynchronizationChangesetItem item = new SynchronizationChangesetItem();
                                    item.ActionType = SynchronizationActionType.Pull;
                                    item.ResourceUri = new UriRef(row.resource.ToString());
                                    item.ResourceType = new UriRef(row.resourceType.ToString());
                                    item.Revision = (int)row.counter;

                                    result.Add(item);
                                }

                                return result;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    Logger.LogInfo("{0} - Requested changeset from {1}", status, url);
                }
            }

            return null;
        }

        public async Task<ResourceSynchronizationState> TryPullAsync(OnlineAccount account, Uri uri, Uri typeUri)
        {
            string type = typeUri.AbsoluteUri;

            switch (type)
            {
                default: return null;
                case ART.Project: return await TryPullProjectAsync(account, uri);
                case PROV.Activity: return await TryPullActivityAsync(account, uri);
            }
        }

        private async Task<ResourceSynchronizationState> TryPullProjectAsync(OnlineAccount account, Uri uri)
        {
            string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

            Uri url = new Uri(baseUrl + "/api/1.0/sync/project/" + Path.GetFileName(uri.AbsolutePath));

            using (HttpClient client = GetHttpClient(account))
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    dynamic data = JsonConvert.DeserializeObject(json);

                    if(data != null)
                    {
                        IModel model = ModelProvider.GetActivities();

                        Project project;

                        if (model.ContainsResource(uri))
                        {
                            project = model.GetResource<Project>(uri);
                        }
                        else
                        {
                            project = model.CreateResource<Project>(uri);
                        }

                        string name = data.Name;

                        if (!string.IsNullOrEmpty(name))
                        {
                            project.Name = name;
                        }

                        if (!project.IsSynchronized)
                        {
                            project.Commit();
                        }

                        Logger.LogInfo("{0} - {1}", response.StatusCode, url);

                        return project.SynchronizationState;
                    }
                }
            }

            return null;
        }

        private async Task<ResourceSynchronizationState> TryPullActivityAsync(OnlineAccount account, Uri uri)
        {
            throw new NotImplementedException();
        }

        public async Task<ResourceSynchronizationState> TryPushAsync(OnlineAccount account, Uri uri, Uri typeUri)
        {
            string type = typeUri.AbsoluteUri;

            switch (type)
            {
                default: return null;
                case ART.Project: return await TryPushProjectAsync(account, uri);
                case PROV.Activity: return await TryPushActivityAsync(account, uri);
            }
        }

        private async Task<ResourceSynchronizationState> TryPushProjectAsync(OnlineAccount account, Uri uri)
        {
            string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

            Uri url = new Uri(baseUrl + "/api/1.0/sync/project/" + Path.GetFileName(uri.AbsolutePath));

            using (HttpClient client = GetHttpClient(account))
            {
                IModel model = ModelProvider.GetActivities();

                if (model.ContainsResource(uri))
                {
                    Project project = model.GetResource<Project>(uri);

                    StringContent content = new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    Logger.LogInfo("{0} - {1}", response.StatusCode, url);

                    return response.IsSuccessStatusCode ? project.SynchronizationState : null;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<ResourceSynchronizationState> TryPushActivityAsync(OnlineAccount account, Uri uri)
        {
            throw new NotImplementedException();
        }

        protected HttpClient GetHttpClient(OnlineAccount account)
        {
            HttpAuthenticationParameter token = account.AuthenticationParameters.FirstOrDefault(p => p.Name == "token");

            if(token != null)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return client;
            }
            else
            {
                throw new Exception("Could not retrieve JWT token from account authentication parameters.");
            }
        }

        #endregion
    }
}
