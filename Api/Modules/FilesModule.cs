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

using Artivity.Api.Platform;
using Artivity.DataModel;
using Nancy;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
                InitializeRequest();

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

            Get["/recent"] = parameters =>
            {
                InitializeRequest();

                GetFilesSettings settings = new GetFilesSettings() { OrderBy = OrderBy.Time, Offset = 0, Limit = 100 };

                return GetRecentFiles(settings);
            };

            Get["/revisions"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.fileUri))
                {
                    UriRef fileUri = new UriRef(Request.Query.fileUri);

                    return GetRevisionsFromFile(fileUri);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Get["/revisions/latest"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.fileUri))
                {
                    UriRef fileUri = new UriRef(Request.Query.fileUri);

                    return GetLatestRevisionFromFile(fileUri);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Put["/revisions/latest/publish"] = parameters =>
            {
                InitializeRequest();

                string fileUri = Request.Query.fileUri;

                if (string.IsNullOrEmpty(fileUri) || !IsUri(fileUri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return PublishLatestRevision(new UriRef(fileUri));
            };

            Get["/edit"] = parameters =>
            {
                InitializeRequest();

                string fileUrl = Request.Query.fileUrl;

                if (string.IsNullOrEmpty(fileUrl) || !IsFileUrl(fileUrl))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return EditFile(new UriRef(fileUrl));
            };

            Put["/tags/important"] = parameters =>
            {
                InitializeRequest();

                string fileUri = Request.Query.fileUri;
                bool important = Request.Query.value;

                if (string.IsNullOrEmpty(fileUri) || !IsUri(fileUri))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return SetImportant(new UriRef(fileUri), important);
            };
        }

        #endregion

        #region Methods

        private Response GetRecentFiles(GetFilesSettings settings)
        {
            IModel model = ModelProvider.GetAll();

            if (model == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            string OrderClause = settings.GetOrderClause();
            string FilterClause = settings.GetFilterClause();
            string LimitClause = settings.GetLimitClause();
            string OffsetClause = settings.GetOffsetClause();

            string queryString = @"
                SELECT DISTINCT
                    MAX(?t) AS ?time 
                    ?entityUri
                    ?file AS ?uri
                    ?label
                    ?folder
                    SAMPLE(?p) AS ?thumbnail 
                    COALESCE(?agentColor, '#cecece') AS ?agentColor
                    COALESCE(?synchronizationEnabled, 'false') AS ?synchronizationEnabled
                WHERE
                {{
                    ?activity prov:generated | prov:used ?entity ;
                        prov:startedAtTime ?startTime ;
                        prov:endedAtTime ?endTime .
  
                    ?entity nie:isStoredAs ?file.
                    ?file rdfs:label ?label .

                    OPTIONAL {{ ?file nfo:belongsToContainer / nie:url ?folder . }}

                    BIND(STRBEFORE(STR(?entity), '#') AS ?e).
                    BIND(IF(?e != '', ?e, STR(?entity)) AS ?entityUri).
                    {4}
                    BIND(IF(BOUND(?endTime), ?endTime, ?startTime) AS ?t).

                    OPTIONAL {{
                        ?activity prov:qualifiedAssociation / prov:agent ?agent .

                        ?agent a prov:SoftwareAgent.
                        ?agent art:hasColourCode ?agentColor .
                    }}

                    OPTIONAL {{
                        ?entity arts:synchronizationEnabled ?synchronizationEnabled .
                    }}

                    FILTER NOT EXISTS {{
                      ?entity prov:qualifiedRevision / prov:entity ?x .
                    }}

                    FILTER NOT EXISTS
                    {{
                        ?activity a art:Project .
                        ?activity prov:qualifiedUsage / prov:entity ?file .
                    }}

                    {0}
                }}
                GROUP BY ?label ?folder ?file ?entityUri ?agentColor ?synchronizationEnabled ?time {1} {2} {3}";

            queryString = string.Format(queryString, FilterClause, OrderClause, LimitClause, OffsetClause, ModelProvider.GetFilesQueryModifier);

            ISparqlQuery query = new SparqlQuery(queryString);

            var bindings = model.GetBindings(query);

            return Response.AsJsonSync(bindings);
        }

        private Response GetRevisionsFromFile(UriRef fileUri)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?time
                    ?revision
                    COALESCE(?remote, -1) AS ?remote
                WHERE
                {
                    ?revision nie:isStoredAs @file ; nie:created ?time .

                    OPTIONAL
                    {
                        ?revision arts:synchronizationState / arts:lastRemoteRevision ?remote .
                    }
                }
                ORDER BY DESC(?time)
            ");

            query.Bind("@file", fileUri);

            List<BindingSet> bindings = model.GetBindings(query).ToList();

            return Response.AsJsonSync(bindings);
        }

        private Response GetLatestRevisionFromFile(UriRef fileUri)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?time
                    ?revision
                    COALESCE(?remote, -1) AS ?remote
                WHERE
                {
                    ?revision nie:isStoredAs @file .

                    @file nie:created ?time .

                    OPTIONAL
                    {
                        ?revision arts:synchronizationState / arts:lastRemoteRevision ?remote .
                    }

                    FILTER NOT EXISTS
                    {
                        ?older prov:qualifiedRevision / prov:entity ?revision .
                    }
                }
                LIMIT 1
            ");

            query.Bind("@file", fileUri);

            List<BindingSet> bindings = model.GetBindings(query).ToList();

            return Response.AsJsonSync(bindings);
        }

        /// <summary>
        /// This function publishes the most recent version of a file.
        /// </summary>
        /// <param name="fileUri"></param>
        /// <returns></returns>
        private Response PublishLatestRevision(Uri fileUri)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            SparqlQuery q = new SparqlQuery(@"
                DESCRIBE ?entity 
                WHERE 
                {
                    ?entity nie:isStoredAs @fileUri .

                    FILTER NOT EXISTS 
                    {
                        ?entity prov:qualifiedRevision / prov:entity ?newer .
                    }
                }");

            q.Bind("@fileUri", fileUri);

            var entity = model.ExecuteQuery(q).GetResources<Entity>().First();

            if (entity != null)
            {
                entity.IsSynchronizationEnabled = true;
                entity.Commit();

                return Response.AsJsonSync(true);
            }
            else
            {
                return Response.AsJsonSync(false);
            }
        }

        private Response GetFileFromUri(Uri fileUri)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.InternalServerError;
            }

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

            BindingSet bindings = model.GetBindings(query).FirstOrDefault();

            return Response.AsJsonSync(bindings);
        }

        private Response CreateFile(UriRef uri, Uri url)
        {
            PlatformProvider.AddFile(uri, url);

            return HttpStatusCode.OK;
        }

        private Response EditFile(UriRef fileUrl)
        {
            FileInfo file = new FileInfo(fileUrl.LocalPath);

            if(file.Exists)
            {
                string[] supportedExtensions = { ".psd", ".ai", ".svg" };

                if (supportedExtensions.Contains(file.Extension))
                {
                    Process.Start(fileUrl.LocalPath);

                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.MethodNotAllowed;
                }
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }

        private Response SetImportant(UriRef fileUri, bool imporant)
        {
            IModel model = ModelProvider.GetActivities();

            if (model == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            Entity entity = model.GetResource<Entity>(fileUri);

            if (entity != null)
            {
                if(imporant)
                {
                    entity.AddTag(art.important.Uri);
                }
                else
                {
                    entity.RemoveTag(art.important.Uri);
                }

                entity.Commit();

                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
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