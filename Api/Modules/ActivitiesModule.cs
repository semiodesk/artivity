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
        #region Members

        private static object _modelLock = new object();

        private static readonly Dictionary<string, Browse> _activities = new Dictionary<string, Browse>();

        private IPlatformProvider _platformProvider;

        #endregion 

        #region Constructors

        public ActivitiesModule(IModelProvider modelProvider, IPlatformProvider platform)
            : base("/artivity/api/1.0/activities", modelProvider, platform)
        {
            _platformProvider = platform;

            Get["/"] = parameters =>
            {
                string uri = Request.Query.uri;

                if (string.IsNullOrEmpty(uri) || !IsUri(uri))
                {
                    return GetActivities();
                }
                else
                {
                    return GetActivities(new UriRef(uri));
                }
            };

            Post["/"] = parameters =>
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    string data = reader.ReadToEnd();

                    // We start a new task so that the agent can continue its work as fast as possible.
                    new Task(() => PostActivities(data)).Start();

                    return HttpStatusCode.OK;
                }
            };

            Post["/web"] = parameters =>
            {
                ActivityParameter p = this.Bind<ActivityParameter>();

                return PostActivity(p);
            };

            Get["/clear"] = parameters =>
            {
                // TODO: We definitely need to add some kind of security here, i.e. a token.
                return ClearActivities();
            };
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
                    SAMPLE(COALESCE(?agentColor,'#FF0000')) AS ?agentColor
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

        private Response GetActivities(UriRef entityUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT
                    ?activity AS ?uri
                    ?startTime 
                    ?endTime
                    MAX(?time) as ?maxTime
                    ?agent
                    SAMPLE(COALESCE(?agentColor,'#FF0000')) AS ?agentColor
                WHERE
                {
                  ?activity
                    prov:generated | prov:used @entity ;
                    prov:startedAtTime ?startTime .

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

            query.Bind("@entity", entityUri);

            var bindings = ModelProvider.GetAll().GetBindings(query).ToList();

            return Response.AsJsonSync(bindings);
        }

        private Response PostActivities(string data)
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

        private Response PostActivity(ActivityParameter p)
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
                    return _platformProvider.Logger.LogError(HttpStatusCode.BadRequest, "Invalid parameters.", p);
                }

                if (string.IsNullOrEmpty(p.tab))
                {
                    return _platformProvider.Logger.LogRequest(HttpStatusCode.NotModified, Request.Url, "POST", p.tab);
                }

                if (!IsCaptureEnabled(p))
                {
                    return _platformProvider.Logger.LogRequest(HttpStatusCode.Locked, Request.Url, "POST", "");
                }

                IModel model = ModelProvider.GetWebActivities();

                Browse activity;

                if (!_activities.ContainsKey(p.tab))
                {
                    Association association = model.CreateResource<Association>();
                    association.Agent = new SoftwareAgent(new UriRef(p.agent));
                    association.Role = new Role(art.SOFTWARE);
                    association.Commit();

                    activity = model.CreateResource<Browse>();
                    activity.Associations.Add(association);
                    activity.StartTime = p.startTime != null ? (DateTime)p.startTime : DateTime.Now;
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
                    activity.EndTime = (DateTime)p.endTime;
                    activity.Commit();

                    _activities.Remove(p.tab);

                    _platformProvider.Logger.LogRequest(HttpStatusCode.OK, Request.Url, "POST", "");
                }

                return _platformProvider.Logger.LogError(HttpStatusCode.BadRequest, Request.Url);
            }
            catch (Exception e)
            {
                return _platformProvider.Logger.LogError(HttpStatusCode.InternalServerError, "{0}: {1}", Request.Url, e.Message);
            }
        }

        private Response ClearActivities()
        {
            ModelProvider.GetActivities().Clear();

            return HttpStatusCode.OK;
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
        #endregion
    }
}
