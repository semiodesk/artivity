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

using Artivity.Api;
using Artivity.Api.Platform;
using Artivity.Api.Modules;
using Artivity.DataModel;
using Nancy;
using Nancy.Extensions;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Storage;

namespace Artivity.Apid.Modules
{
    public class SparqlModule : ModuleBase
    {
        #region Members

        private static object _modelLock = new object();

        #endregion

        #region Constructor

        public SparqlModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/sparql", modelProvider, platformProvider)
        {
            Post["/"] = parameters =>
            {
                string query = Request.Form.query;

                if (string.IsNullOrEmpty(query) && Request.Body.Length == 0)
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                if(!string.IsNullOrEmpty(query))
                {
                    if (Request.Query.inference)
                    {
                        return ExecuteQuery(query, true);
                    }
                    else
                    {
                        return ExecuteQuery(query);
                    }
                }
                else
                {
                    // Remove the 'update=' at the start of the string.
                    string update = Context.Request.Body.AsString().Remove(0, 7);
                    update = Uri.UnescapeDataString(update);
                    update = update.Replace('+', ' ');

                    return ExecuteUpdate(update);
                }
            };

            #if DEBUG
            Get["/dump"] = parameters =>
            {
                var m = ModelProvider.Activities;
                VirtuosoManager man = new VirtuosoManager(ModelProvider.NativeConnectionString);
                var g = man.Query(string.Format("DESCRIBE ?s FROM <{0}> WHERE {{ ?s ?p ?o . }}", m.AbsoluteUri));
                IGraph graph = g as IGraph;

                if (graph != null && !graph.IsEmpty)
                {
                    var syntax = VDS.RDF.Parsing.TurtleSyntax.W3C;

                    var writer = new VDS.RDF.Writing.CompressingTurtleWriter(syntax);
                    writer.DefaultNamespaces.AddNamespace("art", ART.Namespace);
                    writer.DefaultNamespaces.AddNamespace("dc", DCES.Namespace);
                    writer.DefaultNamespaces.AddNamespace("nie", NIE.Namespace);
                    writer.DefaultNamespaces.AddNamespace("nfo", NFO.Namespace);
                    writer.DefaultNamespaces.AddNamespace("prov", PROV.Namespace);


                    graph.SaveToFile("activities.ttl", writer);

                }
                return Response.AsText("Hello");
            };
            #endif
        }

        #endregion

        #region Methods

        private Response ExecuteQuery(string queryString, bool inferenceEnabled = false)
        {
            try
            {
                lock (_modelLock)
                {
                    IModel model = ModelProvider.GetAll();

                    if (model == null)
                    {
                        PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, "Could not establish connection to model <{0}>", model.Uri);
                    }

                    SparqlQuery query = new SparqlQuery(queryString, false);

                    var results = model.ExecuteQuery(query, inferenceEnabled).GetBindings();

                    if (results != null && results.Any())
                    {
                        var vars = results.First().Keys.ToList();
                        var bindings = new List<Dictionary<string, object>>();

                        foreach (BindingSet row in results)
                        {
                            var item = new Dictionary<string, object>();

                            foreach (KeyValuePair<string, object> column in row)
                            {
                                string type = column.Value is Uri ? "uri" : "literal";
                                string value = column.Value.ToString();

                                var b = new Dictionary<string, string>() { { "type", type }, { "value", value } };

                                if (type == "literal" && !(column.Value is string))
                                {
                                    Type valueType = column.Value.GetType();

                                    if (!valueType.IsAssignableFrom(typeof(DBNull)))
                                    {
                                        b["datatype"] = XsdTypeMapper.GetXsdTypeUri(valueType).ToString();
                                    }
                                }

                                item[column.Key] = b;
                            }

                            bindings.Add(item);
                        }

                        Dictionary<string, object> result = new Dictionary<string, object>();
                        result["head"] = new Dictionary<string, List<string>>() { { "vars", vars } };
                        result["results"] = new Dictionary<string, List<Dictionary<string, object>>> { { "bindings", bindings } };

                        Response response = Response.AsJsonSync(result);
                        response.ContentType = "application/sparql-results+json";

                        return response;
                    }
                    else
                    {
                        Dictionary<string, object> result = new Dictionary<string, object>();
                        result["head"] = new Dictionary<string, List<string>>() { };
                        result["results"] = new Dictionary<string, List<Dictionary<string, object>>> { { "bindings", new List<Dictionary<string, object>>() } };

                        Response response = Response.AsJsonSync(result);
                        response.ContentType = "application/sparql-results+json";

                        return response;
                    }
                }
            }
            catch (Exception e)
            {
                PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e);

                List<string> messages = new List<string>() { e.Message };

                if (e.InnerException != null)
                {
                    messages.Add(e.InnerException.Message);
                }

                return Response.AsJsonSync(messages);
            }
        }

        private Response ExecuteUpdate(string updateString)
        {
            SparqlUpdate update = new SparqlUpdate(updateString);

            IModel model = ModelProvider.GetActivities();
            model.ExecuteUpdate(update);

            return HttpStatusCode.OK;
        }

        #endregion
    }
}
