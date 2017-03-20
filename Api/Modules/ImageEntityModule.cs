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
using System.Linq;
using System.Collections.Generic;
using Nancy.Security;

namespace Artivity.Api.Modules
{
    public class ImageEntityModule : EntityModuleBase<Image>
    {
        public ImageEntityModule(IModelProvider modelProvider, IPlatformProvider platformProvider) : 
            base("/artivity/api/1.0/entity/images", modelProvider, platformProvider)
        {
            Get["/recent"] = parameters =>
            {
                if (!string.IsNullOrEmpty(Request.Query["uri"]))
                {
                    string fileUriString = Request.Query["uri"];
                    Uri uri = new Uri(fileUriString);
                    return GetRecentByFile(uri);
                }else
                    return HttpStatusCode.InternalServerError;
            };
        }

        protected override Response GetEntity()
        {
            InitQuery();

            if (Request.Query["fileUri"] != null) 
            {
                Uri fileUri = new Uri(Request.Query["fileUri"]);
                return GetByFile(fileUri);
            }


            return base.GetEntity();
           
        }

        protected Response GetRecentByFile(Uri fileUri)
        {
            InitQuery();

            string queryString = @"
            DESCRIBE ?entity WHERE
            {
                ?entity a nfo:Image ;
                  nie:isStoredAs @fileUri .
                FILTER NOT EXISTS {{
                      ?v prov:qualifiedRevision / prov:entity ?entity .
                    }}
            }";
            SparqlQuery query = new SparqlQuery(queryString);
            query.Bind("@fileUri", fileUri);

            var entity = UserModel.ExecuteQuery(query, true).GetResources<Image>().First();

            return Response.AsJsonSync(entity);

        }

        protected Response GetByFile(Uri fileUri)
        {
            ResourceQuery entity = new ResourceQuery(nfo.Image);
            //ResourceQuery file = new ResourceQuery(fileUri);
            entity.Where(nie.isStoredAs, fileUri);

            if (Request.Query["sort"] != null)
            {
                ResourceQuery influence = new ResourceQuery();
                entity.Where(prov.qualifiedInfluence, influence);
                if( Request.Query["sort"] == "asc")
                    influence.Where(prov.atTime).SortAscending();
                if( Request.Query["sort"] == "desc")
                    influence.Where(prov.atTime).SortDescending();
            }

            var queryResult = UserModel.ExecuteQuery(entity, true);
            
            int count = queryResult.Count();

            int offset = -1;
            int limit = -1;
            GetOffsetLimit(out offset, out limit);

            List<Image> list = queryResult.GetResources<Image>(offset, limit).ToList();

            return Response.AsJsonSync(new Dictionary<string, object> { { "success", true },{"count", count},{"offset", offset}, {"limit", limit},{ "data", list },  });

        }
    }
}
