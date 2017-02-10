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

        private bool _isSynchronizing;

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
            if (!url.EndsWith(_path, StringComparison.InvariantCulture))
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

        public bool BeginSynchronization(IUserAgent user, OnlineAccount account)
        {
            if (!_isSynchronizing)
            {
                _isSynchronizing = true;

                PlatformProvider.Logger.LogInfo("Starting synchronization..");

                InitializeActivitySynchronizationState();

                // TODO: Here we can query the server here to see if another client is currently syncing.
                return true;
            }

            return false;
        }

        public IModelSynchronizationState EndSynchronization(IUserAgent user, OnlineAccount account)
        {
            IModelSynchronizationState state = ModelProvider.GetModelSynchronizationState(user);

            Task<int> stateTask = TryGetSynchronizationState(user, account);

            stateTask.Wait();

            int revision = stateTask.Result;

            if (revision > 0)
            {
                // Update the server revision for the online account.
                state.LastRemoteRevision = revision;
                state.Commit();
            }

            _isSynchronizing = false;

            PlatformProvider.Logger.LogInfo("Finished synchronization; Server at revision #{0}", revision);

            return state;
        }

        private void InitializeActivitySynchronizationState()
        {
            PlatformProvider.Logger.LogInfo("Initializing activity synchronization states..");

            IModel model = ModelProvider.GetActivities();

            SparqlUpdate update = new SparqlUpdate(@"
                WITH @model
                INSERT
                {
                    ?activity arts:synchronizationState ?state .

                    ?state a arts:ResourceSynchronizationState ; arts:lastRemoteRevision @undefined .
                }
                WHERE
                {
                    ?influence prov:activity | prov:hadActivity ?activity .

                    FILTER NOT EXISTS { ?activity arts:synchronizationState ?s . }

                    BIND(URI(CONCAT(STR(?activity), '#sync')) AS ?state) .
                }
            ");

            update.Bind("@model", model);
            update.Bind("@undefined", -1);

            model.ExecuteUpdate(update);
        }

        public async Task<int> TryGetSynchronizationState(IUserAgent user, OnlineAccount account)
        {
            if (account != null && account.ServiceUrl != null)
            {
                string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

                Uri url = new Uri(baseUrl + "/api/1.0/sync/");

                using (HttpClient client = GetHttpClient(account))
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    string json = await response.Content.ReadAsStringAsync();

                    dynamic data = JsonConvert.DeserializeObject(json);

                    int revision = data.revision;

                    return revision;
                }
            }

            return -1;
        }

        public async Task<SynchronizationChangeset> TryGetChangesetAsync(IUserAgent user, OnlineAccount account)
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
                                SynchronizationChangeset result = new SynchronizationChangeset((int)data.revision);

                                foreach (var row in data.changes)
                                {
                                    SynchronizationChangesetItem item = new SynchronizationChangesetItem();
                                    item.ActionType = SynchronizationActionType.Pull;
                                    item.ResourceUri = new UriRef(row.resource.ToString());
                                    item.ResourceType = new UriRef(row.resourceType.ToString());
                                    item.Revision = (int)row.revision;

                                    result.Add(item);
                                }

                                return result;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex);
                    }

                    Logger.LogInfo("{0} - Requested changeset from {1}", status, url);
                }
            }

            return null;
        }

        public async Task<bool> TryPullAsync(OnlineAccount account, Uri uri, Uri typeUri, int revision)
        {
            string type = typeUri.AbsoluteUri;

            switch (type)
            {
                case ART.Project:
                {
                    return await TryPullProjectAsync(account, uri, revision);
                }
                case ART.CreateFile:
                case ART.EditFile:
                case PROV.Activity:
                {
                    return await TryPullActivityAsync(account, uri, revision);
                }
            }

            return false;
        }

        private async Task<bool> TryPullProjectAsync(OnlineAccount account, Uri uri, int revision)
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

                        // Do not commit _after_ setting the revision.
                        project.Commit(revision);

                        Logger.LogInfo("{0} - {1}", response.StatusCode, url);

                        return true;
                    }
                }
            }

            return false;
        }

        private async Task<bool> TryPullActivityAsync(OnlineAccount account, Uri uri, int revision)
        {
            // TODO: Copied in large parts from ArtivityCloud server. Share the code.
            string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

            Uri url = new Uri(baseUrl + "/api/1.0/sync/activity?uri=" + uri);

            using (HttpClient client = GetHttpClient(account))
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode && response.Content.Headers.ContentLength > 0)
                {
                    // 1. Create a temporary archive file.
                    string archiveName = FileNameEncoder.Encode(uri.AbsoluteUri) + ".arta";

                    string[] path = new string[] { PlatformProvider.TempFolder, archiveName };

                    string archivePath = string.Join(Path.DirectorySeparatorChar.ToString(), path);

                    FileInfo archive = new FileInfo(archivePath);

                    try
                    {
                        // Delete any previous fils, if any.
                        if (File.Exists(archive.FullName))
                        {
                            File.Delete(archive.FullName);
                        }

                        // 3. Download the file into the temporary archive.
                        Stream httpStream = await response.Content.ReadAsStreamAsync();

                        using (StreamReader reader = new StreamReader(httpStream))
                        {
                            using (FileStream fileStream = File.Create(archive.FullName))
                            {
                                httpStream.CopyTo(fileStream);

                                fileStream.Flush();
                            }
                        }

                        // 4. Extract and import the archive.
                        ArchiveReader archiveReader = new ArchiveReader(PlatformProvider, ModelProvider);
                        archiveReader.Read(archive.FullName);

                        ArchiveManifest manifest = archiveReader.GetManifest(archive.FullName);

                        foreach (Uri entityUri in manifest.ExportedEntites)
                        {
                            IModel model = ModelProvider.GetActivities();

                            // Swap the 'local' for 'remote' in the revision property.
                            SparqlUpdate update = new SparqlUpdate(@"
                                WITH @model
                                DELETE { ?state arts:lastLocalRevision ?revision . }
                                INSERT { ?state arts:lastRemoteRevision ?revision . }
                                WHERE
                                {
                                    @activity arts:synchronizationState ?state .
                                    ?state arts:lastLocalRevision ?revision .
                                }
                            ");

                            update.Bind("@model", model);
                            update.Bind("@activity", uri);

                            model.ExecuteUpdate(update);
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        PlatformProvider.Logger.LogError(ex);
                    }
                    finally
                    {
                        if (File.Exists(archive.FullName))
                        {
                            File.Delete(archive.FullName);
                        }
                    }
                }

                return false;
            }
        }

        public async Task<bool> TryPushAsync(OnlineAccount account, Uri uri, Uri typeUri, int revision)
        {
            string type = typeUri.AbsoluteUri;

            switch (type)
            {
                case ART.Project:
                {
                    return await TryPushProjectAsync(account, uri, revision);
                }
                case ART.CreateFile:
                case ART.EditFile:
                case PROV.Activity:
                {
                    return await TryPushActivityAsync(account, uri, revision);
                }
            }

            return false;
        }

        private async Task<bool> TryPushProjectAsync(OnlineAccount account, Uri uri, int revision)
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

                    if (response.IsSuccessStatusCode)
                    {
                        int r = await GetLastRemoteRevision(response);

                        project.Commit(r);

                        return true;
                    }
                }
            }

            return false;
        }

        private async Task<bool> TryPushActivityAsync(OnlineAccount account, Uri uri, int revision)
        {
            string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

            Uri url = new Uri(baseUrl + "/api/1.0/sync/activity/");

            using (HttpClient client = GetHttpClient(account))
            {
                string archiveName = FileNameEncoder.Encode(uri.AbsoluteUri) + ".arta";
                string archivePath = Path.Combine(PlatformProvider.TempFolder, archiveName);

                FileInfo archive = new FileInfo(archivePath);

                try
                {
                    if(archive.Exists)
                    {
                        File.Delete(archive.FullName);
                    }

                    Uri userUri = new UriRef(PlatformProvider.Config.Uid);

                    ActivityArchiveWriter writer = new ActivityArchiveWriter(PlatformProvider, ModelProvider);

                    await writer.WriteAsync(userUri, uri, archive.FullName, DateTime.MinValue);

                    using (FileStream stream = File.Open(archive.FullName, FileMode.Open))
                    {
                        HttpContent content = new StreamContent(stream);
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/artivity+zip");

                        MultipartFormDataContent form = new MultipartFormDataContent();
                        form.Add(content, "file", archive.Name + archive.Extension);

                        HttpResponseMessage response = await client.PostAsync(url, form);

                        if(response.IsSuccessStatusCode)
                        {
                            int r = await GetLastRemoteRevision(response);

                            IModel model = ModelProvider.GetActivities();

                            SparqlUpdate update = new SparqlUpdate(@"
                                WITH @model
                                DELETE { ?state arts:lastRemoteRevision @undefined . }
                                INSERT { ?state arts:lastRemoteRevision @revision . }
                                WHERE { @activity arts:synchronizationState ?state . }
                            ");

                            update.Bind("@model", model);
                            update.Bind("@activity", uri);
                            update.Bind("@undefined", -1);
                            update.Bind("@revision", r);

                            model.ExecuteUpdate(update);
                        }

                        return response.IsSuccessStatusCode;
                    }
                }
                catch(Exception ex)
                {
                    Logger.LogError(ex);

                    return false;
                }
                finally
                {
                    if (archive.Exists)
                    {
                        File.Delete(archive.FullName);
                    }
                }
            }
        }

        private async Task<int> GetLastRemoteRevision(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(json);

            int revision = data.revision;

            return revision;
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
