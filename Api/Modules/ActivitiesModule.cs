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
    public class ActivitiesModule : ModuleBase
    {
        #region Constructors

        public ActivitiesModule(IModelProvider modelProvider, IPlatformProvider platformProvider, IUserProvider userProvider)
            : base("/artivity/api/1.0/activities", modelProvider, platformProvider, userProvider)
        {
            Get["/"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri))
                {
                    return GetActivities();
                }
                else if (IsUri(uri))
                {
                    return GetActivitiesFromFileUri(new UriRef(uri));
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            #if DEBUG
            Get["/clear"] = parameters =>
            {
                return ClearActivities();
            };
            #endif
        }

        #endregion

        #region Methods

        private Response GetActivities()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT
                    ?activity AS ?uri
                    ?startTime 
                    ?endTime
                    MAX(?time) as ?maxTime
                    ?agent
                    COALESCE(?agentColor,'#FF0000') AS ?agentColor
                WHERE
                {
                  ?activity
                    prov:startedAtTime ?startTime .

                  OPTIONAL
                  {
                    ?activity prov:endedAtTime ?endTime .
                  }

                  OPTIONAL
                  {
                    ?activity prov:qualifiedAssociation / prov:agent ?agent .

                    ?agent art:hasColourCode ?agentColor .
                  }
                }
                ORDER BY DESC(?startTime)");

            var bindings = ModelProvider.GetAll().GetBindings(query).ToList();

            return Response.AsJsonSync(bindings);
        }

        private Response GetActivitiesFromFileUri(UriRef fileUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT
                    ?activity AS ?uri
                    ?startTime 
                    ?endTime
                    MAX(?time) as ?maxTime
                    ?agent
                    COALESCE(?agentColor,'#FF0000') AS ?agentColor
                WHERE
                {
                  ?activity
                    prov:generated | prov:used ?entity ;
                    prov:startedAtTime ?startTime .
                    ?entity nie:isStoredAs @file.

                  OPTIONAL
                  {
                    ?activity prov:endedAtTime ?endTime .
                  }

                  OPTIONAL
                  {
                    ?activity prov:qualifiedAssociation
                    [
                      prov:hadRole art:SOFTWARE ;
                      prov:agent ?agent
                    ] .

                    OPTIONAL
                    {
                        ?agent art:hasColourCode ?agentColor .
                    }
                  }

                  ?influence
                    prov:activity | prov:hadActivity ?activity ;
                    prov:atTime ?time.
                }
                ORDER BY DESC(?startTime)");

            query.Bind("@file", fileUri);

            var bindings = ModelProvider.GetAll().GetBindings(query).ToList();

            return Response.AsJsonSync(bindings);
        }

        private Response ClearActivities()
        {
            ModelProvider.GetActivities().Clear();

            return HttpStatusCode.OK;
        }

        #endregion
    }
}
