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
        }

        #endregion

        #region Methods

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
                    
                    FILTER NOT EXISTS { ?var prov:invalidated ?uri }
                   
                }
                ORDER BY DESC(?time) LIMIT 1");

            query.Bind("@fileName", file);
            query.Bind("@folderUrl", new Uri(folder));

            var bindings = ModelProvider.GetActivities().GetBindings(query).FirstOrDefault();

            PlatformProvider.Logger.LogRequest(HttpStatusCode.OK, Request);

            return Response.AsJsonSync(bindings);
        }

        #endregion
    }
}
