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

using Artivity.Api;
using Artivity.Api.Modules;
using Artivity.Api.Parameters;
using Artivity.Api.Platform;
using Artivity.Apid.Plugins;
using Artivity.Apid.Accounts;
using Artivity.DataModel;
using Nancy;
using Nancy.Responses;
using Nancy.ModelBinding;
using Nancy.IO;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace Artivity.Apid.Modules
{
    public class SoftwareModule : ModuleBase
    {
        #region Members

        private PluginChecker _checker;

        #endregion 

        #region Constructors

        public SoftwareModule(PluginChecker checker, IModelProvider modelProvider, IPlatformProvider platformProvider, IUserProvider userProvider)
            : base("/artivity/api/1.0/agents/software", modelProvider, platformProvider, userProvider)
        {
            _checker = checker;

            // Get a list of all installed agents.
            Get["/"] = paramters =>
            {
                if (IsUri(Request.Query.uri))
                {
                    UriRef uri = new UriRef(Request.Query.uri);

                    return GetSoftwareAgentFromUri(uri);
                }
                else if (IsUri(Request.Query.fileUri))
                {
                    UriRef fileUri = new UriRef(Request.Query.fileUri);

                    return GetSoftwareAgentFromFileUri(fileUri);
                }
                else if(Request.Query.Count == 0)
                {
                    return GetSoftwareAgents();
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

                    return PostAgents(data);
                }
            };

            Get["/associations"] = parameters =>
            {
                if (Request.Query.Count == 0)
                {
                    return GetSoftwareAgentAssociations();
                }

                string role = Request.Query.role;

                if (string.IsNullOrEmpty(role) || !Uri.IsWellFormedUriString(role, UriKind.Absolute))
                {
                    return HttpStatusCode.BadRequest;
                }

                string agent = Request.Query.agent;
                string version = Request.Query.version;

                if (string.IsNullOrEmpty(agent))
                {
                    return GetSoftwareAgentAssociation(new UriRef(role));
                }

                if (string.IsNullOrEmpty(version) || !Uri.IsWellFormedUriString(agent, UriKind.Absolute))
                {
                    return HttpStatusCode.BadRequest;
                }

                return GetSoftwareAgentAssociation(new UriRef(role), new UriRef(agent), version);
            };

            #if DEBUG
            Get["/initialize"] = parameters =>
            {
                ModelProvider.InitializeAgents();

                return HttpStatusCode.OK;
            };
            #endif

            Get["/icon"] = paramters =>
            {
                string uri = Request.Query["uri"];

                if (!IsUri(uri))
                {
                    return HttpStatusCode.BadRequest;
                }

                return GetSoftwareAgentIcon(new Uri(uri));
            };

            Get["/install"] = paramters =>
            {
                string uri = Request.Query["uri"];

                if (!IsUri(uri))
                {
                    return HttpStatusCode.BadRequest;
                }

                return InstallSoftwareAgent(new Uri(uri));
            };

            Get["/uninstall"] = paramters =>
            {
                string uri = Request.Query["uri"];

                if (!IsUri(uri))
                {
                    return HttpStatusCode.BadRequest;
                }

                return UninstallSoftwareAgent(new Uri(uri));
            };

            Get["/paths"] = paramters =>
            {
                return GetPluginSearchDirectories();
            };

            Post["/paths"] = paramters =>
            {
                string url = Request.Query["url"];

                if (!IsFileUrl(url))
                {
                    return HttpStatusCode.BadRequest;
                }

                return PostPluginSearchDirectory(new Uri(url));
            };

            Delete["/paths"] = paramters =>
            {
                string url = Request.Query["url"];

                if (!IsFileUrl(url))
                {
                    return HttpStatusCode.BadRequest;
                }

                return DeletePluginSearchDirectory(new Uri(url));
            };

            #if DEBUG
            Get["/clear"] = parameters =>
            {
                return ClearAgents();
            };
            #endif
        }

        #endregion

        #region Methods

        public Response GetSoftwareAgents()
        {
            _checker.CheckPlugins();

            return Response.AsJsonSync(_checker.Plugins);
        }

        public Response GetSoftwareAgentFromUri(Uri uri)
        {
            _checker.CheckPlugins();

            return Response.AsJsonSync(_checker.Plugins.FirstOrDefault(p => p.Manifest.AgentUri == uri.AbsoluteUri));
        }

        private Response GetSoftwareAgentFromFileUri(Uri fileUri)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?agent ?color ?association
                WHERE 
                {
                    ?activity prov:used | prov:generated ?e.
                    ?e nie:isStoredAs @file .
                    ?activity prov:qualifiedAssociation ?association .

                    ?association prov:hadRole art:SOFTWARE .
                    ?association prov:agent ?agent .

                    OPTIONAL { ?agent art:hasColourCode ?color . }
                }
                LIMIT 1");

            query.Bind("@entity", fileUri);

            BindingSet bindings = ModelProvider.GetAll().GetBindings(query).FirstOrDefault();

            return Response.AsJsonSync(bindings);
        }

        public Response PostAgents(string agentData)
        {
            IModel model = ModelProvider.GetAgents();

            SoftwareAgentParameter[] agents = JsonConvert.DeserializeObject<SoftwareAgentParameter[]>(agentData);

            foreach (SoftwareAgentParameter agent in agents)
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

        public Response GetSoftwareAgentIcon(Uri agentUri)
        {
            SoftwareAgentPlugin plugin = _checker.Plugins.FirstOrDefault(p => p.Manifest.AgentUri == agentUri.AbsoluteUri);

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

        public Response InstallSoftwareAgent(Uri agentUri)
        {
            try
            {
                if (_checker.InstallPlugin(agentUri))
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

                return Response.AsJsonSync(error, HttpStatusCode.InternalServerError);
            }
        }

        public Response UninstallSoftwareAgent(Uri agentUri)
        {
            try
            {
                if (_checker.UninstallPlugin(agentUri))
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

                return Response.AsJsonSync(error, HttpStatusCode.InternalServerError);
            }
        }

        private Response GetSoftwareAgentAssociations()
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

            return Response.AsJsonSync(bindings);
        }

        private Response GetSoftwareAgentAssociation(UriRef role)
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

            return Response.AsJsonSync(bindings);
        }

        private Response GetSoftwareAgentAssociation(UriRef role, UriRef agent, string version)
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT
                    ?association ?agent ?role
                WHERE
                {
                    ?association prov:hadRole @role
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

            return Response.AsJsonSync(bindings);
        }

        private Response GetPluginSearchDirectories()
        {
            return Response.AsJsonSync(PlatformProvider.Config.SoftwarePaths);
        }

        private Response PostPluginSearchDirectory(Uri url)
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

        private Response DeletePluginSearchDirectory(Uri url)
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

        private Response ClearAgents()
        {
            ModelProvider.GetAgents().Clear();

            return HttpStatusCode.OK;
        }

        #endregion

        #region Types

        private class SoftwareAgentParameter
        {
            public string uri { get; set; }

            public string name { get; set; }

            public string color { get; set; }

            public bool pluginEnabled { get; set; }
        }

        #endregion
    }
}
