using Artivity.Api.Platform;
using Artivity.DataModel;
using Nancy;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Modules
{
    public class SparqlModule : ModuleBase
    {
        #region Members
        private static object _modelLock = new object();
        #endregion

        #region Constructor
        public SparqlModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0", modelProvider, platformProvider)
        {
            Post["/query"] = parameters =>
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    string query = reader.ReadToEnd();

                    if (string.IsNullOrEmpty(query))
                    {
                        return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                    }

                    if (Request.Query.inference)
                    {
                        return ExecuteQuery(query, true);
                    }
                    else
                    {
                        return ExecuteQuery(query);
                    }
                }
            };

            Post["/sparql"] = parameters =>
            {
                string query = Request.Form.query;

                if (string.IsNullOrEmpty(query))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                if (Request.Query.inference)
                {
                    return ExecuteQuery(query, true);
                }
                else
                {
                    return ExecuteQuery(query);
                }
            };
        }
        #endregion

        #region Members
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

        #endregion
    }
}
