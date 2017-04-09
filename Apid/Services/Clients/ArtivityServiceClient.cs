﻿// LICENSE:
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
using Newtonsoft.Json.Linq;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artivity.Apid.Accounts
{
    /// <summary>
    /// A helper class for installing an authenticated Artivity online account.
    /// </summary>
    public class ArtivityServiceClient : OnlineServiceClientBase, IArtivityServiceSynchronizationClient
    {
        #region Members

        private IPlatformProvider _platformProvider;

        private IModelProvider _modelProvider;

        private IArtivityServiceSynchronizationProvider _synchronizer;

        private bool _isSynchronizing;

        private const string _path = "/api/1.0/auth/login";

        private Uri _socketUrl = new Uri("ws://192.168.0.109:8082/");

        private ClientWebSocket _socket;

        private UTF8Encoding _encoder = new UTF8Encoding();

        #endregion

        #region Constructors

        public ArtivityServiceClient(IModelProvider modelProvider, IPlatformProvider platformProvider, IArtivityServiceSynchronizationProvider synchronizer)
            : base(new UriRef("http://artivity.online"), modelProvider, platformProvider)
        {
            Title = "Artivity";

            ClientFeatures.Add(artf.SynchronizeUser);
            ClientFeatures.Add(artf.SynchronizeActivities);

            _platformProvider = platformProvider;
            _modelProvider = modelProvider;

            if (synchronizer != null)
            {
                _synchronizer = synchronizer;
                _synchronizer.RegisterClient(this);

                TryConnectClientAsync();
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

        public async Task TryConnectClientAsync()
        {
            _socket = new ClientWebSocket();
            _socket.Options.AddSubProtocol("sync");
            _socket.Options.KeepAliveInterval = TimeSpan.FromDays(1);

            await _socket.ConnectAsync(_socketUrl, CancellationToken.None);

            await Task.WhenAll(ReceiveMessage());
        }

        private async Task ReceiveMessage()
        {
            byte[] buffer = new byte[1024];

            while (_socket.State == WebSocketState.Open)
            {
                var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);

                    Logger.LogInfo("Closed socket connection to {0}", _socketUrl);
                }
                else if(!_isSynchronizing)
                {
                    string json = _encoder.GetString(buffer);

                    dynamic data = JObject.Parse(json);

                    int revision = data.revision;

                    Logger.LogInfo("Server at revision #{0}", revision);

                    // TODO: Check if client sync is necessary.
                    _synchronizer.TrySynchronize();
                }
            }
        }

        public bool BeginSynchronization(IPerson user, OnlineAccount account)
        {
            if (!_isSynchronizing)
            {
                _isSynchronizing = true;

                PlatformProvider.Logger.LogInfo("Starting synchronization..");

                //InitializeActivitySynchronizationState();

                // TODO: Here we can query the server here to see if another client is currently syncing.
                return true;
            }

            return false;
        }

        public IModelSynchronizationState EndSynchronization(IPerson user, OnlineAccount account)
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

        public async Task<int> TryGetSynchronizationState(IPerson user, OnlineAccount account)
        {
            if (account != null && account.ServiceUrl != null)
            {
                string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

                Uri url = new Uri(baseUrl + "/api/1.0/sync/");

                using (HttpClient client = GetHttpClient(account))
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);

                        string json = await response.Content.ReadAsStringAsync();

                        dynamic data = JsonConvert.DeserializeObject(json);

                        int revision = data.revision;

                        return revision;
                    }
                    catch(Exception ex)
                    {
                        Logger.LogError(ex);
                    }
                }
            }

            return -1;
        }

        public async Task<SynchronizationChangeset> TryGetChangesetAsync(IPerson user, OnlineAccount account)
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
            if (IsInstanceOf(uri, art.Project))
            {
                return await TryPullProjectAsync(account, uri, revision);
            }
            else if (IsInstanceOf(uri, prov.Person))
            {
                return await TryPullPersonAsync(account, uri, revision);
            }
            else if (IsInstanceOf(uri, prov.Entity))
            {
                return await TryPullEntityAsync(account, uri, revision);
            }
            else
            {
                return false;
            }
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

                        if (data.Name != null)
                        {
                            project.Title = data.Name;
                        }

                        if(data.CreationDate != null)
                        {
                            project.CreationTimeUtc = data.CreationDate;
                        }

                        if(data.ModificationDate != null)
                        {
                            project.ModificationTimeUtc = data.ModificationDate;
                        }

                        if (data.DeletionDate != null)
                        {
                            project.DeletionTimeUtc = data.DeletionDate;
                        }

                        // Do not commit _after_ setting the revision.
                        project.Commit(revision);

                        return true;
                    }
                }
            }

            return false;
        }

        private async Task<bool> TryPullPersonAsync(OnlineAccount account, Uri uri, int revision)
        {
            string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

            Uri url = new Uri(baseUrl + "/api/1.0/sync/agents/persons?agentUri=" + uri.AbsoluteUri);

            using (HttpClient client = GetHttpClient(account))
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    dynamic data = JsonConvert.DeserializeObject(json);

                    if (data != null)
                    {
                        IModel model = ModelProvider.GetAgents();

                        Person person;

                        if (model.ContainsResource(uri))
                        {
                            person = model.GetResource<Person>(uri);
                        }
                        else
                        {
                            person = model.CreateResource<Person>(uri);
                        }

                        if (data.Name != null)
                        {
                            person.Name = data.Name;
                        }

                        if(data.EmailAddress != null)
                        {
                            person.EmailAddress = data.EmailAddress;
                        }

                        if (data.CreationDate != null)
                        {
                            person.CreationTimeUtc = data.CreationDate;
                        }

                        if (data.ModificationDate != null)
                        {
                            person.ModificationTimeUtc = data.ModificationDate;
                        }

                        if (data.DeletionDate != null)
                        {
                            person.DeletionTimeUtc = data.DeletionDate;
                        }

                        // Do not commit _after_ setting the revision.
                        person.Commit(revision);

                        return true;
                    }
                }
            }

            return false;
        }

        private async Task<bool> TryPullEntityAsync(OnlineAccount account, Uri uri, int revision)
        {
            // TODO: Copied in large parts from ArtivityCloud server. Share the code.
            string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

            ActivityArchiveWriter writer = new ActivityArchiveWriter(PlatformProvider, ModelProvider);
            string projectId = writer.GetProjectId(uri, _modelProvider.GetActivities());

            Uri url = new Uri(baseUrl + "/api/1.0/sync/projects/" + projectId + "/entity?uri=" + uri);

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
                            // Delete any exisiting remote revision triples.
                            SparqlUpdate update = new SparqlUpdate(@"
                                WITH @model
                                DELETE
                                {
                                    ?state arts:lastRemoteRevision ?remote .
                                    ?state arts:lastLocalRevision ?local .
                                }
                                INSERT
                                {
                                    ?state arts:lastRemoteRevision ?local .
                                }
                                WHERE
                                {
                                    @activity arts:synchronizationState ?state .
                                    ?state arts:lastLocalRevision ?local .
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
            if(IsInstanceOf(uri, art.Project))
            {
                return await TryPushProjectAsync(account, uri, revision);
            }
            else if(IsInstanceOf(uri, prov.Person))
            {
                return await TryPushPersonAsync(account, uri, revision);
            }
            else if(IsInstanceOf(uri, nfo.FileDataObject))
            {
                return await TryPushFileAsync(account, uri, revision);
            }
            else if (IsInstanceOf(uri, prov.Entity))
            {
                return await TryPushEntityAsync(account, uri, revision);
            }

            return false; 
        }

        private async Task<bool> TryPushProjectAsync(OnlineAccount account, Uri uri, int revision)
        {
            string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

            Uri url = new Uri(baseUrl + "/api/1.0/sync/project/" + Path.GetFileName(uri.AbsolutePath));

            using (HttpClient client = GetHttpClient(account))
            {
                IModel model = ModelProvider.GetAll();

                if (model.ContainsResource(uri))
                {
                    Project project = model.GetResource<Project>(uri);

                    string json = JsonConvert.SerializeObject(project);

                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        int r = await GetLastRemoteRevision(response);

                        model = ModelProvider.GetActivities();

                        if (model.ContainsResource(uri))
                        {
                            project = model.GetResource<Project>(uri);
                            project.Commit(r);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private async Task<bool> TryPushPersonAsync(OnlineAccount account, Uri uri, int revision)
        {
            string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

            Uri url = new Uri(baseUrl + "/api/1.0/sync/agents/persons");

            using (HttpClient client = GetHttpClient(account))
            {
                IModel model = ModelProvider.GetAll();

                if (model.ContainsResource(uri))
                {
                    Person person = model.GetResource<Person>(uri);

                    string json = JsonConvert.SerializeObject(person);

                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        int r = await GetLastRemoteRevision(response);

                        model = ModelProvider.GetAgents();

                        if (model.ContainsResource(uri))
                        {
                            person = model.GetResource<Person>(uri);
                            person.Commit(r);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private async Task<bool> TryPushEntityAsync(OnlineAccount account, Uri uri, int revision)
        {
            string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

            using (HttpClient client = GetHttpClient(account))
            {
                string archiveName = FileNameEncoder.Encode(uri.AbsoluteUri) + ".arta";
                string archivePath = Path.Combine(PlatformProvider.TempFolder, archiveName);

                FileInfo archive = new FileInfo(archivePath);

                try
                {
                    if (archive.Exists)
                    {
                        File.Delete(archive.FullName);
                    }

                    EntityArchiveWriter writer = new EntityArchiveWriter(PlatformProvider, ModelProvider);

                    string id = writer.GetProjectId(uri, _modelProvider.GetActivities());

                    if (string.IsNullOrEmpty(id))
                    {
                        return false;
                    }

                    Uri url = new Uri(baseUrl + "/api/1.0/sync/project/" + id + "/entity/");

                    Uri userUri = new UriRef(PlatformProvider.Config.Uid);

                    await writer.WriteAsync(userUri, uri, archive.FullName, DateTime.MinValue);

                    return await HandleArchiveUploadAsync(client, url, uri, archive);
                }
                catch (Exception ex)
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

        private async Task<bool> TryPushFileAsync(OnlineAccount account, Uri uri, int revision)
        {
            string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

            using (HttpClient client = GetHttpClient(account))
            {
                string archiveName = FileNameEncoder.Encode(uri.AbsoluteUri) + ".arta";
                string archivePath = Path.Combine(PlatformProvider.TempFolder, archiveName);

                FileInfo archive = new FileInfo(archivePath);

                try
                {
                    if (archive.Exists)
                    {
                        File.Delete(archive.FullName);
                    }

                    Uri userUri = new UriRef(PlatformProvider.Config.Uid);

                    FileSyncArchiveWriter writer = new FileSyncArchiveWriter(PlatformProvider, ModelProvider);

                    string id = writer.GetProjectId(uri, _modelProvider.GetActivities());

                    if (string.IsNullOrEmpty(id))
                    {
                        return false;
                    }

                    Uri url = new Uri(baseUrl + "/api/1.0/sync/project/" + id + "/file/");

                    List<FileInfo> renderings = writer.ListRenderings(uri).ToList();

                    await TryPushRenderingsAsync(account, renderings);

                    await writer.WriteAsync(userUri, uri, archive.FullName, DateTime.MinValue);

                    return await HandleArchiveUploadAsync(client, url, uri, archive);
                }
                catch (Exception ex)
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

        private async Task<bool> TryPushRenderingsAsync(OnlineAccount account, IEnumerable<FileInfo> renderings)
        {
            S3Uploader.Policy policy = null;

            foreach(var rendering in renderings)
            {
                if (policy == null) // We could reuse a policy here if we test for expiration. Caveat: How long should the grace period be?
                {
                    policy = await GetPolicy(account);
                }

                var parts = rendering.FullName.Split(Path.DirectorySeparatorChar);

                if (parts.Count() > 2)
                { 
                    string folder = parts[parts.Count() - 2];

                    await S3Uploader.Upload(policy, rendering, folder);
                }
                else
                {
                    throw new Exception(string.Format("Got unexpected file path {0}", rendering.FullName));
                }
            }

            return false;
        }

        private async Task<bool> HandleArchiveUploadAsync(HttpClient client, Uri url, Uri resourceUri, FileInfo archive)
        {
            using (FileStream stream = File.Open(archive.FullName, FileMode.Open))
            {
                HttpContent content = new StreamContent(stream);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/artivity+zip");

                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(content, "file", archive.Name + archive.Extension);

                HttpResponseMessage response = await client.PostAsync(url, form);

                if (response.IsSuccessStatusCode)
                {
                    int r = await GetLastRemoteRevision(response);

                    IModel model = ModelProvider.GetActivities();

                    SparqlUpdate update = new SparqlUpdate(@"
			            WITH @model
			            DELETE { ?state arts:lastRemoteRevision ?revision . }
			            INSERT { ?state arts:lastRemoteRevision @revision . }
			            WHERE
			            { 
				            @resource arts:synchronizationState ?state .

				            ?state arts:lastRemoteRevision ?revision .
			            }
		            ");

                    update.Bind("@model", model);
                    update.Bind("@resource", resourceUri);
                    update.Bind("@revision", r);

                    model.ExecuteUpdate(update);
                }

                return response.IsSuccessStatusCode;
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

            if(token != null && !string.IsNullOrEmpty(token.Value))
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

        protected async Task<S3Uploader.Policy> GetPolicy(OnlineAccount account)
        {
            if (account != null && account.ServiceUrl != null)
            {
                string baseUrl = account.ServiceUrl.Uri.AbsoluteUri;

                Uri url = new Uri(baseUrl + "/api/1.0/asset/policy");

                using (var c = GetHttpClient(account))
                {
                    var response = await c.GetStringAsync(url);

                    return JsonConvert.DeserializeObject<S3Uploader.Policy>(response);
                }
            }

            return null;
        }

        private bool IsInstanceOf(Uri resource, Class type)
        {
            ISparqlQuery query = new SparqlQuery(@"
                ASK FROM art: WHERE { @resource a @type . }
            ");

            query.Bind("@type", type);
            query.Bind("@resource", resource);

            // NOTE: Execute the query with inferencing enabled so that the 
            // query evaluates the class inheritance in the ontology.
            return _modelProvider.GetAll().ExecuteQuery(query, true).GetAnwser();
        }

        #endregion
    }
}
