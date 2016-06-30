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

using Artivity.DataModel;
using Artivity.Apid.Accounts;
using Artivity.Apid.Platforms;
using Nancy;
using Nancy.Responses;
using Nancy.IO;
using Semiodesk.Trinity;
using Semiodesk.Trinity.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using VDS.RDF;

namespace Artivity.Apid.Modules
{
    public class ApiModule : ModuleBase
    {
        #region Members

        private static object _modelLock = new object();

        #endregion

        #region Constructors

        public ApiModule(IModelProvider modelProvider, IPlatformProvider platform)
            : base("/artivity/api/1.0", modelProvider, platform)
        {
            Post["/activities"] = parameters =>
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    string data = reader.ReadToEnd();

                    // We start a new task so that the agent can continue its work as fast as possible.
                    new Task(() => PostActivities(data)).Start();

                    return HttpStatusCode.OK;
                }
            };

            // Get a list of all installed online accounts.
            Get["/accounts"] = parameters =>
            {
                return GetAccounts();
            };

            // Get a list of all supported online account types.
            Get["/accounts/providers"] = parameters =>
            {
                string providerId = Request.Query["providerId"];

                if (string.IsNullOrEmpty(providerId))
                {
                    return GetAccountProviders();
                }
                else
                {
                    return GetAccountProvider(providerId);
                }
            };

            Get["/accounts/oauth2/redirect"] = parameters =>
            {
                string providerId = Request.Query["providerId"];

                if (string.IsNullOrEmpty(providerId))
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetOAuth2AccountRedirectUrl(providerId);
            };

            Get["/accounts/oauth2/token"] = parameters =>
            {
                string providerId = Request.Query["providerId"];
                string code = Request.Query["code"];

                if (string.IsNullOrEmpty(providerId) || string.IsNullOrEmpty(code))
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return SendOAuth2AccessToken(providerId, code);
            };

            Get["/accounts/install"] = parameters =>
            {
                string providerId = Request.Query["providerId"];

                if (string.IsNullOrEmpty(providerId))
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return InstallAccount(providerId);
            };

            // Uninstall a account with a specific id.
            Get["/accounts/uninstall"] = parameters =>
            {
                string accountId = Request.Query["accountId"];

                if (string.IsNullOrEmpty(accountId))
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return UninstallAccount(accountId);
            };

            Get["/uris"] = parameters =>
            {
                string fileUrl = Request.Query["fileUrl"];

                if (!IsFileUrl(fileUrl))
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetUri(new UriRef(fileUrl));
            };

            Get["/files"] = parameters =>
            {
                string uri = Request.Query.uri;
                string url = Request.Query.url;
                string create = Request.Query.create;

                if (string.IsNullOrEmpty(uri))
                {
                    return HttpStatusCode.NotImplemented;
                }
                else if (IsUri(uri))
                {
                    if (string.IsNullOrEmpty(url))
                    {
                        return GetFile(new UriRef(uri));
                    }
                    else if(IsFileUrl(url) && !string.IsNullOrEmpty(create))
                    {
                        return CreateFile(new UriRef(uri), new Uri(url));
                    }
                }

                return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            };

            Get["/files/recent"] = parameters =>
            {
                return GetRecentlyUsedFiles();
            };

            Get["/activities"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetActivities(new UriRef(uri));
            };

            Get["/activities/clear"] = parameters =>
            {
                // TODO: We definitely need to add some kind of security here, i.e. a token.
                return ClearActivities();
            };

            Get["/influences"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetInfluences(new UriRef(uri));
            };

            Get["/influences/canvas"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                if (Request.Query.timestamp)
                {
                    string time = Request.Query.timestamp;

                    DateTimeOffset timestamp;

                    if (DateTimeOffset.TryParse(time.Replace(' ', '+'), out timestamp))
                    {
                        return GetCanvases(new UriRef(uri), timestamp.UtcDateTime);
                    }
                    else
                    {
                        return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }
                }
                else
                {
                    return GetCanvases(new UriRef(uri), DateTime.UtcNow);
                }
            };

            Get["/renderings"] = parameters =>
            {
                if(Request.Query.uri)
                {
                    string uri = Request.Query.uri;
                    string file = Request.Query.file;

                    if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                    {
                        return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }

                    if(!string.IsNullOrEmpty(file))
                    {
                        return GetRendering(new UriRef(uri), file);
                    }
                    else if(Request.Query.timestamp)
                    {
                        string time = Request.Query.timestamp;

                        DateTimeOffset timestamp;

                        if (DateTimeOffset.TryParse(time.Replace(' ', '+'), out timestamp))
                        {
                            return GetRenderings(Request.Query.fileUrl, timestamp.UtcDateTime);
                        }
                        else
                        {
                            return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                        }
                    }

                    return GetRenderings(new UriRef(uri));
                }
                else
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };

            Get["/renderings/path"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return HttpStatusCode.BadRequest;
                }

                bool create = Request.Query.create != null;

                return GetRenderOutputPath(new UriRef(uri), create);
            };

            Get["/stats/influences"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                if(Request.Query.timestamp)
                {
                    string time = Request.Query.timestamp;

                    DateTimeOffset timestamp;

                    if (DateTimeOffset.TryParse(time.Replace(' ', '+'), out timestamp))
                    {
                        return GetCompositionStats(new UriRef(uri), timestamp.UtcDateTime);
                    }
                    else
                    {
                        return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }
                }

                return GetCompositionStats(new UriRef(uri));
            };

            Post["/query"] = parameters =>
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    string query = reader.ReadToEnd();

                    if (string.IsNullOrEmpty(query))
                    {
                        return Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }

                    if (Request.Query.inference)
                    {
                        return ExecuteQuery(query, true);
                    }
                    else
                    {
                        return ExecuteQuery(query);
                    }
                }
            };
        }

        #endregion

        #region Methods

        private Response ExecuteQuery(string queryString, bool inferenceEnabled = false)
        {
            try
            {
                lock (_modelLock)
                {
                    IModel model = ModelProvider.GetAll();

                    if (model == null)
                    {
                        Logger.LogError(HttpStatusCode.InternalServerError, "Could not establish connection to model <{0}>", model.Uri);
                    }

                    SparqlQuery query = new SparqlQuery(queryString, false);

                    var bindings = model.ExecuteQuery(query, inferenceEnabled).GetBindings();

                    if (bindings != null)
                    {
                        return Response.AsJson(bindings.ToList());
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e);

                List<string> messages = new List<string>() { e.Message };

                if(e.InnerException != null)
                {
                    messages.Add(e.InnerException.Message);
                }

                return Response.AsJson(messages);
            }

            return null;
        }

        private void PostActivities(string data)
        {
            try
            {
                lock (_modelLock)
                {
                    IModel model = ModelProvider.GetActivities();

                    if (model == null)
                    {
                        Logger.LogError(HttpStatusCode.InternalServerError, "Could not establish connection to model <{0}>", model.Uri);
                    }

                    MemoryStream stream = new MemoryStream();

                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(data);
                    writer.Flush();

                    stream.Position = 0;

                    LoadTurtle(model, stream);
                }

                Logger.LogRequest(HttpStatusCode.OK, Request.Url, "POST", data);
            }
            catch (Exception e)
            {
                Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e, data);
            }
        }

        private void LoadTurtle(IModel model, Stream stream)
        {
            string connectionString = ModelProvider.NativeConnectionString;

            using (StreamReader reader = new StreamReader(stream))
            {
                string data = reader.ReadToEnd();

                using (VDS.RDF.Storage.VirtuosoManager m = new VDS.RDF.Storage.VirtuosoManager(connectionString))
                {
                    using (VDS.RDF.Graph graph = new VDS.RDF.Graph())
                    {
                        IRdfReader parser = dotNetRDFStore.GetReader(RdfSerializationFormat.N3);
                        parser.Load(graph, new StringReader(data));

                        graph.BaseUri = model.Uri;

                        m.UpdateGraph(model.Uri, graph.Triples, new List<Triple>());
                    }
                }
            }
        }

        private Response GetAccounts()
        {
            IModel model = ModelProvider.GetAgents();

            List<OnlineAccount> accounts = model.GetResources<OnlineAccount>(true).ToList();

            return Response.AsJson(accounts);
        }

        private Response GetAccountProviders()
        {
            return Response.AsJson(OnlineAccountFactory.GetRegisteredProviders());
        }

        private Response GetAccountProvider(string providerId)
        {
            try
            {
                return Response.AsJson(OnlineAccountFactory.GetProvider(providerId));
            }
            catch(KeyNotFoundException)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        private Response GetOAuth2AccountRedirectUrl(string providerId)
        {
            OAuth2AccountProvider provider = OnlineAccountFactory.GetProvider(providerId) as OAuth2AccountProvider;

            if (provider != null)
            {
                string redirectUrl = string.Format("http://localhost:8262/artivity/api/1.0/accounts/oauth2/token?providerId={0}", providerId);

                return Response.AsRedirect(provider.GetAuthorizationRequestUrl(redirectUrl));
            }
            else
            {
                return HttpStatusCode.MethodNotAllowed;
            }
        }

        private Response SendOAuth2AccessToken(string providerId, string code)
        {
            OAuth2AccountProvider provider = OnlineAccountFactory.GetProvider(providerId) as OAuth2AccountProvider;

            if (provider != null)
            {
                provider.Authorize(ModelProvider.AgentsModel, code);

                return HttpStatusCode.Accepted;
            }
            else
            {
                return HttpStatusCode.MethodNotAllowed;
            }
        }

        private Response InstallAccount(string providerId)
        {
            return HttpStatusCode.NotImplemented;
        }

        private Response UninstallAccount(string accountId)
        {
            Person user = ModelProvider.AgentsModel.GetResources<Person>().FirstOrDefault();

            if (user == null)
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, "Unable to retrieve user agent.");
            }

            OnlineAccount account = user.Accounts.FirstOrDefault(a => a.Id == accountId);

            if (account == null)
            {
                return Logger.LogInfo(HttpStatusCode.BadRequest, "Did not find account with id {0}", accountId);
            }

            ModelProvider.AgentsModel.DeleteResource(account);

            user.Accounts.Remove(account);
            user.Commit();

            return Logger.LogInfo(HttpStatusCode.OK, "Uninstalled account: {0}", accountId);
        }

        private Response GetRecentlyUsedFiles()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    MAX(?time) as ?time ?uri ?label
                WHERE
                {
                    {
                        SELECT
                            MAX(?time) as ?time ?label
                        WHERE
                        {
                            ?file rdfs:label ?label .
                            ?file nie:lastModified ?time .
                        }
                        GROUP BY ?label
                    }

                    ?uri nie:isStoredAs ?file .

                    ?file rdfs:label ?label .
                    ?file nie:lastModified ?time .
                }
                ORDER BY DESC(?time) LIMIT 25");

            var bindings = ModelProvider.ActivitiesModel.GetBindings(query);

            return Response.AsJson(bindings);
        }

        private Response GetActivities(UriRef entityUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?startTime ?endTime MAX(?time) as ?maxTime ?agent ?agentColor
                WHERE
                {
                    ?activity prov:generated | prov:used @entity .
	                ?activity prov:startedAtTime ?startTime .
	                ?activity prov:endedAtTime ?endTime .
	                ?activity prov:qualifiedAssociation ?association .
	
	                ?association prov:hadRole art:SOFTWARE .
                    ?association prov:agent ?agent .
	                
                    ?agent art:hasColourCode ?agentColor .
	
	                ?influence prov:activity | prov:hadActivity ?activity .
	                ?influence a ?type .
                    ?influence prov:atTime ?time .
                }
                ORDER BY DESC(?startTime)");

            query.Bind("@entity", entityUri);

            var bindings = ModelProvider.GetAll().GetBindings(query).ToList();

            return Response.AsJson(bindings);
        }

        private Response ClearActivities()
        {
            ModelProvider.ActivitiesModel.Clear();

            return HttpStatusCode.OK;
        }

        private Response GetInfluences(UriRef entityUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    DISTINCT ?time ?uri ?type ?description ?agentColor ?layerName ?bounds ?renderUrl ?renderX ?renderY
                WHERE 
                {
                    ?activity prov:generated | prov:used @entity .
                    ?activity prov:qualifiedAssociation ?association .

                    ?association prov:hadRole art:SOFTWARE .
                    ?association prov:agent / art:hasColourCode ?agentColor .

                    ?uri a ?type .
                    ?uri prov:activity | prov:hadActivity ?activity .
                    ?uri prov:atTime ?time .

                    OPTIONAL { ?uri dces:description ?description . }
                    OPTIONAL { ?uri art:selectedLayer / rdfs:label ?layerName . }
                    OPTIONAL { ?uri art:hadBoundaries ?bounds . }
                    OPTIONAL
                    {
                        ?uri art:renderedAs ?rendering .

                        ?rendering nie:url ?renderUrl .
                        ?rendering art:region / art:x ?renderX .
                        ?rendering art:region / art:y ?renderY .
                    }
                }
                ORDER BY DESC(?time)");

            query.Bind("@entity", entityUri);

            var bindings = ModelProvider.GetAllActivities().GetBindings(query);

            return Response.AsJson(bindings);
        }

        private Response GetRenderings(UriRef entityUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
	                ?time ?fileName COALESCE(?x, 0) AS ?x COALESCE(?y, 0) AS ?y ?layerName ?layerZ ?boundsX ?boundsY ?boundsWidth ?boundsHeight
                WHERE 
                {
	                {
		                SELECT
			                ?time ?influence ?layerName COALESCE(?layerZ, 0)
		                WHERE
		                {
			                ?activity prov:generated | prov:used @entity .

			                ?influence prov:activity | prov:hadActivity ?activity .
			                ?influence prov:atTime ?time .
			
			                OPTIONAL
			                {
				                ?influence art:selectedLayer ?layer .

				                ?layer rdfs:label ?layerName .
				                ?layer art:z ?layerZ .
			                }
		                }
		                GROUP BY ?layerZ
	                }

	                ?influence art:renderedAs ?rendering .

	                ?rendering rdfs:label ?fileName .

	                OPTIONAL
	                {
		                ?rendering art:region / art:x ?x .
		                ?rendering art:region / art:y ?y .
	                }

	                OPTIONAL
	                {
		                ?influence art:hadBoundaries ?bounds .

		                ?bounds art:x ?boundsX .
		                ?bounds art:y ?boundsY .
		                ?bounds art:width ?boundsWidth .
		                ?bounds art:height ?boundsHeight .
	                }
                }
                ORDER BY DESC(?time)");

            query.Bind("@entity", entityUri);

            var bindings = ModelProvider.ActivitiesModel.GetBindings(query, true);

            return Response.AsJson(bindings);
        }

        private Response GetRenderings(UriRef entityUri, DateTime time)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?time ?url ?x ?y ?layerName ?layerZ ?boundsX ?boundsY ?boundsWidth ?boundsHeight
                WHERE 
                {
                    {
                        SELECT
                            MAX(?time) AS ?time ?layer ?layerName ?layerZ
                        WHERE
                        {
                            ?activity prov:generated | prov:used @entity .

			                ?influence prov:activity | prov:hadActivity ?activity .
			                ?influence prov:atTime ?time .
				            ?influence art:selectedLayer ?layer .

				            ?layer rdfs:label ?layerName .
				            ?layer art:z ?layerZ .

                            FILTER(?time <= @time)
                        }
                        GROUP BY ?layerZ
                    }

                    ?influence prov:atTime ?time .
                    ?influence art:selectedLayer ?layer .
                    ?influence art:renderedAs ?rendering .

                    ?rendering nie:url ?url .
                    ?rendering art:region / art:x ?x .
                    ?rendering art:region / art:y ?y .

                    OPTIONAL
                    {
                        ?influence art:hadBoundaries ?bounds .

                        ?bounds art:x ?boundsX .
                        ?bounds art:y ?boundsY .
                        ?bounds art:width ?boundsWidth .
                        ?bounds art:height ?boundsHeight .
                    }
                }");

            query.Bind("@entity", entityUri);
            query.Bind("@time", time);

            var bindings = ModelProvider.ActivitiesModel.GetBindings(query, true);

            return Response.AsJson(bindings);
        }

        private Response GetRendering(UriRef uri, string fileName)
        {
            string file = Path.Combine(GetRenderOutputPath(uri), fileName);

            if (File.Exists(file))
            {
                FileStream fileStream = new FileStream(file, FileMode.Open);

                StreamResponse response = new StreamResponse(() => fileStream, MimeTypes.GetMimeType(file));
                response.Headers["Allow-Control-Allow-Origin"] = "127.0.0.1";

                return response.AsAttachment(file);
            }
            else
            {
                return null;
            }
        }

        private string GetRenderOutputPath(UriRef entityUri)
        {
            var invalids = System.IO.Path.GetInvalidFileNameChars();
            var newName = String.Join("_", entityUri.AbsoluteUri.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');

            return Path.Combine(PlatformProvider.ThumbnailFolder, newName);
        }

        private Response GetRenderOutputPath(UriRef entityUri, bool createDirectory = false)
        {
            string path = GetRenderOutputPath(entityUri);

            if (createDirectory && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> result = new List<string>() { path };

            return Response.AsJson(result);
        }

        private string GetEntityUri(string path)
        {
            Uri fileUrl = new FileInfo(path).ToUriRef();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?entity
                WHERE
                {
                    ?entity nie:isStoredAs ?file .

                    ?file nie:url @fileUrl .
                    ?file nie:lastModified ?time .
                }
                ORDER BY DESC(?time) LIMIT 1
            ");

            query.Bind("@fileUrl", fileUrl.AbsoluteUri);

            IEnumerable<BindingSet> bindings = ModelProvider.ActivitiesModel.GetBindings(query);

            if (bindings.Any())
            {
                BindingSet binding = bindings.First();

                string uri = binding["entity"].ToString();

                if (!string.IsNullOrEmpty(uri))
                {
                    return uri;
                }
            }

            return null;
        }

        private Response GetCompositionStats(UriRef entityUri)
        {
            string queryString = @"
                SELECT
                    ?type count(?type) as ?count
                WHERE
                {
                    ?activity prov:generated | prov:used @entity .

	                ?influence a ?type .
	                ?influence prov:activity | prov:hadActivity ?activity .
                }";

            ISparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@entity", entityUri);

            IEnumerable<BindingSet> bindings = ModelProvider.ActivitiesModel.GetBindings(query);

            return Response.AsJson(bindings);
        }

        private Response GetCompositionStats(UriRef entityUri, DateTime time)
        {
            string queryString = @"
                SELECT
                    ?type count(?type) AS ?count
                WHERE
                {
                    ?activity prov:generated | prov:used @entity .

	                ?influence a ?type .
	                ?influence prov:activity | prov:hadActivity ?activity .
	                ?influence prov:atTime ?time .

                    FILTER (?time <= @time)
                }";

            ISparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@entity", entityUri);
            query.Bind("@time", time);

            List<BindingSet> bindings = ModelProvider.ActivitiesModel.GetBindings(query).ToList();

            return Response.AsJson(bindings);
        }

        private Response GetUri(Uri fileUrl)
        {
            string file = Path.GetFileName(fileUrl.LocalPath);
            string folder = Path.GetDirectoryName(fileUrl.LocalPath);

            if(string.IsNullOrEmpty(file) || string.IsNullOrEmpty(folder))
            {
                return Response.AsJson(new { });
            }

            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?uri
                WHERE
                {
                    ?uri nie:isStoredAs ?file .

                    ?file rdfs:label @fileName .
                    ?file nie:lastModified ?time .
                    ?file nfo:belongsToContainer ?folder .

                    ?folder nie:url @folderUrl .
                }
                ORDER BY DESC(?time) LIMIT 1");

            query.Bind("@fileName", file);
            query.Bind("@folderUrl", new Uri(folder));

            var bindings = ModelProvider.ActivitiesModel.GetBindings(query).FirstOrDefault();

            return Response.AsJson(bindings);
        }

        private Response GetFile(Uri entityUri)
        {
            string queryString = @"
                SELECT
                    ?uri ?folder ?label ?created ?lastModified
                WHERE
                {
                    @entity nie:isStoredAs ?uri .

                    ?uri rdfs:label ?label .
                    ?uri nie:created ?created .
                    ?uri nie:lastModified ?lastModified .
                    ?uri nfo:belongsToContainer / nie:url ?folder .
                }";

            ISparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@entity", entityUri);

            BindingSet bindings = ModelProvider.ActivitiesModel.GetBindings(query).FirstOrDefault();

            return Response.AsJson(bindings);
        }

        private Response CreateFile(UriRef uri, Uri url)
        {
            FileSystemMonitor.Instance.AddFile(uri, url);

            return HttpStatusCode.OK;
        }

        private Response GetCanvases(UriRef entityUri, DateTime time)
        {
            string queryString = @"
                SELECT
	                ?canvas COALESCE(?x, 0) AS ?x COALESCE(?y, 0) AS ?y ?width ?height ?lengthUnit
                WHERE
                {
	                {
		                SELECT
			                ?canvas MAX(?t) AS ?time
		                WHERE
		                {
			                ?activity prov:generated | prov:used @entity .

			                ?canvas a art:Canvas .
			                ?canvas ?qualifiedInfluence ?influence .

			                ?influence prov:activity | prov:hadActivity ?activity .
			                ?influence prov:atTime ?t .
			                ?influence art:hadBoundaries ?bounds .
			
			                FILTER(?t <= @time) .

			                FILTER NOT EXISTS
			                {
				                ?canvas prov:qualifiedInvalidation ?invalidation .
				
				                ?invalidation prov:atTime ?t2 .
				
				                FILTER(?t2 <= @time) .
			                }
		                }
	                }
	
	                ?canvas ?qualifiedInfluence ?influence .
	
	                ?influence prov:atTime ?time .
	                ?influence art:hadBoundaries / art:width ?width .
	                ?influence art:hadBoundaries / art:height ?height .
	                ?influence art:hadBoundaries / art:x ?x .
	                ?influence art:hadBoundaries / art:y ?y .
	                ?influence art:hadChange ?u .

	                OPTIONAL
	                {
		                ?u art:property art:lengthUnit .
		                ?u art:value ?lengthUnit .
	                }
                }
                ORDER BY DESC(?time)";
                
            ISparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@entity", entityUri);
            query.Bind("@time", time);
            
            List<BindingSet> bindings = ModelProvider.ActivitiesModel.GetBindings(query).ToList();

            return Response.AsJson(bindings);
        }

        #endregion
    }
}
