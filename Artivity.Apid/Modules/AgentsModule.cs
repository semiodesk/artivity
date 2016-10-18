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
using Artivity.Apid.Parameters;
using Artivity.Apid.Platforms;
using Artivity.Apid.Plugin;
using Nancy;
using Nancy.Responses;
using Nancy.ModelBinding;
using Nancy.IO;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using Newtonsoft.Json;

namespace Artivity.Apid.Modules
{
    public class AgentsModule : ModuleBase
    {
        #region Members

        PluginChecker _checker;

        #endregion 

        #region Constructors

        public AgentsModule(PluginChecker checker, IModelProvider modelProvider, IPlatformProvider platform)
            : base("/artivity/api/1.0/agents", modelProvider, platform)
        {
            _checker = checker;

            // Get a list of all installed agents.
            Get["/"] = paramters =>
            {
                string uri = Request.Query["uri"];
                string entityUri = Request.Query["entityUri"];

                if (IsUri(entityUri))
                {
                    return GetAgentFromEntity(new Uri(entityUri));
                }
                else if (string.IsNullOrEmpty(uri))
                {
                    return GetAgents();
                }
                else if (IsUri(uri))
                {
                    return GetAgent(new Uri(uri));
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            // Install a new or update an existing agent.
            Post["/"] = parameters =>
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    string data = reader.ReadToEnd();

                    if(string.IsNullOrEmpty(data))
                    {
                        return HttpStatusCode.BadRequest;
                    }

                    return SetAgents(data);
                }
            };

            Get["/associations"] = parameters =>
            {
                if (Request.Query.Count == 0)
                {
                    return GetAgentAssociations();
                }

                string role = Request.Query["role"];

                if (string.IsNullOrEmpty(role) || !Uri.IsWellFormedUriString(role, UriKind.Absolute))
                {
                    return HttpStatusCode.BadRequest;
                }

                string agent = Request.Query["agent"];
                string version = Request.Query["version"];

                if (string.IsNullOrEmpty(agent))
                {
                    return GetAgentAssociation(new UriRef(role));
                }

                if (string.IsNullOrEmpty(version) || !Uri.IsWellFormedUriString(agent, UriKind.Absolute))
                {
                    return HttpStatusCode.BadRequest;
                }

                return GetAgentAssociation(new UriRef(role), new UriRef(agent), version);
            };

            Get["/initialize"] = parameters =>
            {
                ModelProvider.InitializeAgents();

                return HttpStatusCode.OK;
            };

            Get["/user"] = parameters =>
            {
                return GetUserAgent();
            };

            Post["/user"] = parameters =>
            {
                return SetUserAgent(Request.Body);
            };

            Get["/user/photo"] = parameters =>
            {
                return GetUserAgentPhoto();
            };

            Post["/user/photo"] = parameters =>
            {
                RequestStream stream = Request.Body;

                return SetUserAgentPhoto(stream);
            };

            Get["/software/icon"] = paramters =>
            {
                string uri = Request.Query["uri"];

                if (!IsUri(uri))
                {
                    return HttpStatusCode.BadRequest;
                }

                return GetAgentIcon(new Uri(uri));
            };

            Get["/software/install"] = paramters =>
            {
                string uri = Request.Query["uri"];

                if (!IsUri(uri))
                {
                    return HttpStatusCode.BadRequest;
                }

                return InstallAgent(new Uri(uri));
            };

            Get["/software/uninstall"] = paramters =>
            {
                string uri = Request.Query["uri"];

                if (!IsUri(uri))
                {
                    return HttpStatusCode.BadRequest;
                }

                return UninstallAgent(new Uri(uri));
            };

            Get["/software/paths"] = paramters =>
            {
                return GetDirectories();
            };

            Get["/software/paths/add"] = paramters =>
            {
                string url = Request.Query["url"];

                if (!IsFileUrl(url))
                {
                    return HttpStatusCode.BadRequest;
                }

                return AddDirectory(new Uri(url));
            };

            Get["/software/paths/remove"] = paramters =>
            {
                string url = Request.Query["url"];

                if (!IsFileUrl(url))
                {
                    return HttpStatusCode.BadRequest;
                }

                return RemoveDirectory(new Uri(url));
            };

            Get["/status"] = parameters =>
            {
                return GetAgentStatus();
            };

            Post["/status"] = parameters =>
            {
                return PostAgentStatus(this.Bind<AgentParameter>());
            };
        }

        #endregion

        #region Methods

        public Response GetAgents()
        {
            _checker.CheckPlugins();

            return Response.AsJson(_checker.Plugins);
        }

        public Response SetAgents(string data)
        {
            IModel model = ModelProvider.GetAgents();

            AgentSettingsParameter[] agents = JsonConvert.DeserializeObject<AgentSettingsParameter[]>(data);

            foreach (AgentSettingsParameter agent in agents)
            {
                Uri agentUri = new Uri(agent.uri);

                SoftwareAgent a;

                if (model.ContainsResource(agentUri))
                {
                    a = model.GetResource<SoftwareAgent>(agentUri);
                }
                else
                {
                    a = model.CreateResource<SoftwareAgent>(agentUri);
                }

                a.Name = agent.name;
                a.ColourCode = agent.color;
                a.Commit();
            }

            return HttpStatusCode.OK;
        }

        public Response GetAgent(Uri uri)
        {
            _checker.CheckPlugins();

            return Response.AsJson(_checker.Plugins.FirstOrDefault(p => p.Manifest.AgentUri == uri.AbsoluteUri));
        }

        private Response GetAgentFromEntity(Uri entityUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?agent ?color ?association
                WHERE 
                {
                    ?activity prov:used | prov:generated @entity .
                    ?activity prov:qualifiedAssociation ?association .

                    ?association prov:hadRole art:SOFTWARE .
                    ?association prov:agent ?agent .

                    OPTIONAL { ?agent art:hasColourCode ?color . }
                }
                LIMIT 1");

            query.Bind("@entity", entityUri);

            BindingSet bindings = ModelProvider.GetAll().GetBindings(query).FirstOrDefault();

            return Response.AsJson(bindings);
        }

        public Response GetAgentIcon(Uri uri)
        {
            SoftwareAgentPlugin plugin = _checker.Plugins.FirstOrDefault(p => p.Manifest.AgentUri == uri.AbsoluteUri);

            if (plugin == null)
            {
                return null;
            }

            Uri iconUrl = plugin.GetIcon();

            if (iconUrl == null)
            {
                return null;
            }

            string iconPath = iconUrl.LocalPath;

            if (File.Exists(iconPath))
            {
                FileStream fileStream = new FileStream(iconPath, FileMode.Open);

                StreamResponse response = new StreamResponse(() => fileStream, MimeTypes.GetMimeType(iconPath));

                return response.AsAttachment(iconPath);
            }

            return null;
        }

        public Response InstallAgent(Uri uri)
        {
            try
            {
                if (_checker.InstallPlugin(uri))
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.NotModified;
                }
            }
            catch(Exception e)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error["type"] = e.GetType().ToString();
                error["message"] = e.Message;

                return Response.AsJson(error, HttpStatusCode.InternalServerError);
            }
        }

        public Response UninstallAgent(Uri uri)
        {
            try
            {
                if (_checker.UninstallPlugin(uri))
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.NotModified;
                }
            }
            catch (Exception e)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error["type"] = e.GetType().ToString();
                error["message"] = e.Message;

                return Response.AsJson(error, HttpStatusCode.InternalServerError);
            }
        }

        private Response GetUserAgent()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?s ?p ?o
                WHERE
                {
                    ?s ?p ?o .

                    ?a prov:agent ?s .
                    ?a prov:hadRole art:USER .
                }
            ");

            IEnumerable<Person> persons = ModelProvider.GetAgents().GetResources<Person>(query);

            if (persons.Any())
            {
                return Response.AsJson(persons.First());
            }
            else
            {
                Association association = CreateUserAssociation();

                return Response.AsJson(association.Agent);
            }
        }

        private Association CreateUserAssociation()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?s ?p ?o
                WHERE
                {
                    ?s ?p ?o .

                    ?s rdf:type prov:Person .
                }
            ");

            IModel model = ModelProvider.GetAgents();

            // See if there is already a person defined..
            Person user = model.ExecuteQuery(query).GetResources<Person>().FirstOrDefault();

            if (user == null)
            {
                PlatformProvider.Logger.LogInfo("Creating new user profile..");

                // If not, create one.
                user = model.CreateResource<Person>();
                user.Commit();
            }
            else
            {
                PlatformProvider.Logger.LogInfo("Upgrading user profile..");
            }

            // Add the user role association.
            Association association = model.CreateResource<Association>();
            association.Agent = user;
            association.Role = new Role(art.USER);
            association.Commit();

            return association;
        }

        private Response GetAgentAssociations()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT
                    ?association ?agent ?role
                WHERE
                {
                    ?association prov:agent ?agent .
                    ?association prov:hadRole ?role .
                }
            ");

            var bindings = ModelProvider.GetAgents().GetBindings(query);

            PlatformProvider.Logger.LogRequest(HttpStatusCode.OK, Request);

            return Response.AsJson(bindings);
        }

        private Response GetAgentAssociation(UriRef role)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?association ?agent ?role
                WHERE
                {
                    ?association prov:agent ?agent .
                    ?association prov:hadRole @role .

                    BIND(@role as ?role)
                }
            ");

            query.Bind("@role", role);

            var bindings = ModelProvider.GetAgents().GetBindings(query).FirstOrDefault();

            PlatformProvider.Logger.LogRequest(HttpStatusCode.OK, Request);

            return Response.AsJson(bindings);
        }

        private Response GetAgentAssociation(UriRef role, UriRef agent, string version)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?association ?agent ?role
                WHERE
                {
                    ?association prov:hadRole @role .
                    ?association prov:agent @agent .
                    ?association art:version @version .

                    BIND(@agent as ?agent)
                    BIND(@role as ?role)
                }
            ");

            query.Bind("@role", role);
            query.Bind("@agent", agent);
            query.Bind("@version", version);

            IModel model = ModelProvider.GetAgents();

            var bindings = model.GetBindings(query).FirstOrDefault();

            if (bindings == null)
            {
                PlatformProvider.Logger.LogInfo("Creating association for agent {0} ; version {1}", agent, version);

                // The URI format of the new agent is always {AGENT_URI}#{VERSION}
                UriRef uri = new UriRef(agent.AbsoluteUri + "#" + version.Replace(' ', '_').Trim());

                SoftwareAssociation association = model.CreateResource<SoftwareAssociation>(uri);
                association.Agent = new Agent(agent);
                association.Role = new Role(role);
                association.ExecutableVersion = version;
                association.Commit();

                // Return the bindings for the new agent association.
                bindings = new BindingSet()
                {
                    { "association", uri },
                    { "agent", agent },
                    { "role", role },
                };
            }

            PlatformProvider.Logger.LogRequest(HttpStatusCode.OK, Request);

            return Response.AsJson(bindings);
        }

        protected Response GetAgentStatus()
        {
            try
            {
                if (Request.Query.agent)
                {
                    AgentParameter p = new AgentParameter();
                    p.agent = Request.Query.agent.ToString();

                    return GetAgentStatus(p);
                }
                else
                {
                    return GetAgentStatus(this.Bind<AgentParameter>());
                }
            }
            catch (Exception e)
            {
                return PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e);
            }
        }

        protected Response GetAgentStatus(AgentParameter p)
        {
            if (p.agent == null)
            {
                PlatformProvider.Logger.LogError(HttpStatusCode.BadRequest, "Invalid value for parameter 'agent': {0}", p.agent);

                // Return disabled state so that the browsers properly indicate disabled logging.
                p.enabled = false;

                return Response.AsJson(p);
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

            return Response.AsJson(p);
        }

        protected Response PostAgentStatus(AgentParameter p)
        {
            try
            {
                if (p.agent == null)
                {
                    PlatformProvider.Logger.LogError(HttpStatusCode.BadRequest, "Invalid value for parameter 'agent': {0}", p.agent);

                    // Return disabled state so that the browsers properly indicate disabled logging.
                    return Response.AsJson(new AgentParameter() { agent = p.agent, enabled = false });
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
                return Response.AsJson(p);
            }
            catch (Exception e)
            {
                return PlatformProvider.Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e);
            }
        }

        private Response SetUserAgent(RequestStream stream)
        {
            Person user = Bind<Person>(ModelProvider.Store, stream);

            if (user == null)
            {
                using (var reader = new StreamReader(stream))
                {
                    string data = reader.ReadToEnd();

                    return PlatformProvider.Logger.LogError(HttpStatusCode.BadRequest, data);
                }
            }

            user.Commit();

            return HttpStatusCode.OK;
        }

        private Response GetUserAgentPhoto()
        {
            try
            {
                string uid = PlatformProvider.Config.GetUserId();
                string file = Path.Combine(PlatformProvider.AvatarsFolder, uid + ".jpg");

                if (!File.Exists(file))
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();

                    using (Stream source = assembly.GetManifestResourceStream("Artivity.Apid.Resources.user.jpg"))
                    {
                        using (FileStream target = File.Create(file))
                        {
                            source.CopyTo(target);
                        }
                    }
                }

                FileStream fileStream = new FileStream(file, FileMode.Open);

                StreamResponse response = new StreamResponse(() => fileStream, MimeTypes.GetMimeType(file));
                response.Headers["Allow-Control-Allow-Origin"] = "127.0.0.1";

                return response.AsAttachment(file);
            }
            catch(IOException ex)
            {
                PlatformProvider.Logger.LogError(ex);

                return HttpStatusCode.InternalServerError;
            }
        }

        private Response SetUserAgentPhoto(RequestStream stream)
        {
            try
            {
                string uid = PlatformProvider.Config.GetUserId();
                string file = Path.Combine(PlatformProvider.AvatarsFolder, uid + ".jpg");

                Bitmap source = new Bitmap(stream);

                // Always resize the image to the given size.
                int width = 160;
                int height = 160;

                Bitmap target = new Bitmap(width, height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(source, 0, 0, width, height);

                    using (FileStream fileStream = File.Create(file))
                    {
                        target.Save(fileStream, ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception ex)
            {
                PlatformProvider.Logger.LogError(ex.Message);

                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }

        Response GetDirectories()
        {
            return Response.AsJson(PlatformProvider.Config.SoftwarePaths);
        }

        Response AddDirectory(Uri url)
        {
            UserConfig config = PlatformProvider.Config;

            if (!config.SoftwarePaths.Contains(url.LocalPath))
            {
                config.SoftwarePaths.Add(url.LocalPath);

                PlatformProvider.WriteConfig(config);
            }

            PlatformProvider.Logger.LogInfo("Added software agent search path: {0}", url.LocalPath);

            return HttpStatusCode.OK;
        }

        Response RemoveDirectory(Uri url)
        {
            UserConfig config = PlatformProvider.Config;

            if (config.SoftwarePaths.Contains(url.LocalPath))
            {
                config.SoftwarePaths.Remove(url.LocalPath);

                PlatformProvider.WriteConfig(config);
            }

            PlatformProvider.Logger.LogInfo("Removed software agent search path: {0}", url.LocalPath);

            return HttpStatusCode.OK;
        }

        #endregion
    }

    class AgentSettingsParameter
    {
        public string uri { get; set; }

        public string name { get; set; }

        public string color { get; set; }

        public bool pluginEnabled { get; set; }
    }
}
