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
    public class CommunicationArchiveWriter : ArchiveWriterBase
    {
        #region Constructors

        public CommunicationArchiveWriter(IPlatformProvider platformProvider, IModelProvider modelProvider)
            : base(platformProvider, modelProvider)
        {
        }

        #endregion

        #region Methods
        public string GetProjectId(Uri entityUri, IModel model=null)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?project
                WHERE
                {
                  ?project prov:qualifiedUsage / prov:entity ?file .
                  ?entity nie:isStoredAs ?file .
                  @entityUri prov:hadPrimarySource* ?entity .
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
            return new EntityRenderingInfo[] { };
        }

        protected override ISparqlQuery GetAgentsQuery(Uri entityUri, DateTime minTime)
        {
            ISparqlQuery query = new SparqlQuery(@"
                DESCRIBE
                    ?agent
                    ?association
                WHERE
                {
                  ?activity prov:qualifiedAssociation ?association .
                  ?activity prov:generated @entityUri .
                  ?association prov:agent ?agent .
                }");

            query.Bind("@entityUri", entityUri);

            return query;
        }

        protected override ISparqlQuery GetActivitiesQuery(Uri entityUri, DateTime minTime)
        {
            ISparqlQuery query = new SparqlQuery(@"
                DESCRIBE
                    ?entity
                    ?activity
                    ?entity
                    ?bounds

                WHERE
                {
                  BIND( @entity as ?entity ).
                  ?activity prov:generated ?entity .
                  ?activity prov:startedAtTime ?startTime .

                  FILTER(@minTime <= ?startTime) .
                  OPTIONAL { ?entity art:region ?bounds. }


                }");

            query.Bind("@entity", entityUri);
            query.Bind("@minTime", minTime);

            return query;
        }

        #endregion
    }
}
