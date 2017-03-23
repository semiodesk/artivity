// LICENSE:
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
// Copyright (c) Semiodesk GmbH 2017

using Artivity.Api.Parameters;
using Artivity.Api.Platform;
using Artivity.DataModel;
using Artivity.DataModel.Tasks;
using Nancy;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nancy.ModelBinding;

namespace Artivity.Api.Modules
{
    public class FilesModule : ModuleBase
    {
        #region Constructors

        public FilesModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/files", modelProvider, platformProvider)
        {
            Get["/"] = parameters =>
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
                        return GetFileFromUri(new UriRef(uri));
                    }
                    else if (IsFileUrl(url) && !string.IsNullOrEmpty(create))
                    {
                        return CreateFile(new UriRef(uri), new Uri(url));
                    }
                }

                return platformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
            };

            Get["/derivations"] = parameters =>
            {
                return HttpStatusCode.NotImplemented;
            };

            Get["/derivations/latest"] = parameters =>
            {
                GetFilesSettings settings = new GetFilesSettings() { OrderBy = OrderBy.Time, Offset = 0, Limit = 100 };

                return GetLatestDerivedEntityFromFile(settings);
            };

            Put["/derivations/latest/publish"] = parameters =>
            {
                string fileUri = Request.Query.uri;

                if (string.IsNullOrEmpty(fileUri) || !IsUri(fileUri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return PutLatestDerivedEntityFromFile(new UriRef(fileUri));
            };
        }

        #endregion

        #region Methods

        private Response GetLatestDerivedEntityFromFile(GetFilesSettings settings)
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
                    {4}
                    BIND(if(bound(?ended), ?ended, ?started) as ?t).
                    OPTIONAL{{
                        ?agent art:hasColourCode ?agentColor .
                    }}
                    FILTER NOT EXISTS {{
                      ?entity prov:qualifiedRevision / prov:entity ?x .
                    }}
                    {0}
                    }}GROUP BY ?label ?file ?q ?time {1} {2} {3}";
            queryString = string.Format(queryString, FilterClause, OrderClause, LimitClause, OffsetClause, PlatformProvider.GetFilesQueryModifier);
            ISparqlQuery query = new SparqlQuery(queryString);

            var bindings = ModelProvider.GetAll().GetBindings(query);

            return Response.AsJsonSync(bindings);
        }

        /// <summary>
        /// This function publishes the most recent version of a file.
        /// </summary>
        /// <param name="fileUri"></param>
        /// <returns></returns>
        private Response PutLatestDerivedEntityFromFile(Uri fileUri)
        {
            IModel Model = ModelProvider.GetActivities();

            SparqlQuery q = new SparqlQuery(@"
                DESCRIBE ?entity 
                WHERE 
                {
                    ?entity nie:isStoredAs @fileUri .

                    FILTER NOT EXISTS 
                    {
                        ?v prov:qualifiedRevision / prov:entity ?entity
                    }
                }");

            q.Bind("@fileUri", fileUri);

            var entity = Model.ExecuteQuery(q).GetResources<Entity>().First();

            if (entity != null)
            {
                entity.Publish = true;
                entity.Commit();

                // Now we make sure the file has the synchronization state attached.
                var file = Model.GetResource<FileDataObject>(fileUri);

                if (file.SynchronizationState == null)
                {
                    file.Commit();
                }

                return Response.AsJsonSync(true);
            }
            else
            {
                return Response.AsJsonSync(false);
            }
        }

        private Response GetFileFromUri(Uri fileUri)
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

        #endregion

        #region Types

        private enum Order { Ascending, Descending }

        private enum OrderBy { None, Name, Time, Size }

        private enum FilterBy { None, SoftwareAgent, UserAgent, Project }

        private class GetFilesSettings
        {
            #region Members

            public Order Order = Order.Descending;

            public OrderBy OrderBy;

            public FilterBy FilterBy;

            public string FilterValue;

            public int Offset = 0;

            public int Limit = 100;

            #endregion

            #region Methods

            public string GetOrderClause()
            {
                string order = "";

                if (Order == FilesModule.Order.Ascending)
                {
                    order = "ASC";
                }
                else
                {
                    order = "DESC";
                }

                switch (OrderBy)
                {
                    case FilesModule.OrderBy.None:
                        return "";

                    case FilesModule.OrderBy.Name:
                        return string.Format("ORDER BY {0}(?label) ", order);

                    case FilesModule.OrderBy.Time:
                        return string.Format("ORDER BY {0}(?time) ", order);

                    case FilesModule.OrderBy.Size:
                        return "";

                    default:
                        return "";
                }
            }

            public string GetFilterClause()
            {
                if (FilterBy == FilesModule.FilterBy.None)
                {
                    return "";
                }

                Uri uri = new Uri(FilterValue);

                switch (FilterBy)
                {
                    case FilesModule.FilterBy.SoftwareAgent:
                        return string.Format("FILTER(?agent = <{0}>)", uri.AbsoluteUri);

                    case FilesModule.FilterBy.UserAgent:
                        return "";

                    case FilesModule.FilterBy.Project:
                        return string.Format("<{0}> prov:hadMember ?entity .", uri.AbsoluteUri);

                    default:
                        return "";
                }
            }

            public string GetLimitClause() { return "LIMIT " + Limit; }

            public string GetOffsetClause() { return "OFFSET " + Offset; }

            #endregion
        }

        #endregion
    }
}