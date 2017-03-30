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
using Newtonsoft.Json;
using Semiodesk.Trinity;
using Semiodesk.Trinity.Store;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using VDS.RDF;

namespace Artivity.Api.Modules
{
    public class InfluencesModule : ModuleBase
    {
        #region Constructors

        public InfluencesModule(IModelProvider modelProvider, IPlatformProvider platformProvider, IUserProvider userProvider)
            : base("/artivity/api/1.0/influences", modelProvider, platformProvider, userProvider)
        {
            Get["/"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetInfluencesFromFile(new UriRef(uri));
            };

            Get["/canvas"] = parameters =>
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
                        return GetInfluencedCanvasesFromFile(new UriRef(uri), timestamp.UtcDateTime);
                    }
                    else
                    {
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }
                }
                else
                {
                    return GetInfluencedCanvasesFromFile(new UriRef(uri));
                }
            };

            Get["/layers"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetInfluencedLayersFromFile(new UriRef(uri));
            };

            Get["/stats"] = parameters =>
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
                        return GetCompositionStatsFromFile(new UriRef(uri), timestamp.UtcDateTime);
                    }
                    else
                    {
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }
                }

                return GetCompositionStatsFromFile(new UriRef(uri));
            };
        }

        #endregion

        #region Methods

        private Response GetInfluencesFromFile(UriRef fileUri)
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
                    COALESCE(?agentColor, '#FF0000') AS ?agentColor
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
                if (lastInfluence == null || lastInfluence.uri != uri)
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

        private Response GetInfluencedCanvasesFromFile(UriRef fileUri)
        {
            return GetInfluencedCanvasesFromFile(fileUri, DateTime.UtcNow);
        }

        private Response GetInfluencedCanvasesFromFile(UriRef fileUri, DateTime maxTime)
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

        private Response GetInfluencedLayersFromFile(UriRef fileUri)
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

            query.Bind("@fileUri", fileUri);

            IList<BindingSet> bindings = ModelProvider.GetActivities().GetBindings(query).ToList();

            return Response.AsJsonSync(bindings);
        }

        private Response GetCompositionStatsFromFile(UriRef fileUri)
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

        private Response GetCompositionStatsFromFile(UriRef fileUri, DateTime time)
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

        #endregion

        #region Types

        private class Influence
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

        private struct Change
        {
            public string entity;

            public string entityType;

            public string property;
        }

        #endregion
    }
}
