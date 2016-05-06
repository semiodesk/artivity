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

namespace Artivity.Apid.Modules
{
    public class ApiModule : ModuleBase
    {
        #region Constructors

        public ApiModule(IModelProvider provider)
            : base("/artivity/api/1.0", provider)
        {
            ModelProvider = provider;

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

                return GetOAuth2AccountRedirectUrl(providerId);
            };

            Get["/accounts/oauth2/token"] = parameters =>
            {
                string providerId = Request.Query["providerId"];
                string code = Request.Query["code"];

                return SendOAuth2AccessToken(providerId, code);
            };

            Get["/accounts/install"] = parameters =>
            {
                string providerId = Request.Query["providerId"];

                return InstallAccount(providerId);
            };

            // Uninstall a account with a specific id.
            Get["/accounts/uninstall"] = parameters =>
            {
                string accountId = Request.Query["accountId"];

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
                string fileUrl = Request.Query["fileUrl"];

                return GetActivities(fileUrl);
            };

            Get["/influences"] = parameters =>
            {
                string fileUrl = Request.Query["fileUrl"];

                return GetInfluences(fileUrl);
            };

            Get["/thumbnails"] = parameters =>
            {
                string fileUrl = Request.Query["fileUrl"];

                return GetThumbnails(fileUrl);
            };

            Get["/thumbnails/paths"] = parameters =>
            {
                string fileUrl = Request.Query["fileUrl"];
                return GetThumbnailPaths(fileUrl);
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
            string file = Path.Combine(Platform.GetAppDataFolder(), "user.jpg");

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
            string file = Path.Combine(Platform.GetAppDataFolder(), "user.jpg");

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
                SELECT MAX(?startTime) as ?time ?uri ?url ?agent WHERE
                {
                    ?activity prov:used ?uri ;
                        prov:startedAtTime ?startTime ;
                        prov:qualifiedAssociation ?association .

                    ?association prov:agent ?agent .

                    ?uri nfo:fileUrl ?url .
                }
                ORDER BY DESC(?time) LIMIT 25");

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

                    ?agent foaf:name ?name ;
                        art:hasColourCode ?color .
                }
                ORDER BY DESC(?startTime)");

            query.Bind("@fileUrl", Uri.EscapeUriString(fileUrl));

            return Response.AsJson(model.GetBindings(query));
        }

        public Response GetInfluences(string fileUrl)
        {
            IModel model = ModelProvider.GetAllActivities();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?time ?uri ?type ?color ?description ?bounds WHERE 
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
                }
                ORDER BY (?time)");

            query.Bind("@fileUrl", Uri.EscapeUriString(fileUrl));

            return Response.AsJson(model.GetBindings(query));
        }

        private Response GetThumbnails(string fileUrl)
        {
            IModel model = ModelProvider.ActivitiesModel;

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?thumbnailUrl WHERE 
                {
                    ?file nfo:fileUrl @fileUrl .

                    ?activity prov:used ?file;
                        prov:generated ?entity .

                    ?entity prov:qualifiedGeneration ?generation .

                    ?generation art:thumbnailUrl ?thumbnailUrl .
                }
                ORDER BY (?time)");

            return Response.AsJson(model.GetBindings(query));
        }

        private Uri GetUri(string path)
        {
            FileInfo f = new FileInfo(path);
            return f.ToUriRef();
        }

        private string GetFileUri(string path)
        {
            Uri url = GetUri(path);

            string queryString = @"
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>

                SELECT DISTINCT ?uri WHERE { ?v prov:specializationOf ?uri . ?uri nfo:fileUrl """ + url.AbsoluteUri + "\" . } LIMIT 1";

            IModel model = ModelProvider.GetActivities();

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            if (result.GetBindings().Any())
            {
                BindingSet binding = result.GetBindings().First();

                string uri = binding["uri"].ToString();

                if (!string.IsNullOrEmpty(uri))
                {
                   return uri;
                }
            }
            return null;
        }

        private Response GetThumbnailPaths(string path)
        {

            string dirName = GetFileUri(path);
            var invalids = System.IO.Path.GetInvalidFileNameChars();
            var newName = String.Join("_", dirName.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');

            DirectoryInfo dir = new DirectoryInfo(Path.Combine(Platform.GetAppDataFolder("Thumbnails"), newName));
            if (!dir.Exists)
                dir.Create();

            List<string> result = new List<string>()
            {
                dir.FullName
            };

            return Response.AsJson(result);
        }

        #endregion
    }
}
