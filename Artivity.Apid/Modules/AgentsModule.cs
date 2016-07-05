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
using Artivity.Apid.Accounts;
using Artivity.Apid.Parameters;
using Artivity.Apid.Platforms;
using Artivity.Api.Plugin;
using Nancy;
using Nancy.Responses;
using Nancy.ModelBinding;
using Nancy.IO;
using Nancy.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

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
                Agent agent = Bind<Agent>(ModelProvider.Store, Request.Body);

                if (agent == null)
                {
                    return HttpStatusCode.BadRequest;
                }

                agent.Commit();

                return HttpStatusCode.OK;
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

            Get["/status"] = parameters =>
            {
                return GetAgentStatus();
            };

            Post["/status"] = parameters =>
            {
                return PostAgentStatus(this.Bind<AgentParameters>());
            };
        }

        #endregion

        #region Methods

        public Response GetAgents()
        {
            _checker.CheckPlugins();

            return Response.AsJson(_checker.Plugins);
        }

        public Response GetAgent(Uri uri)
        {
            _checker.CheckPlugins();

            return Response.AsJson(_checker.Plugins.FirstOrDefault(p => p.AssociationUri == uri));
        }

        private Response GetAgentFromEntity(Uri entityUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?agent ?association
                WHERE 
                {
                    ?activity prov:used | prov:generated @entity .
                    ?activity prov:qualifiedAssociation ?association .

                    ?association prov:hadRole art:SOFTWARE .
                    ?association prov:agent ?agent .
                }
                LIMIT 1");

            query.Bind("@entity", entityUri);

            BindingSet bindings = ModelProvider.GetAllActivities().GetBindings(query).FirstOrDefault();

            return Response.AsJson(bindings);
        }

        public Response GetAgentIcon(Uri uri)
        {
            SoftwareAgentPlugin plugin = _checker.Plugins.FirstOrDefault(p => p.AssociationUri == uri);

            if (plugin == null || plugin.ExecutableIcon == null)
            {
                return null;
            }

            string iconPath = plugin.ExecutableIcon.LocalPath;

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

            // See if there is already a person defined..
            Person user = ModelProvider.GetAgents().ExecuteQuery(query).GetResources<Person>().FirstOrDefault();

            if (user == null)
            {
                Logger.LogInfo("Creating new user profile..");

                // If not, create one.
                user = ModelProvider.GetAgents().CreateResource<Person>();
                user.Commit();
            }
            else
            {
                Logger.LogInfo("Upgrading user profile..");
            }

            // Add the user role association.
            Association association = ModelProvider.GetAgents().CreateResource<Association>();
            association.Agent = user;
            association.Role = new Role(art.USER);
            association.Commit();

            return association;
        }

        private Response GetAgentAssociations()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?association ?agent ?role
                WHERE
                {
                    ?association prov:agent ?agent .
                    ?association prov:hadRole ?role .
                }
            ");

            var bindings = ModelProvider.GetAgents().GetBindings(query);

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

            var bindings = ModelProvider.GetAgents().GetBindings(query).FirstOrDefault();

            if (bindings == null)
            {
                Logger.LogInfo("Creating association for agent {0} ; version {1}", agent, version);

                // The URI format of the new agent is always {AGENT_URI}#{VERSION}
                UriRef uri = new UriRef(agent.AbsoluteUri + "#" + version.Replace(' ', '_').Trim());

                SoftwareAssociation association = ModelProvider.GetAgents().CreateResource<SoftwareAssociation>(uri);
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

            return Response.AsJson(bindings);
        }

        protected Response GetAgentStatus()
        {
            try
            {
                if (Request.Query.agent)
                {
                    AgentParameters p = new AgentParameters();
                    p.agent = Request.Query.agent.ToString();

                    return GetAgentStatus(p);
                }
                else
                {
                    return GetAgentStatus(this.Bind<AgentParameters>());
                }
            }
            catch (Exception e)
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e);
            }
        }

        protected Response GetAgentStatus(AgentParameters p)
        {
            if (p.agent == null)
            {
                Logger.LogError(HttpStatusCode.BadRequest, "Invalid value for parameter 'agent': {0}", p.agent);

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

            Logger.LogRequest(HttpStatusCode.OK, Request.Url + " " + p.agent, "GET", "");

            return Response.AsJson(p);
        }

        protected Response PostAgentStatus(AgentParameters p)
        {
            try
            {
                if (p.agent == null)
                {
                    Logger.LogError(HttpStatusCode.BadRequest, "Invalid value for parameter 'agent': {0}", p.agent);

                    // Return disabled state so that the browsers properly indicate disabled logging.
                    return Response.AsJson(new AgentParameters() { agent = p.agent, enabled = false });
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

                Logger.LogRequest(HttpStatusCode.OK, Request.Url + " " + p.agent, "POST", "");

                // We return the request so that the plugin can set the server's enabled status.
                return Response.AsJson(p);
            }
            catch (Exception e)
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, Request.Url, e);
            }
        }

        private Response SetUserAgent(RequestStream stream)
        {
            Person user = Bind<Person>(ModelProvider.Store, stream);

            if (user == null)
            {
                return HttpStatusCode.BadRequest;
            }

            user.Commit();

            return HttpStatusCode.OK;
        }

        private Response GetUserAgentPhoto()
        {
            string uid = PlatformProvider.Config.GetUserId();
            string file = Path.Combine(PlatformProvider.AvatarsFolder, uid + ".jpg");

            if (!File.Exists(file))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                using (Stream source = assembly.GetManifestResourceStream("Artivity.Api.Resources.user.jpg"))
                {
                    using (FileStream target = File.Create(file))
                    {
                        source.CopyTo(target);
                    }
                }
            }

            FileStream fileStream = new FileStream(file, FileMode.Open);

            StreamResponse response = new StreamResponse(() => fileStream, MimeTypes.GetMimeType(file));

            return response.AsAttachment(file);
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
                Logger.LogError(ex.Message);

                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }

        #endregion
    }
}
