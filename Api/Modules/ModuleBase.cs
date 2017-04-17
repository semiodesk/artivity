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

using Artivity.Api.IO;
using Artivity.Api.Platform;
using Artivity.DataModel;
using System;
using System.IO;
using Semiodesk.Trinity;
using Nancy;
using Nancy.IO;
using Newtonsoft.Json;
using Nancy.Security;
using System.Collections.Generic;
using System.Linq;

namespace Artivity.Api.Modules
{
	public abstract class ModuleBase : NancyModule
    {
        #region Members

        public IModelProvider ModelProvider { get; set; }

        public IPlatformProvider PlatformProvider { get; set; }

        protected string ArtivityModulePath { get; set; }

        #endregion

        #region Constructors

        public ModuleBase(IModelProvider modelProvider, IPlatformProvider platformProvider) 
        {
            ModelProvider = modelProvider;
            PlatformProvider = platformProvider;

            After.AddItemToEndOfPipeline((ctx) =>
            {
                ModelProvider.ReleaseStore();
            });

            OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                ModelProvider.ReleaseStore();

                return null;
            });
        }

        public ModuleBase(string modulePath, IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base(modulePath)
        {
            ModelProvider = modelProvider;
            PlatformProvider = platformProvider;
            ArtivityModulePath = modulePath;

            After.AddItemToEndOfPipeline((ctx) =>
            {
                ModelProvider.ReleaseStore();
            });

            OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                ModelProvider.ReleaseStore();

                return null;
            });
        }

		#endregion

        #region Methods

        public Response InitializeRequest()
        {
            if (PlatformProvider.RequiresAuthentication)
            {
                this.RequiresAuthentication();
            }

            return null;
        }

        public T Bind<T>(IStore store, RequestStream stream) where T : Resource
        {
            using (var reader = new StreamReader(stream))
            {
                string value = reader.ReadToEnd();

                CustomJsonResourceSerializerSettings settings = new CustomJsonResourceSerializerSettings(store);

                return JsonConvert.DeserializeObject<T>(value, settings);
            }
        }

        protected bool IsUri(string uri, UriKind kind = UriKind.Absolute)
        {
            return !string.IsNullOrEmpty(uri) && Uri.IsWellFormedUriString(uri, kind);
        }

        protected bool IsFileUrl(string url)
        {
            return (!string.IsNullOrEmpty(url)) && IsUri(Uri.EscapeUriString(url));
        }


        /// <summary>
        /// Processes the regular get function for this endpoint.
        /// Takes an 'uri' parameter if you want a specific entity.
        /// Returns a list of entities if no parameter was specified.
        /// </summary>
        /// <returns></returns>
        protected virtual Response GetEntities<T>(IModel model, Uri entityTypeUri) where T : Resource
        {
            int offset = -1;
            int limit = -1;

            GetOffsetLimit(out offset, out limit);

            if (Request.Query["uri"] != null && !string.IsNullOrEmpty(Request.Query["uri"]))
            {
                Uri uri = new Uri(Request.Query["uri"]);

                T entity = model.GetResource<T>(uri);

                return Response.AsJsonSync(entity);
            }
            else
            {
                int count = 0;

                SparqlQuery countQuery = new SparqlQuery(@"
                    SELECT COUNT(DISTINCT ?s) AS ?count WHERE
                    {
	                    ?s a @type .
	
	                    OPTIONAL { ?s art:deleted ?deletionTime . }
	
	                    FILTER(!BOUND(?deletionTime) || ?deletionTime = @minDate)
                    }");

                countQuery.Bind("@type", entityTypeUri);
                countQuery.Bind("@minDate", DateTime.MinValue);

                BindingSet b = model.ExecuteQuery(countQuery).GetBindings().FirstOrDefault();

                if (b != null)
                {
                    count = (int)b["count"];

                    if (count > 0)
                    {
                        SparqlQuery query = new SparqlQuery(@"
                        SELECT ?s ?p ?o WHERE
                        {
                            ?s ?p ?o
                            {
                                SELECT ?s WHERE
                                {
	                                ?s a @type .
	
	                                OPTIONAL { ?s art:deleted ?deletionTime . }
	
	                                FILTER(!BOUND(?deletionTime) || ?deletionTime = @minDate)
                                }
                                OFFSET @offset LIMIT @limit
                            }
                        }");

                        query.Bind("@type", entityTypeUri);
                        query.Bind("@minDate", DateTime.MinValue);
                        query.Bind("@offset", offset);
                        query.Bind("@limit", limit);

                        List<T> data = model.GetResources<T>(query).ToList();

                        return Response.AsJsonSync(new Dictionary<string, object>
                        {
                            {"success", true},
                            {"count", count},
                            {"offset", offset},
                            {"limit", limit},
                            {"data", data}
                        });
                    }
                }

                return Response.AsJsonSync(new Dictionary<string, object>
                {
                    {"success", false},
                    {"count", count},
                    {"offset", offset},
                    {"limit", limit},
                    {"data", new List<T>()}
                });
            }
        }

        protected void GetOffsetLimit(out int offset, out int limit)
        {
            offset = Request.Query["offset"] ? (int)Request.Query["offset"] : 0;
            limit = Request.Query["limit"] ? (int)Request.Query["limit"] : 10;
        }

        #endregion
	}
}
