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
using Artivity.Api.Platform;
using Artivity.Api.IO;
using Nancy;
using Nancy.Responses;
using Nancy.Extensions;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Artivity.Api.Modules
{
    public class ApiModule : ModuleBase
    {
        #region Members

        #endregion

        #region Constructors

        public ApiModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0", modelProvider, platformProvider)
        {
            Get["/uris"] = parameters =>
            {
                string fileUrl = Request.Query["fileUrl"];

                if (!IsFileUrl(fileUrl))
                {
                    return platformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
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

                return platformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            };

            Get["/files/recent"] = parameters =>
            {
                var settings = new GetFilesSettings() { OrderBy = OrderBy.Time, Offset = 0, Limit = 100 };
                return GetFiles(settings);
            };

            Get["/files/project"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetProjectFilese(new UriRef(uri));
            };

            Get["/files/publish"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return PublishMostRecentEntity(uri);
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

            Get["/renderings/canvases"] = parameters =>
            {
                if (Request.Query.entity)
                {
                    string entity = Request.Query.entity;

                    if (string.IsNullOrEmpty(entity) || !IsUri(entity))
                    {
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }

                    return GetCanvasRenderingsFromEntity(new UriRef(entity));
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


        }


        #endregion

        #region Methods
        enum Ordering { Ascending, Descending }
        enum OrderBy { None, Name, Time, Size }
        enum FilterBy { None, SoftwareAgent, UserAgent, Project }

        class GetFilesSettings
        {
            public Ordering Ordering = Ordering.Descending;
            public OrderBy OrderBy;
            public FilterBy FilterBy;
            public string FilterValue;
            public int Offset = 0;
            public int Limit = 100;

            public string GetOrderClause()
            {
                string ordering = "";
                if (Ordering == ApiModule.Ordering.Ascending)
                    ordering = "ASC";
                else
                    ordering = "DESC";

                switch (OrderBy)
                {
                    case ApiModule.OrderBy.None:
                        return "";

                    case ApiModule.OrderBy.Name:
                        return string.Format("ORDER BY {0}(?label) ", ordering);

                    case ApiModule.OrderBy.Time:
                        return string.Format("ORDER BY {0}(?time) ", ordering);

                    case ApiModule.OrderBy.Size:
                        return "";


                    default:
                        return "";
                }

            }
            public string GetFilterClause() 
            {
                if (FilterBy == ApiModule.FilterBy.None)
                    return "";
                Uri u = new Uri(FilterValue);
                switch( FilterBy )
                {
                    case ApiModule.FilterBy.SoftwareAgent:
                        return string.Format("FILTER(?agent = <{0}>)", u.AbsoluteUri);

                    case ApiModule.FilterBy.UserAgent:
                        return "";

                    case ApiModule.FilterBy.Project:
                        return string.Format("<{0}> prov:hadMember ?entity .", u.AbsoluteUri);

                    default:
                        return "";
                }
 
            }
            public string GetLimitClause() { return string.Format("LIMIT {0}", Limit); }
            public string GetOffsetClause() { return string.Format("OFFSET {0}", Offset); }
        }

        private Response GetFiles(GetFilesSettings settings)
        {
            string OrderClause = settings.GetOrderClause();
            string FilterClause = settings.GetFilterClause();
            string LimitClause = settings.GetLimitClause();
            string OffsetClause = settings.GetOffsetClause();
            string queryString = @"
                SELECT DISTINCT
                    ?label 
                    ?file as ?uri
                    ?q as ?entityUri
                    SAMPLE(?p) as ?thumbnail 
                    MAX(?t) as ?time 
                    SAMPLE(COALESCE(?agentColor, '#cecece')) AS ?agentColor
                    WHERE {{
                            ?a prov:generated | prov:used ?entity ;
                            prov:endedAtTime ?ended ;
                            prov:startedAtTime ?started ;
                            prov:qualifiedAssociation / prov:agent ?agent.
  
                    ?agent a prov:SoftwareAgent.
                    ?entity nie:isStoredAs ?file.
                    ?file rdfs:label ?label .
                    BIND( STRBEFORE( STR(?entity), '#' ) as ?e ).
                    BIND( if(?e != '', ?e, str(?entity)) as ?q).
                    BIND( CONCAT('http://localhost:8262/artivity/api/1.0/thumbnails?entityUri=', ?q) as ?p ).
                    BIND(if(bound(?ended), ?ended, ?started) as ?t).
                    OPTIONAL{{
                        ?agent art:hasColourCode ?agentColor .
                    }}
                    FILTER NOT EXISTS {{
                      ?entity prov:qualifiedRevision / prov:entity ?x .
                    }}
                    {0}
                    }}GROUP BY ?label ?file ?q ?time {1} {2} {3}";
            queryString = string.Format(queryString, FilterClause, OrderClause, LimitClause, OffsetClause);
            ISparqlQuery query = new SparqlQuery(queryString);

            var bindings = ModelProvider.GetAll().GetBindings(query);

            return Response.AsJsonSync(bindings);
        }

        private Response GetProjectFilese(UriRef projectUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT
	                ?t1 AS ?time
	                ?file AS ?uri
	                ?label
                    SAMPLE(COALESCE(?agentColor, '#FF0000')) AS ?agentColor
                WHERE
                {
                    ?a1
                        prov:generated | prov:used ?entity ;
                        prov:endedAtTime ?t1 .

	                ?entity nie:isStoredAs ?file .

                    ?file rdfs:label ?label .

                    <" + projectUri.AbsoluteUri + @"> prov:hadMember ?entity .
	
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
                ORDER BY DESC(?t1) LIMIT 100");

            var bindings = ModelProvider.GetAll().GetBindings(query);

            return Response.AsJsonSync(bindings);
        }

        private Response GetInfluences(UriRef fileUri)
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
                        prov:generated | prov:used ?e ;
                        prov:qualifiedAssociation ?association .
                    ?e nie:isStoredAs @fileUri.

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

            query.Bind("@fileUri", fileUri);

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

            return Response.AsJsonSync(influences);
        }

        private Response HasThumbnail(UriRef entityUri)
        {
            string file = Path.Combine(PlatformProvider.GetRenderOutputPath(entityUri), "thumbnail.png");

            return Response.AsJsonSync(File.Exists(file));
        }

        private Response GetThumbnail(UriRef entityUri)
        {
            string file = Path.Combine(PlatformProvider.GetRenderOutputPath(entityUri), "thumbnail.png");

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

        private Response GetRenderings(UriRef fileUri)
        {
            return GetRenderings(fileUri, DateTime.UtcNow);
        }

        private Response GetRenderings(UriRef fileUri, DateTime time)
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
			        ?activity prov:generated | prov:used  ?e.
                    ?e nie:isStoredAs @fileUri.
                    

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

            query.Bind("@fileUri", fileUri);
            query.Bind("@time", time);

            var bindings = ModelProvider.GetActivities().GetBindings(query);

            return Response.AsJsonSync(bindings);
        }

        private Response GetRendering(UriRef uri, string fileName)
        {
            string file = Path.Combine(PlatformProvider.GetRenderOutputPath(uri), fileName);

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

        private Response GetRenderOutputPath(UriRef entityUri, bool createDirectory = false)
        {
            string path = PlatformProvider.GetRenderOutputPath(entityUri);

            if (createDirectory && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> result = new List<string>() { path };

            return Response.AsJsonSync(result);
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

        private Response GetCompositionStats(UriRef fileUri)
        {
            string queryString = @"
                SELECT
                    ?type count(?type) as ?count
                WHERE
                {
                    ?activity prov:generated | prov:used ?e.
                    ?e nie:isStoredAs @fileUri.

	                ?influence a ?type .
	                ?influence prov:activity | prov:hadActivity ?activity .
                }";

            ISparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@fileUri", fileUri);

            IEnumerable<BindingSet> bindings = ModelProvider.GetActivities().GetBindings(query);

            return Response.AsJsonSync(bindings);
        }

        private Response GetCompositionStats(UriRef fileUri, DateTime time)
        {
            string queryString = @"
                SELECT
                    ?type count(?type) AS ?count
                WHERE
                {
                    ?activity prov:generated | prov:used ?e.
                    ?e nie:isStoredAs @fileUri.

	                ?influence a ?type .
	                ?influence prov:activity | prov:hadActivity ?activity .
	                ?influence prov:atTime ?time .

                    FILTER (?time <= @time)
                }";

            ISparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@fileUri", fileUri);
            query.Bind("@time", time);

            List<BindingSet> bindings = ModelProvider.GetActivities().GetBindings(query).ToList();

            return Response.AsJsonSync(bindings);
        }

        private Response GetUri(Uri fileUrl)
        {
            string file = Path.GetFileName(fileUrl.LocalPath);
            string folder = Path.GetDirectoryName(fileUrl.LocalPath);

            if(string.IsNullOrEmpty(file) || string.IsNullOrEmpty(folder))
            {
                PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);

                return Response.AsJsonSync(new {});
            }

            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?uri ?file
                WHERE
                {
                    ?uri nie:isStoredAs ?file .

                    ?file rdfs:label @fileName .
                    ?file nie:lastModified ?time .
                    ?file nfo:belongsToContainer ?folder .

                    ?folder nie:url @folderUrl .
                    
                    FILTER NOT EXISTS{ ?var prov:invalidated ?uri }
                   
                }
                ORDER BY DESC(?time) LIMIT 1");

            query.Bind("@fileName", file);
            query.Bind("@folderUrl", new Uri(folder));

            var bindings = ModelProvider.GetActivities().GetBindings(query).FirstOrDefault();

            PlatformProvider.Logger.LogRequest(HttpStatusCode.OK, Request);

            return Response.AsJsonSync(bindings);
        }

        private Response GetFile(Uri fileUri)
        {
            string queryString = @"
                SELECT
                    ?uri ?folder ?label ?created ?lastModified
                WHERE
                {
                    BIND(@uri as ?uri).

                    ?uri rdfs:label ?label .
                    ?uri nie:created ?created .
                    ?uri nie:lastModified ?lastModified .

                    OPTIONAL { ?uri nfo:belongsToContainer / nie:url ?folder . }
                }";

            ISparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@uri", fileUri);

            BindingSet bindings = ModelProvider.GetActivities().GetBindings(query).FirstOrDefault();

            return Response.AsJsonSync(bindings);
        }

        private Response CreateFile(UriRef uri, Uri url)
        {
            PlatformProvider.AddFile(uri, url);

            return HttpStatusCode.OK;
        }

        /// <summary>
        /// This function publishes the most recent version of a file.
        /// </summary>
        /// <param name="fileUri"></param>
        /// <returns></returns>
        private Response PublishMostRecentEntity(string fileUri)
        {
            bool res = false;
            var uri = new UriRef(fileUri);
            IModel Model = ModelProvider.GetActivities();

            string query = @"
            DESCRIBE ?entity 
            WHERE 
            {
                ?entity nie:isStoredAs @fileUri .
                FILTER NOT EXISTS 
                {
                    ?v prov:qualifiedRevision / prov:entity ?entity
                }
            }
            ";
            SparqlQuery q = new SparqlQuery(query);
            q.Bind("@fileUri", uri);
            
            var entity = Model.ExecuteQuery(q).GetResources<Entity>().First();

            if (entity != null)
            {
                entity.Publish = true;
                entity.Commit();
                res = true;

                // Now we make sure the file has the synchronization state attached.
                var file = Model.GetResource<FileDataObject>(uri);
                if (file.SynchronizationState == null)
                    file.Commit();
            }
            else
            {
            }

            return Response.AsJsonSync(res);
        }


        private Response GetCanvases(UriRef fileUri)
        {
            return GetCanvases(fileUri, DateTime.UtcNow);
        }

        private Response GetCanvases(UriRef fileUri, DateTime maxTime)
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
                        prov:generated | prov:used ?e.
                    ?e nie:isStoredAs @fileUri.

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
            query.Bind("@fileUri", fileUri);
            query.Bind("@maxTime", maxTime);
            
            List<BindingSet> bindings = ModelProvider.GetActivities().GetBindings(query).ToList();

            return Response.AsJsonSync(bindings);
        }

        private Response GetCanvasRenderingsFromEntity(UriRef entityUri)
        {
            string baseUri = "http://localhost:8262/artivity/api/1.0/renderings?uri=";
            string queryString = @"
                SELECT DISTINCT
                    ?time
                    ?type
                    ?file
                    ?entity as ?layer
                  	COALESCE(?x, 0) AS ?x
	                COALESCE(?y, 0) AS ?y
	                COALESCE(?w, 0) AS ?w
	                COALESCE(?h, 0) AS ?h  
                WHERE
                {
                    BIND( @entity as ?entity) .
                    ?entity prov:qualifiedInfluence ?gen . 
                    ?gen prov:atTime ?time . 
                    BIND( STRBEFORE( STR(?entity), '#' ) as ?strippedEntity ).
                    BIND( if(?strippedEntity != '', ?strippedEntity, str(?entity)) as ?entityStub).

                    ?entity art:renderedAs [
                        rdf:type ?type ;
                        rdfs:label ?f ;
                        art:region [
                            art:x ?x ;
                            art:y ?y ;
                            art:width ?w ;
                            art:height ?h
                        ]
                    ] .
                    BIND( CONCAT(@baseUri, ?entityStub, '&file=', STR(?f) ) as ?file ).
                    BIND(?entity AS ?layer)
                }
                ORDER BY DESC(?time)";

            ISparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@entity", entityUri);
            query.Bind("@baseUri", baseUri);

            List<BindingSet> bindings = ModelProvider.GetActivities().GetBindings(query, true).ToList();

            return Response.AsJsonSync(bindings);
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
                        prov:generated | prov:used ?e.
                    ?e nie:isStoredAs @fileUri.

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

            query.Bind("@fileUri", uriRef);

            IList<BindingSet> bindings = ModelProvider.GetActivities().GetBindings(query).ToList();

            return Response.AsJsonSync(bindings);
        }

        #endregion

        #region Classes

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

        #endregion
    }
}
