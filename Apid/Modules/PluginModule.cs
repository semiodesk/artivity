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
using Artivity.Api.Modules;
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

namespace Artivity.Apid.Modules
{
    public class PluginModule : ModuleBase
    {
        #region Members

        private static object _modelLock = new object();

        private static readonly Dictionary<string, Browse> _activities = new Dictionary<string, Browse>();

        #endregion

        #region Constructors

        public PluginModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/plugin", modelProvider, platformProvider)
        {
            Post["/file/activities"] = parameters =>
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    string data = reader.ReadToEnd();

                    // We start a new task so that the agent can continue its work as fast as possible.
                    new Task(() => PostFileActivityData(data)).Start();

                    return HttpStatusCode.OK;
                }
            };

            Get["/file/open"] = parameters =>
            {
                string fileUrl = Request.Query["fileUrl"];

                if (!IsFileUrl(fileUrl))
                {
                    return platformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }

                return GetUri(new UriRef(fileUrl));


            };

            Post["/file/new"] = parameters =>
            {
                return HttpStatusCode.NotImplemented;
            };

            Post["/web/activities"] = parameters =>
            {
                ActivityParameter p = this.Bind<ActivityParameter>();

                return PostWebActivity(p);
            };

            Get["/web/status"] = parameters =>
            {
                return GetSoftwareAgentStatus();
            };

            Post["/web/status"] = parameters =>
            {
                var p = this.Bind<AgentParameter>();

                return PostSoftwareAgentStatus(p);
            };
        }

        #endregion

        #region Methods

        private Response PostFileActivityData(string data)
        {
            try
            {
                lock (_modelLock)
                {
                    MemoryStream stream = new MemoryStream();

                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(data);
                    writer.Flush();

                    stream.Position = 0;

                    LoadTurtle(ModelProvider.Activities, stream);
                }

                return PlatformProvider.Logger.LogRequest(HttpStatusCode.OK, Request.Url, "POST", data);
            }
            catch (Exception ex)
            {
                return PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, ex, data);
            }
        }

        private void LoadTurtle(Uri modelUri, Stream stream)
        {
            string connectionString = ModelProvider.NativeConnectionString;

            using (StreamReader reader = new StreamReader(stream))
            {
                string data = reader.ReadToEnd();

                using (VDS.RDF.Storage.VirtuosoManager m = new VDS.RDF.Storage.VirtuosoManager(connectionString))
                {
                    using (VDS.RDF.Graph graph = new VDS.RDF.Graph())
                    {
                        IRdfReader parser = dotNetRDFStore.GetReader(RdfSerializationFormat.N3);
                        parser.Load(graph, new StringReader(data));

                        graph.BaseUri = modelUri;

                        m.UpdateGraph(modelUri, graph.Triples, new List<Triple>());
                    }
                }
            }
        }

        private Response PostWebActivity(ActivityParameter p)
        {
            try
            {
                if (string.IsNullOrEmpty(p.url) && p.endTime == null)
                {
                    // We do not log this access because it happes regularly with Chrome.
                    return HttpStatusCode.BadRequest;
                }

                if (string.IsNullOrEmpty(p.agent))
                {
                    return PlatformProvider.Logger.LogError(HttpStatusCode.BadRequest, "Invalid parameters.", p);
                }

                if (string.IsNullOrEmpty(p.tab))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.NotModified, Request.Url, "POST", p.tab);
                }

                if (!IsCaptureEnabled(p))
                {
                    return PlatformProvider.Logger.LogRequest(HttpStatusCode.Locked, Request.Url, "POST", "");
                }

                IModel model = ModelProvider.GetWebActivities();

                Browse activity;

                if (!_activities.ContainsKey(p.tab))
                {
                    Association association = model.CreateResource<Association>();
                    association.Agent = new SoftwareAgent(new UriRef(p.agent));
                    association.Role = new Role(art.EditingSoftwareRole);
                    association.Commit();

                    activity = model.CreateResource<Browse>();
                    activity.Associations.Add(association);
                    activity.StartTimeUtc = p.startTime != null ? (DateTime)p.startTime : DateTime.Now;
                    activity.Commit();

                    _activities[p.tab] = activity;
                }
                else
                {
                    activity = _activities[p.tab];
                }

                if (!string.IsNullOrEmpty(p.url))
                {
                    UriRef url = new UriRef(p.url);

                    WebDataObject website;

                    if (!model.ContainsResource(url))
                    {
                        website = model.CreateResource<WebDataObject>(url);
                        website.Title = p.title;
                        website.Commit();
                    }
                    else
                    {
                        website = model.GetResource<WebDataObject>(url);
                    }

                    DateTime time = p.time != null ? (DateTime)p.time : DateTime.Now;

                    View view = model.CreateResource<View>();
                    view.Entity = website;
                    view.Time = time;
                    view.Commit();

                    activity.Usages.Add(view);
                    activity.Commit();

                    PlatformProvider.Logger.LogRequest(HttpStatusCode.OK, Request.Url, "POST", "");
                }

                if (p.endTime != null)
                {
                    activity.EndTimeUtc = (DateTime)p.endTime;
                    activity.Commit();

                    _activities.Remove(p.tab);

                    PlatformProvider.Logger.LogRequest(HttpStatusCode.OK, Request.Url, "POST", "");
                }

                return PlatformProvider.Logger.LogError(HttpStatusCode.BadRequest, Request.Url);
            }
            catch (Exception e)
            {
                return PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, "{0}: {1}", Request.Url, e.Message);
            }
        }

        private bool IsCaptureEnabled(ActivityParameter p)
        {
            SoftwareAgent agent = null;

            Uri agentUri = new Uri(p.agent);

            IModel model = ModelProvider.GetAgents();

            if (model.ContainsResource(agentUri))
            {
                agent = model.GetResource<SoftwareAgent>(agentUri);
            }

            return agent.IsCaptureEnabled;
        }

        protected Response GetSoftwareAgentStatus()
        {
            try
            {
                if (Request.Query.agent)
                {
                    AgentParameter p = new AgentParameter();
                    p.agent = Request.Query.agent.ToString();

                    return GetSoftwareAgentStatus(p);
                }
                else
                {
                    AgentParameter p = this.Bind<AgentParameter>();

                    return GetSoftwareAgentStatus(p);
                }
            }
            catch (Exception e)
            {
                return PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e);
            }
        }

        private Response GetSoftwareAgentStatus(AgentParameter p)
        {
            if (p.agent == null)
            {
                PlatformProvider.Logger.LogError(HttpStatusCode.BadRequest, "Invalid value for parameter 'agent': {0}", p.agent);

                // Return disabled state so that the browsers properly indicate disabled logging.
                p.enabled = false;

                return Response.AsJsonSync(p);
            }

            IModel model = ModelProvider.GetAgents();

            SoftwareAgent agent = null;
            Uri agentUri = new Uri(p.agent);

            if (model.ContainsResource(agentUri))
            {
                agent = model.GetResource<SoftwareAgent>(agentUri);
            }

            // Capturing agent data is disabled by default.
            p.enabled = agent != null ? agent.IsCaptureEnabled : false;

            PlatformProvider.Logger.LogRequest(HttpStatusCode.OK, Request.Url + " " + p.agent, "GET", "");

            return Response.AsJsonSync(p);
        }

        private Response PostSoftwareAgentStatus(AgentParameter p)
        {
            try
            {
                if (p.agent == null)
                {
                    PlatformProvider.Logger.LogError(HttpStatusCode.BadRequest, "Invalid value for parameter 'agent': {0}", p.agent);

                    // Return disabled state so that the browsers properly indicate disabled logging.
                    return Response.AsJsonSync(new AgentParameter() { agent = p.agent, enabled = false });
                }

                IModel model = ModelProvider.GetAgents();

                SoftwareAgent agent = null;
                Uri agentUri = new Uri(p.agent);

                if (model.ContainsResource(agentUri))
                {
                    agent = model.GetResource<SoftwareAgent>(agentUri);
                }
                else
                {
                    agent = model.CreateResource<SoftwareAgent>(agentUri);
                }

                agent.IsCaptureEnabled = Convert.ToBoolean(p.enabled);
                agent.Commit();

                PlatformProvider.Logger.LogRequest(HttpStatusCode.OK, Request.Url + " " + p.agent, "POST", "");

                // We return the request so that the plugin can set the server's enabled status.
                return Response.AsJsonSync(p);
            }
            catch (Exception e)
            {
                return PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e);
            }
        }

        private Response FileOpen(Uri fileUrl)
        {
            string file = Path.GetFileName(fileUrl.LocalPath);
            string folder = Path.GetDirectoryName(fileUrl.LocalPath);

            if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(folder))
            {
                PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);

                return Response.AsJsonSync(new { });
            }

            ISparqlQuery query = new SparqlQuery(@"
                SELECT 
                    ?uri ?file ?association
                WHERE
                {
                    {
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
                        ORDER BY DESC(?time) LIMIT 1
                        }UNION
                        {
                            SELECT
                            ?association
                            WHERE
                            {
                                ?association prov:hadRole art:AccountOwnerRole .
                            }
                    }
                }
                ");

            query.Bind("@fileName", file);
            query.Bind("@folderUrl", new Uri(folder));
            var bindings = ModelProvider.GetActivities().GetBindings(query).FirstOrDefault();

            return Response.AsJsonSync(bindings);
        }

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

            if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(folder))
            {
                PlatformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);

                return Response.AsJsonSync(new { });
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

            return Response.AsJsonSync(bindings);
        }

        #endregion
    }
}
