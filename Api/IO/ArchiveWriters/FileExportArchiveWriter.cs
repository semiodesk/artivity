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
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Artivity.Api.IO
{
    public class FileExportArchiveWriter : ArchiveWriterBase
    {
        #region Constructors

        public FileExportArchiveWriter(IPlatformProvider platformProvider, IModelProvider modelProvider)
            : base(platformProvider, modelProvider)
        {
        }

        #endregion

        #region Methods

        public string GetProjectId(Uri entityUri, IModel model = null)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?project
                WHERE
                {
                  ?project prov:qualifiedUsage / prov:entity ?file .
                  @entityUri nie:isStoredAs ?file .
                }");

            query.Bind("@entityUri", entityUri);

            if (model == null)
                model = DefaultModel;
            IEnumerable<BindingSet> bindings = model.GetBindings(query);
            if (bindings.Any())
            {
                BindingSet binding = bindings.First();
                string uri = binding["project"].ToString();

                if (!string.IsNullOrEmpty(uri))
                {
                    string p = new Uri(uri).AbsolutePath;
                    return Path.GetFileName(p);
                }
            }
            return null;
        }

        protected override IEnumerable<EntityRenderingInfo> GetRenderings(Uri uri, DateTime minTime)
        {
            IModel model = ModelProvider.GetActivities();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?entityStub as ?entity ?file WHERE
                {
                    ?activity prov:generated | prov:used ?entity .
                    ?entity nie:isStoredAs @fdo .

                    BIND( STRBEFORE( STR(?entity), '#' ) as ?e ).
                    BIND( if(?e != '', ?e, str(?entity)) as ?entityStub).

                    {
                        ?influence
                            prov:activity | prov:hadActivity ?activity ;
                            prov:atTime ?time ;
                            art:renderedAs / rdfs:label ?file .
                    }UNION
                    {
                        ?entity art:renderedAs / rdfs:label ?file .
                        ?entity prov:qualifiedInfluence / prov:atTime ?time .
                    }
                    FILTER(@minTime <= ?time) .

                }");

            query.Bind("@fdo", uri);
            query.Bind("@minTime", minTime);

            IEnumerable<BindingSet> bindings = model.GetBindings(query, true);

            foreach (BindingSet b in bindings)
            {
                string entity = b["entity"].ToString();
                string file = b["file"].ToString();

                yield return new EntityRenderingInfo(entity, file);
            }
        }

        protected override ISparqlQuery GetAgentsQuery(Uri uri, DateTime minTime)
        {
            ISparqlQuery query = new SparqlQuery(@"
                DESCRIBE
                    ?agent
                    ?association
                WHERE
                {
                  ?activity prov:generated | prov:used ?entity .
                  ?entity nie:isStoredAs @file .
                  ?activity prov:qualifiedAssociation ?association .

                  ?association prov:agent ?agent .
                }");

            query.Bind("@file", uri);

            return query;
        }

        protected override ISparqlQuery GetActivitiesQuery(Uri uri, DateTime minTime)
        {
            ISparqlQuery query = new SparqlQuery(@"
                DESCRIBE
                    ?entity
                    ?file
                    ?activity
                    ?influence
                    ?fileEntity
                    ?undo
                    ?redo
                    ?bounds
                    ?change
                    ?render
                    ?renderRegion
                WHERE
                {
                  ?activity prov:generated | prov:used ?entity .
                  ?activity prov:generated | prov:used ?fileEntity .
                  ?activity prov:startedAtTime ?startTime .

                  FILTER(@minTime <= ?startTime) .

                  ?fileEntity nie:isStoredAs @file .

                  BIND( @file as ?file ) .

                  ?influence prov:activity | prov:hadActivity ?activity .

                  OPTIONAL
                  {
                     ?entity art:renderedAs ?render .

                     OPTIONAL { ?render art:region ?renderRegion . }
                  }

                  OPTIONAL
                  {
                     ?influence art:renderedAs ?render .

                     OPTIONAL { ?render art:region ?renderRegion . }
                  }

                  OPTIONAL { ?influence art:hadViewport ?viewport . }
                  OPTIONAL { ?influence art:hadBoundaries ?bounds . }
                  OPTIONAL
                  {
                     ?influence art:hadChange ?change .

                     OPTIONAL { ?change art:entity ?entity . }
                  }

                  OPTIONAL { ?undo art:reverted ?influence . }
                  OPTIONAL { ?redo art:restored ?influence . }
                }");

            query.Bind("@file", uri);
            query.Bind("@minTime", minTime);

            return query;
        }

        #endregion
    }
}
