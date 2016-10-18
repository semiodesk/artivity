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
using Newtonsoft.Json;

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
            Get["/uris"] = parameters =>
            {
                string fileUrl = Request.Query["fileUrl"];

                if (!IsFileUrl(fileUrl))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
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

                return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            };

            Get["/files/recent"] = parameters =>
            {
                return GetRecentlyUsedFiles();
            };

            Get["/influences"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetInfluences(new UriRef(uri));
            };

            Get["/influences/canvas"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
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
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }
                }
                else
                {
                    return GetCanvases(new UriRef(uri));
                }
            };

            Get["/influences/layers"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetLayers(new UriRef(uri));
            };

            Get["/thumbnails"] = parameters =>
            {
                if (Request.Query.entityUri)
                {
                    string uri = Request.Query.entityUri;

                    if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                    {
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }

                    if (Request.Query.exists)
                    {
                        return HasThumbnail(new UriRef(uri));
                    }
                    else
                    {
                        return GetThumbnail(new UriRef(uri));
                    }
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
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
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
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
                            return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                        }
                    }

                    return GetRenderings(new UriRef(uri));
                }
                else
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
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
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
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
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
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
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
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

            Post["/sparql"] = parameters =>
            {
                string query = Request.Form.query;

                if (string.IsNullOrEmpty(query))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                if (Request.Query.inference)
                {
                    return ExecuteQuery(query, true);
                }
                else
                {
                    return ExecuteQuery(query);
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
                        PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, "Could not establish connection to model <{0}>", model.Uri);
                    }

                    SparqlQuery query = new SparqlQuery(queryString, false);

                    var results = model.ExecuteQuery(query, inferenceEnabled).GetBindings();

                    if (results != null && results.Any())
                    {
                        var vars = results.First().Keys.ToList();
                        var bindings = new List<Dictionary<string, object>>();

                        foreach(BindingSet row in results)
                        {
                            var item = new Dictionary<string, object>();

                            foreach(KeyValuePair<string, object> column in row)
                            {
                                string type = column.Value is Uri ? "uri" : "literal";
                                string value = column.Value.ToString();

                                var b = new Dictionary<string, string>() { { "type", type }, { "value", value } };

                                if(type == "literal" && !(column.Value is string))
                                {
                                    Type valueType = column.Value.GetType();

                                    if(!valueType.IsAssignableFrom(typeof(DBNull)))
                                    {
                                        b["datatype"] = XsdTypeMapper.GetXsdTypeUri(valueType).ToString();
                                    }
                                }

                                item[column.Key] = b;
                            }

                            bindings.Add(item);
                        }

                        Dictionary<string, object> result = new Dictionary<string, object>();
                        result["head"] = new Dictionary<string, List<string>>() { { "vars", vars } };
                        result["results"] = new Dictionary<string, List<Dictionary<string, object>>> { { "bindings", bindings } };

                        Response response = Response.AsJson(result);
                        response.ContentType = "application/sparql-results+json";

                        return response;
                    }
                    else
                    {
                        Dictionary<string, object> result = new Dictionary<string, object>();
                        result["head"] = new Dictionary<string, List<string>>() { };
                        result["results"] = new Dictionary<string, List<Dictionary<string, object>>> { { "bindings", new List<Dictionary<string, object>>() } };

                        Response response = Response.AsJson(result);
                        response.ContentType = "application/sparql-results+json";

                        return response;
                    }
                }
            }
            catch (Exception e)
            {
                PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e);

                List<string> messages = new List<string>() { e.Message };

                if(e.InnerException != null)
                {
                    messages.Add(e.InnerException.Message);
                }

                return Response.AsJson(messages);
            }
        }

        private Response GetRecentlyUsedFiles()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT
	                ?t1 AS ?time
	                ?entity AS ?uri
	                ?label
                    SAMPLE(COALESCE(?agentColor, '#FF0000')) AS ?agentColor
                WHERE
                {
                    ?a1
                        prov:generated | prov:used ?entity ;
                        prov:endedAtTime ?t1 .

	                ?entity nie:isStoredAs [ rdfs:label ?label ] .
	
	                OPTIONAL
	                {
                        ?a2
                            prov:generated | prov:used ?entity ;
                            prov:endedAtTime ?t2 .
		
		                FILTER(?t1 > ?t2)
	                }
	
	                FILTER(!BOUND(?t2))

                    OPTIONAL
                    {
                        ?a1 prov:qualifiedAssociation / prov:agent / art:hasColourCode ?agentColor .
                    }
                }
                ORDER BY DESC(?t1) LIMIT 25");

            var bindings = ModelProvider.GetAll().GetBindings(query);
            PlatformProvider.Logger.LogInfo("Update");
            var resp = Response.AsJson(bindings);
            return resp;
        }

        private Response GetInfluences(UriRef entityUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT
                    ?uri
                    ?activity
                    ?time
                    ?type
                    ?description
                    ?comment
					?entity
					?entityType
                    ?property
                    ?layer
                    SAMPLE(COALESCE(?agentColor, '#FF0000')) AS ?agentColor
                    COALESCE(?x, 0) AS ?x
                    COALESCE(?y, 0) AS ?y
                    COALESCE(?w, 0) AS ?w
                    COALESCE(?h, 0) AS ?h
                    ?vx
                    ?vy
                    ?vw
                    ?vh
                WHERE 
                {
                    ?activity
                        prov:generated | prov:used @entity ;
                        prov:qualifiedAssociation ?association .

                    OPTIONAL
                    {
                        ?association prov:agent / art:hasColourCode ?agentColor .
                    }

                    ?uri
                        rdf:type ?type ;
                        prov:activity | prov:hadActivity ?activity ;
                        prov:atTime ?time .

                    OPTIONAL
                    {
                        ?uri dces:description ?description .
                    }

                    OPTIONAL
                    {
                        ?uri rdfs:comment ?comment .
                    }

                    OPTIONAL
                    {
                        ?uri art:hadBoundaries [
                            art:x ?x ;
                            art:y ?y ;
                            art:width ?w ;
                            art:height?h
                        ] .
                    }

                    OPTIONAL
                    {
                        ?uri art:hadViewport [
                            art:x ?vx ;
                            art:y ?vy ;
                            art:width ?vw ;
                            art:height?vh
                        ] .
                    }

                    OPTIONAL
                    {
						?uri art:hadChange [
                            art:entity ?entity;
                            art:property ?property ;
                        ] .

						?entity a ?entityType .

                        FILTER(?entityType != prov:Entity)
                    }

                    OPTIONAL
                    {
                        ?uri art:renderedAs [
                            art:renderedLayer ?layer ;
                        ] .
                    }
                }
                ORDER BY DESC(?time)");

            query.Bind("@entity", entityUri);

            IEnumerable<BindingSet> bindings = ModelProvider.GetAll().GetBindings(query);

            // Since SPARQL does not support to return nested values (1 influence -> n-changes)
            // we consolidate the changes made in an influence into a list.
            IList<Influence> influences = new List<Influence>();

            Influence lastInfluence = null;

            foreach (BindingSet b in bindings)
            {
                string uri = b["uri"].ToString();

                // Initialize the last influence.
                if(lastInfluence == null || lastInfluence.uri != uri)
                {
                    lastInfluence = new Influence();
                    lastInfluence.uri = uri;
                    lastInfluence.activity = b["activity"];
                    lastInfluence.type = b["type"];
                    lastInfluence.time = b["time"];
                    lastInfluence.description = b["description"];
                    lastInfluence.comment = b["comment"];
                    lastInfluence.layer = b["layer"];
                    lastInfluence.agentColor = b["agentColor"];
                    lastInfluence.x = b["x"];
                    lastInfluence.y = b["y"];
                    lastInfluence.w = b["w"];
                    lastInfluence.h = b["h"];
                    lastInfluence.vx = b["vx"];
                    lastInfluence.vy = b["vy"];
                    lastInfluence.vw = b["vw"];
                    lastInfluence.vh = b["vh"];

                    influences.Add(lastInfluence);
                }
                
                // Add the current change to the last influence.
                object entity = b["entity"];

                if (entity != null)
                {
                    object entityType = b["entityType"];
                    object property = b["property"];

                    Change change = new Change()
                    {
                        entity = entity.ToString(),
                        entityType = entityType.ToString(),
                        property = property.ToString()
                    };

                    lastInfluence.changes.Add(change);
                }
            }

            return Response.AsJson(influences);
        }

        private Response HasThumbnail(UriRef entityUri)
        {
            string file = Path.Combine(GetRenderOutputPath(entityUri), "thumbnail.png");

            return Response.AsJson(File.Exists(file));
        }

        private Response GetThumbnail(UriRef entityUri)
        {
            string file = Path.Combine(GetRenderOutputPath(entityUri), "thumbnail.png");

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

        private Response GetRenderings(UriRef entityUri)
        {
            return GetRenderings(entityUri, DateTime.UtcNow);
        }

        private Response GetRenderings(UriRef entityUri, DateTime time)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
	                ?time
                    ?type
                    ?file
                    ?layer
                    COALESCE(?x, 0) AS ?x
                    COALESCE(?y, 0) AS ?y
                    COALESCE(?w, 0) AS ?w
                    COALESCE(?h, 0) AS ?h
                WHERE 
                {
			        ?activity prov:generated | prov:used @entity .

			        ?influence
                        prov:activity | prov:hadActivity ?activity ;
                        prov:atTime ?time ;
                        art:renderedAs ?rendering .

                    ?rendering
                        rdf:type ?type ;
                        rdfs:label ?file .

                    OPTIONAL
                    {
                        ?rendering art:region [
                            art:x ?x ;
                            art:y ?y ;
                            art:width ?w ;
                            art:height ?h
                        ] .
                    }

                    OPTIONAL
                    {
                        ?rendering art:renderedLayer ?layer .
                    }

                    FILTER(?time <= @time)
                }
                ORDER BY DESC(?time)");

            query.Bind("@entity", entityUri);
            query.Bind("@time", time);

            var bindings = ModelProvider.GetActivities().GetBindings(query);

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
            string entityName = PlatformProvider.EncodeFileName(entityUri.AbsoluteUri);

            return Path.Combine(PlatformProvider.RenderingsFolder, entityName);
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

            IEnumerable<BindingSet> bindings = ModelProvider.GetActivities().GetBindings(query);

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

            IEnumerable<BindingSet> bindings = ModelProvider.GetActivities().GetBindings(query);

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

            List<BindingSet> bindings = ModelProvider.GetActivities().GetBindings(query).ToList();

            return Response.AsJson(bindings);
        }

        private Response GetUri(Uri fileUrl)
        {
            string file = Path.GetFileName(fileUrl.LocalPath);
            string folder = Path.GetDirectoryName(fileUrl.LocalPath);

            if(string.IsNullOrEmpty(file) || string.IsNullOrEmpty(folder))
            {
                PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);

                return Response.AsJson(new {});
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

            var bindings = ModelProvider.GetActivities().GetBindings(query).FirstOrDefault();

            PlatformProvider.Logger.LogRequest(HttpStatusCode.OK, Request);

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

                    OPTIONAL { ?uri nfo:belongsToContainer / nie:url ?folder . }
                }";

            ISparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@entity", entityUri);

            BindingSet bindings = ModelProvider.GetActivities().GetBindings(query).FirstOrDefault();

            return Response.AsJson(bindings);
        }

        private Response CreateFile(UriRef uri, Uri url)
        {
            PlatformProvider.AddFile(uri, url);

            return HttpStatusCode.OK;
        }

        private Response GetCanvases(UriRef entityUri)
        {
            return GetCanvases(entityUri, DateTime.UtcNow);
        }

        private Response GetCanvases(UriRef entityUri, DateTime maxTime)
        {
            string queryString = @"
                SELECT DISTINCT
	                ?time
	                ?type
	                ?canvas AS ?uri
	                ?property
	                ?value
	                COALESCE(?x, 0) AS ?x
	                COALESCE(?y, 0) AS ?y
	                COALESCE(?w, 0) AS ?w
	                COALESCE(?h, 0) AS ?h
                WHERE
                {
	                ?activity
                        prov:generated | prov:used @entity.

                    ?canvas a art:Canvas ;
                        ?qualifiedInfluence ?influence .

	                ?influence a ?type ;
		                prov:activity | prov:hadActivity ?activity ;
		                prov:atTime ?time .
	
                    FILTER (?time <= @maxTime)

                    OPTIONAL
                    {
                        ?influence art:hadChange [ 
                            art:property ?property ;
                            art:value ?value
                        ] .
                    }

                    OPTIONAL
                    {
                        ?influence art:hadBoundaries [
                            art:x ?x ;
                            art:y ?y ;
                            art:width ?w ;
                            art:height ?h
                        ] .
                    }
                }
                ORDER BY DESC(?time)";
                
            ISparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@entity", entityUri);
            query.Bind("@maxTime", maxTime);
            
            List<BindingSet> bindings = ModelProvider.GetActivities().GetBindings(query).ToList();

            return Response.AsJson(bindings);
        }

        private Response GetLayers(UriRef uriRef)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT
                    ?time
                    ?type
                    ?layer AS ?uri
                    ?property
                    ?value
                WHERE
                {
	                ?activity
                        prov:generated | prov:used @entity .

	                ?layer a art:Layer ;
                        ?qualifiedInfluence ?influence .

	                ?influence a ?type ;
		                prov:activity | prov:hadActivity ?activity ;
		                prov:atTime ?time .

                    OPTIONAL
                    {
                        ?influence art:hadChange ?change .

    	                ?change art:entity ?layer ;
    		                art:property ?property ;
    		                art:value ?value .

                        VALUES ?property
                        {
                            rdfs:label
                            art:aboveLayer
                            art:parentLayer
                        }
                    }
                }
                ORDER BY DESC(?time)");

            query.Bind("@entity", uriRef);

            IList<BindingSet> bindings = ModelProvider.GetActivities().GetBindings(query).ToList();

            return Response.AsJson(bindings);
        }

        #endregion
    }

    class Influence
    {
        [JsonIgnore]
        public string uri;

        public object activity;

        public object time;

        public object type;

        public object description;

        public object comment;

        public object layer;

        public object agentColor;

        public object x;

        public object y;

        public object w;

        public object h;

        public object vx;

        public object vy;

        public object vw;

        public object vh;

        public List<Change> changes = new List<Change>();
    }

    struct Change
    {
        public string entity;

        public string entityType;

        public string property;
    }
}
