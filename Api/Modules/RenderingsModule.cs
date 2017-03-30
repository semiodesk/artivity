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

using Artivity.Api.Parameters;
using Artivity.Api.Platform;
using Artivity.DataModel;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using Semiodesk.Trinity.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;

namespace Artivity.Api.Modules
{
    public class RenderingsModule : ModuleBase
    {
        #region Constructors

        public RenderingsModule(IModelProvider modelProvider, IPlatformProvider platformProvider, IUserProvider userProvider)
            : base("/artivity/api/1.0/renderings", modelProvider, platformProvider, userProvider)
        {
            Get["/"] = parameters =>
            {
                if (Request.Query.uri)
                {
                    string uri = Request.Query.uri;
                    string file = Request.Query.file;

                    if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                    {
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }

                    if (!string.IsNullOrEmpty(file))
                    {
                        return GetRendering(new UriRef(uri), file);
                    }
                    else if (Request.Query.timestamp)
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

            Get["/canvases"] = parameters =>
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

            Get["/path"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return HttpStatusCode.BadRequest;
                }

                bool create = Request.Query.create != null;

                return GetRenderOutputPath(new UriRef(uri), create);
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
        }

        #endregion

        #region Methods

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

        private Response GetCanvasRenderingsFromEntity(UriRef entityUri)
        {

            string queryString = @"
                SELECT DISTINCT
                    ?time
                    MIN(?type) as ?type
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
                    " + ModelProvider.RenderingQueryModifier + @"
                    BIND(?entity AS ?layer)
                }
                ORDER BY DESC(?time)";

            //queryString = String.Format(queryString, );
            ISparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@entity", entityUri);

            var bindings = ModelProvider.GetActivities().GetBindings(query, true);

            return Response.AsJsonSync(bindings);
        }

        #endregion
    }
}
