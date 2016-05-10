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
using Artivity.DataModel.Journal;
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Artivity.Apid.Parameters;
using Artivity.Apid.Accounts;
using Artivity.Apid.Platforms;
using System.Globalization;

namespace Artivity.Apid.Modules
{
    public class ApiModule : ModuleBase
    {
        #region Constructors

        public ApiModule(IModelProvider modelProvider, IPlatformProvider platform)
            : base("/artivity/api/1.0", modelProvider, platform)
        {

            // Get a list of all installed agents.
            Get["/agents"] = paramters =>
            {
                string fileUrl = Request.Query["fileUrl"];

                if(string.IsNullOrEmpty(fileUrl))
                {
                    return GetAgents();
                }
                else
                {
                    return GetAgent(fileUrl);
                }
            };

            // Install a new or update an existing agent.
            Post["/agents"] = parameters =>
            {
                Agent agent = Bind<Agent>(ModelProvider.Store, Request.Body);

                if (agent == null)
                {
                    return HttpStatusCode.BadRequest;
                }

                agent.Commit();

                return HttpStatusCode.OK;
            };

            Get["/agents/install"] = parameters =>
            {
                IModel model = ModelProvider.AgentsModel;

                InstallAgent(model, "application://inkscape.desktop/", "Inkscape", "inkscape", "#EE204E", true);
                InstallAgent(model, "application://krita.desktop/", "Krita", "krita", "#926EAE", true);
                InstallAgent(model, "application://chromium-browser.desktop/", "Chromium", "chromium-browser", "#1F75FE");
                InstallAgent(model, "application://firefox-browser.desktop/", "Firefox", "firefox", "#1F75FE");
                InstallAgent(model, "application://photoshop.desktop", "Adobe Photoshop", "photoshop", "#EE2000", true);
                InstallAgent(model, "application://illustrator.desktop", "Adobe Illustrator", "illustrator", "#EE2000", true);

                return HttpStatusCode.OK;
            };

            // Get a list of all installed online accounts.
            Get["/accounts"] = parameters =>
            {
                return GetAccounts();
            };

            // Get a list of all supported online account types.
            Get["/accounts/providers"] = parameters =>
            {
                string providerId = Request.Query["providerId"];

                if (string.IsNullOrEmpty(providerId))
                {
                    return GetAccountProviders();
                }
                else
                {
                    return GetAccountProvider(providerId);
                }
            };

            Get["/accounts/oauth2/redirect"] = parameters =>
            {
                string providerId = Request.Query["providerId"];

                if (string.IsNullOrEmpty(providerId))
                {
                    return HttpStatusCode.BadRequest;
                }

                return GetOAuth2AccountRedirectUrl(providerId);
            };

            Get["/accounts/oauth2/token"] = parameters =>
            {
                string providerId = Request.Query["providerId"];
                string code = Request.Query["code"];

                if (string.IsNullOrEmpty(providerId) || string.IsNullOrEmpty(code))
                {
                    return HttpStatusCode.BadRequest;
                }

                return SendOAuth2AccessToken(providerId, code);
            };

            Get["/accounts/install"] = parameters =>
            {
                string providerId = Request.Query["providerId"];

                if (string.IsNullOrEmpty(providerId))
                {
                    return HttpStatusCode.BadRequest;
                }

                return InstallAccount(providerId);
            };

            // Uninstall a account with a specific id.
            Get["/accounts/uninstall"] = parameters =>
            {
                string accountId = Request.Query["accountId"];

                if (string.IsNullOrEmpty(accountId))
                {
                    return HttpStatusCode.BadRequest;
                }

                return UninstallAccount(accountId);
            };

            Get["/user"] = parameters =>
            {
                return GetUserAgent();
            };

            Post["/user"] = parameters =>
            {
                Person user = Bind<Person>(ModelProvider.Store, Request.Body);

                if (user == null)
                {
                    return HttpStatusCode.BadRequest;
                }

                user.Commit();

                return HttpStatusCode.OK;
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

            Get["/files/recent"] = parameters =>
            {
                return GetRecentlyUsedFiles();
            };

            Get["/activities"] = parameters =>
            {
                if (string.IsNullOrEmpty(Request.Query.fileUrl))
                {
                    return HttpStatusCode.BadRequest;
                }

                return GetActivities(Request.Query.fileUrl);
            };

            Get["/activities/remove/all"] = parameters =>
            {
                // TODO: We definitely need to add some kind of security here, i.e. a token.
                return RemoveAllActivities();
            };

            Get["/influences"] = parameters =>
            {
                if (string.IsNullOrEmpty(Request.Query.fileUrl))
                {
                    return HttpStatusCode.BadRequest;
                }

                return GetInfluences(Request.Query.fileUrl);
            };

            Get["/thumbnails"] = parameters =>
            {
                if(Request.Query.fileUrl)
                {
                    if(Request.Query.timestamp)
                    {
                        string time = Request.Query.timestamp;

                        DateTimeOffset timestamp = DateTimeOffset.Parse(time.Replace(' ', '+'));

                        return GetThumbnails(Request.Query.fileUrl, timestamp.UtcDateTime);
                    }

                    return GetThumbnails(Request.Query.fileUrl);
                }
                else if(Request.Query.thumbnailUrl)
                {
                    return GetThumbnail(Request.Query.thumbnailUrl);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Get["/thumbnails/paths"] = parameters =>
            {
                if (string.IsNullOrEmpty(Request.Query.fileUrl))
                {
                    return HttpStatusCode.BadRequest;
                }

                return GetThumbnailPaths(Request.Query.fileUrl);
            };
        }

        #endregion

        #region Methods

        public Response GetAgents()
        {
            IModel model = ModelProvider.GetAgents();

            IEnumerable<SoftwareAgent> agents = model.GetResources<SoftwareAgent>();

            return Response.AsJson(agents);
        }

        public Response GetAgent(string fileUrl)
        {
            IModel model = ModelProvider.GetAllActivities();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?s ?p ?o WHERE 
                {
                    ?s ?p ?o .

                    {
                        SELECT ?s WHERE
                        {
                            ?file nfo:fileUrl @fileUrl .

                            ?activity prov:used ?file;
                                prov:qualifiedAssociation ?association .

                            ?association prov:agent ?s .

                            ?s a prov:SoftwareAgent .
                        }
                        LIMIT 1
                    }
                }");

            query.Bind("@fileUrl", fileUrl);

            IEnumerable<IResource> agents = model.GetResources(query);

            if (agents.Any())
            {
                return Response.AsJson(agents.FirstOrDefault());
            }
            else
            {
                return null;
            }
        }

        public void InstallAgent(IModel model, string uri, string name, string executableName, string colour, bool captureEnabled = false)
        {
            UriRef agentUri = new UriRef(uri);

            if (!model.ContainsResource(agentUri))
            {
                Logger.LogInfo("Installing agent {0}", name);

                SoftwareAgent agent = model.CreateResource<SoftwareAgent>(agentUri);
                agent.Name = name;
                agent.ExecutableName = executableName;
                agent.IsCaptureEnabled = captureEnabled;
                agent.ColourCode = colour;
                agent.Commit();
            }
            else
            {
                bool modified = false;

                SoftwareAgent agent = model.GetResource<SoftwareAgent>(agentUri);

                if (agent.Name != name)
                {
                    agent.Name = name;

                    modified = true;
                }

                if (string.IsNullOrEmpty(agent.ColourCode))
                {
                    agent.ColourCode = colour;

                    modified = true;
                }

                if (modified)
                {
                    Logger.LogInfo("Updating agent {0}", name);

                    agent.Commit();
                }
            }
        }

        public Response GetUserAgent()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?s ?p ?o WHERE
                {
                    ?s ?p ?o .
                    ?s rdf:type foaf:Person .
                }
            ");

            ISparqlQueryResult result = ModelProvider.AgentsModel.ExecuteQuery(query);

            if (result.GetResources<Person>().Any())
            {
                return Response.AsJson(result.GetResources<Person>().First());
            }
            else
            {
                return null;
            }
        }

        private Response GetAccounts()
        {
            IModel model = ModelProvider.GetAgents();

            IEnumerable<OnlineAccount> accounts = model.GetResources<OnlineAccount>(true);

            return Response.AsJson(accounts);
        }

        private Response GetAccountProviders()
        {
            return Response.AsJson(OnlineAccountFactory.GetRegisteredProviders());
        }

        private Response GetAccountProvider(string providerId)
        {
            try
            {
                return Response.AsJson(OnlineAccountFactory.GetProvider(providerId));
            }
            catch(KeyNotFoundException)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        private Response GetOAuth2AccountRedirectUrl(string providerId)
        {
            OAuth2AccountProvider provider = OnlineAccountFactory.GetProvider(providerId) as OAuth2AccountProvider;

            if (provider != null)
            {
                string redirectUrl = string.Format("http://localhost:8262/artivity/api/1.0/accounts/oauth2/token?providerId={0}", providerId);

                return Response.AsRedirect(provider.GetAuthorizationRequestUrl(redirectUrl));
            }
            else
            {
                return HttpStatusCode.MethodNotAllowed;
            }
        }

        private Response SendOAuth2AccessToken(string providerId, string code)
        {
            OAuth2AccountProvider provider = OnlineAccountFactory.GetProvider(providerId) as OAuth2AccountProvider;

            if (provider != null)
            {
                provider.Authorize(ModelProvider.AgentsModel, code);

                return HttpStatusCode.Accepted;
            }
            else
            {
                return HttpStatusCode.MethodNotAllowed;
            }
        }

        private Response InstallAccount(string providerId)
        {
            return HttpStatusCode.NotImplemented;
        }

        private Response UninstallAccount(string accountId)
        {
            Person user = ModelProvider.AgentsModel.GetResources<Person>().FirstOrDefault();

            if (user == null)
            {
                return Logger.LogError(HttpStatusCode.InternalServerError, "Unable to retrieve user agent.");
            }

            OnlineAccount account = user.Accounts.FirstOrDefault(a => a.Id == accountId);

            if (account == null)
            {
                return Logger.LogInfo(HttpStatusCode.BadRequest, "Did not find account with id {0}", accountId);
            }

            ModelProvider.AgentsModel.DeleteResource(account);

            user.Accounts.Remove(account);
            user.Commit();

            return Logger.LogInfo(HttpStatusCode.OK, "Uninstalled account: {0}", accountId);
        }

        public Response GetUserAgentPhoto()
        {
            string file = Path.Combine(PlatformProvider.ArtivityUserDataFolder, "user.jpg");

            if (File.Exists(file))
            {
                FileStream fileStream = new FileStream(file, FileMode.Open);

                StreamResponse response = new StreamResponse(() => fileStream, MimeTypes.GetMimeType(file));

                return response.AsAttachment(file);
            }
            else
            {
                return null;
            }
        }

        public Response SetUserAgentPhoto(RequestStream stream)
        {
            string file = Path.Combine(PlatformProvider.ArtivityUserDataFolder, "user.jpg");

            try
            {
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
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);

                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }

        public Response GetRecentlyUsedFiles()
        {
            IModel model = ModelProvider.ActivitiesModel;

            ISparqlQuery query = new SparqlQuery(@"
                SELECT MAX(?time) as ?time ?entity ?url WHERE
                {
                    ?activity prov:used ?entity .
                    ?activity prov:startedAtTime ?time .

                    ?entity nfo:fileUrl ?url .
                }
                GROUP BY ?entity ?url ORDER BY DESC(?time) LIMIT 25");

            return Response.AsJson(model.GetBindings(query));
        }

        public Response GetActivities(string fileUrl)
        {
            IModel model = ModelProvider.GetAll();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?startTime ?endTime ?color ?name WHERE
                {
                    ?file nfo:fileUrl @fileUrl .

                    ?activity prov:used ?file ;
                        prov:qualifiedAssociation ?association ;
                        prov:startedAtTime ?startTime ;
                        prov:endedAtTime ?endTime .

                    ?association prov:agent ?agent .

                    ?agent foaf:name ?name .

                    OPTIONAL { ?agent art:hasColourCode ?color . }
                }
                ORDER BY DESC(?startTime)");

            query.Bind("@fileUrl", Uri.EscapeUriString(fileUrl));

            return Response.AsJson(model.GetBindings(query));
        }

        public Response RemoveAllActivities()
        {
            ModelProvider.ActivitiesModel.Clear();

            return HttpStatusCode.OK;
        }

        public Response GetInfluences(string fileUrl)
        {
            IModel model = ModelProvider.GetAllActivities();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?time ?uri ?type ?color ?description ?bounds ?thumbnailUrl ?x ?y WHERE 
                {
                    ?file nfo:fileUrl @fileUrl .

                    ?activity prov:used ?file;
                        prov:qualifiedAssociation ?association ;
                        prov:generated ?entity .

                    ?association prov:agent ?agent .

                    ?agent art:hasColourCode ?color .

                    ?entity prov:qualifiedGeneration ?generation .

                    ?generation a ?type ;
                        prov:atTime ?time .

                    OPTIONAL { ?generation dces:description ?description . }
                    OPTIONAL { ?generation art:hadBoundaries ?bounds . }
                    OPTIONAL
                    {
                        ?generation art:thumbnailUrl ?thumbnailUrl .
                        ?generation art:thumbnailPosition ?rectangle .

                        ?rectangle art:position ?position .

                        ?position art:x ?x .
                        ?position art:y ?y .
                    }
                }
                ORDER BY DESC(?time)");

            query.Bind("@fileUrl", Uri.EscapeUriString(fileUrl));

            return Response.AsJson(model.GetBindings(query));
        }

        private Response GetThumbnails(string fileUrl)
        {
            IModel model = ModelProvider.ActivitiesModel;

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?layer ?time ?thumbnailUrl ?x ?y WHERE 
                {
                    {
                        SELECT ?layer (SAMPLE(?generation) AS ?generation) (MAX(?time) AS ?time) WHERE
                        {
                            ?file nfo:fileUrl @fileUrl .

                            ?activity prov:used ?file .
                            ?activity prov:generated ?entity .

                            ?entity prov:qualifiedGeneration ?generation .

                            ?generation art:selectedLayer ?layer .
                            ?generation prov:atTime ?time .
                        }
                        GROUP BY ?layer
                    }

                    ?generation art:thumbnailUrl ?thumbnailUrl .
                    ?generation art:thumbnailPosition ?rectangle .

                    ?rectangle art:position ?position .

                    ?position art:x ?x .
                    ?position art:y ?y .
                }
                ORDER BY DESC(?layer)");

            query.Bind("@fileUrl", Uri.EscapeUriString(fileUrl));

            return Response.AsJson(model.GetBindings(query));
        }

        private Response GetThumbnails(string fileUrl, DateTime timestamp)
        {
            IModel model = ModelProvider.ActivitiesModel;

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?layer ?time ?generation ?thumbnailUrl ?x ?y WHERE 
                {
                    {
                        SELECT ?layer MAX(?time) AS ?time WHERE
                        {
                            ?file nfo:fileUrl @fileUrl .

                            ?activity prov:used ?file .
                            ?activity prov:generated ?entity .

                            ?entity prov:qualifiedGeneration ?generation .

                            ?generation art:selectedLayer ?layer .
                            ?generation prov:atTime ?time .

                            FILTER(?time <= @time)
                        }
                        GROUP BY ?layer
                    }

                    ?generation prov:atTime ?time .
                    ?generation art:selectedLayer ?layer .
                    ?generation art:thumbnailUrl ?thumbnailUrl .
                    ?generation art:thumbnailPosition ?rectangle .

                    ?rectangle art:position ?position .

                    ?position art:x ?x .
                    ?position art:y ?y .
                }");

            query.Bind("@fileUrl", Uri.EscapeUriString(fileUrl));
            query.Bind("@time", timestamp);

            return Response.AsJson(model.GetBindings(query));
        }

        private Response GetThumbnail(string thumbnailUrl)
        {
            string file = new Uri(thumbnailUrl).AbsolutePath;

            if (File.Exists(file))
            {
                FileStream fileStream = new FileStream(file, FileMode.Open);

                StreamResponse response = new StreamResponse(() => fileStream, MimeTypes.GetMimeType(file));

                return response.AsAttachment(file);
            }
            else
            {
                return null;
            }
        }

        private Response GetThumbnailPaths(string filePath)
        {
            string dirName = GetFileUri(filePath);

            if (string.IsNullOrEmpty(dirName))
            {
                return HttpStatusCode.InternalServerError;
            }

            var invalids = System.IO.Path.GetInvalidFileNameChars();
            var newName = String.Join("_", dirName.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');

            DirectoryInfo dir = new DirectoryInfo(Path.Combine(PlatformProvider.ThumbnailFolder, newName));

            if (!dir.Exists)
            {
                dir.Create();
            }

            List<string> result = new List<string>()
            {
                dir.FullName
            };

            return Response.AsJson(result);
        }

        private string GetFileUri(string filePath)
        {
            Uri fileUrl = new FileInfo(filePath).ToUriRef();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT ?entity WHERE
                {
                    ?activity prov:startedAtTime ?startTime .
                    ?activity prov:used ?entity .

                    ?entity nfo:fileUrl @fileUrl .
                }
                ORDER BY DESC(?startTime) LIMIT 1
            ");

            query.Bind("@fileUrl", fileUrl.AbsoluteUri);

            ISparqlQueryResult result = ModelProvider.ActivitiesModel.ExecuteQuery(query);

            if (result.GetBindings().Any())
            {
                BindingSet binding = result.GetBindings().First();

                string uri = binding["entity"].ToString();

                if (!string.IsNullOrEmpty(uri))
                {
                    return uri;
                }
            }

            return null;
        }

        #endregion
    }
}
