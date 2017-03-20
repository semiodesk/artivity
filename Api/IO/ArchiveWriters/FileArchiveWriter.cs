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
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.IO
{
    public class FileArchiveWriter : ArchiveWriterBase
    {
        #region Constructors

        public FileArchiveWriter(IPlatformProvider platformProvider, IModelProvider modelProvider)
            : base(platformProvider, modelProvider)
        {
        }

        #endregion

        #region Methods

        protected override IEnumerable<EntityRenderingInfo> GetRenderings(Uri fileUri, DateTime minTime)
        {
            IModel model = ModelProvider.GetActivities();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?entity ?file WHERE
                {
                    ?activity prov:generated | prov:used ?entity .
                    ?entity nie:isStoredAs @fileUri .
                    ?fileEntity art:publish ""true""^^xsd:boolean_ . 
                    ?entity art:renderdAs / rdfs:label ?file .
                }");

            query.Bind("@fileUri", fileUri);

            IEnumerable<BindingSet> bindings = model.GetBindings(query);

            foreach (BindingSet b in bindings)
            {
                string entity = b["entity"].ToString();
                string file = b["file"].ToString();

                yield return new EntityRenderingInfo(entity, file);
            }
        }

        protected override ISparqlQuery GetAgentsQuery(Uri fileUri, DateTime minTime)
        {
            ISparqlQuery query = new SparqlQuery(@"
                DESCRIBE
                    ?agent
                    ?association
                WHERE
                {
                  ?activity prov:generated | prov:used ?entity .
                  ?entity nie:isStoredAs @fileUri .
                  ?entity art:publish ""true""^^xsd:boolean_ . 
                  ?activity prov:qualifiedAssociation ?association .
                  ?association prov:agent ?agent .
                }");

            query.Bind("@fileUri", fileUri);

            return query;
        }

        protected override ISparqlQuery GetActivitiesQuery(Uri uri, DateTime minTime)
        {


            ISparqlQuery query = new SparqlQuery(@"
                DESCRIBE
                    ?activity
                    ?file
                    ?influence
                    ?entity
                    ?fileEntity
                    ?bounds
                    ?change
                    ?render
                    ?renderRegion
                WHERE
                {
                  BIND( @file as ?file ).
                  ?fileEntity nie:isStoredAs @file .
                  ?fileEntity art:publish ""true""^^xsd:boolean_ . 

                  ?activity prov:generated | prov:used ?entity .
                  ?activity prov:generated | prov:used ?fileEntity .
                  ?activity prov:startedAtTime ?startTime .

                  ?influence prov:activity | prov:hadActivity ?activity .
                  ?influence prov:entity ?entity .
                  ?entity rdf:type ?entityType .
                  FILTER ( ?entityType = nfo:VectorImage || ?entityType = nfo:RasterImage || ?entityType = art:Canvas )

                  OPTIONAL
                  {
                     ?fileEntity art:renderedAs ?render .

                     OPTIONAL { ?render art:region ?renderRegion . }
                     OPTIONAL { ?render art:renderedLayer ?layer . }
                  }

                  OPTIONAL { ?influence art:hadViewport ?viewport . }
                  OPTIONAL { ?influence art:hadBoundaries ?bounds . }
                  OPTIONAL
                  {
                     ?influence art:hadChange ?change .

                     OPTIONAL { ?change art:entity ?entity . }
                  }
                }");


            query.Bind("@file", uri);

            return query;
        }

        #endregion
    }
}
