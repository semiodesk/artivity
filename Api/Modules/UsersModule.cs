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
using Artivity.Api.IO;

namespace Artivity.Api.Modules
{
    public class UsersModule : ModuleBase
    {
        #region Constructors

        public UsersModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/agents/users", modelProvider, platformProvider)
        {
            Get["/"] = parameters =>
            {
                InitializeRequest();

                if(Request.Query.Count == 0)
                {
                    return GetPersons();
                }
                else if(IsUri(Request.Query.agentUri))
                {
                    UriRef agentUri = new UriRef(Request.Query.agentUri);

                    return GetPersonFromUri(agentUri);
                }
                else if (!string.IsNullOrEmpty(Request.Query.role))
                {
                    UserRoles role;

                    if (Enum.TryParse(Request.Query.role, out role))
                    {
                        return GetPersonsFromRole(role);
                    }
                    else
                    {
                        return HttpStatusCode.BadRequest;
                    }
                }
                else if(!string.IsNullOrEmpty(Request.Query.q))
                {
                    return GetPersonsFromQuery(Request.Query.q);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Put["/"] = parameters =>
            {
                InitializeRequest();

                return PutPerson(Request.Body);
            };

            Delete["/"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.agentUri))
                {
                    UriRef agentUri = new UriRef(Request.Query.agentUri);

                    return DeletePerson(agentUri);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Get["/new"] = parameters =>
            {
                InitializeRequest();

                return CreatePerson();
            };

            Get["/photo"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.agentUri))
                {
                    UriRef agentUri = new UriRef(Request.Query.agentUri);

                    return GetPersonPhoto(agentUri);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Put["/photo"] = parameters =>
            {
                InitializeRequest();

                if (IsUri(Request.Query.agentUri))
                {
                    UriRef agentUri = new UriRef(Request.Query.agentUri);

                    RequestStream stream = Request.Body;

                    return PutPersonPhoto(agentUri, stream);
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };
        }

        #endregion

        #region Methods

        private Response GetPersons()
        {
            List<Person> persons = ModelProvider.GetAgents().GetResources<Person>(true).ToList();

            return Response.AsJsonSync(persons);
        }

        private Response GetPersonFromUri(Uri agentUri)
        {
            Person person = ModelProvider.GetAgents().GetResource<Person>(agentUri);

            return Response.AsJsonSync(person);
        }

        private Response GetPersonsFromRole(UserRoles userRole)
        {
            IModel Model = ModelProvider.GetAgents();

            if (Model == null)
            {
                return HttpStatusCode.BadRequest;
            }

            Resource role;

            switch(userRole)
            {
                case UserRoles.AccountOwnerRole: role = art.AccountOwnerRole; break;
                case UserRoles.ProjectAdministratorRole: role = art.ProjectAdministratorRole; break;
                case UserRoles.ProjectMemberRole: role = art.ProjectMemberRole; break;
                default: throw new ArgumentException("userRole");
            }

            SparqlQuery query = new SparqlQuery(@"
                SELECT ?s ?p ?o WHERE
                {
                    ?s ?p ?o .

                    ?association prov:agent ?s; prov:hadRole @role .
                }
            ");

            query.Bind("@role", role);

            List<Person> persons = Model.GetResources<Person>(query, true).ToList();

            return Response.AsJsonSync(persons);
        }

        private Response GetPersonsFromQuery(string q)
        {
            IModel agents = ModelProvider.GetAgents();

            ISparqlQuery query = new SparqlQuery(@"
                SELECT ?s ?p ?o WHERE
                {
                    ?s ?p ?o .
                    ?s foaf:name ?name .
                    ?s foaf:mbox ?email .

                    FILTER(STRSTARTS(LCASE(?name), @q) || STRSTARTS(LCASE(?email), @q))
                }");

            query.Bind("@q", q.ToLowerInvariant());

            List<Person> persons = agents.GetResources<Person>(query).ToList();

            return Response.AsJsonSync(persons);
        }

        private Response DeletePerson(Uri agentUri)
        {
            IModel agents = ModelProvider.GetAgents();

            if(agents.ContainsResource(agentUri))
            {
                agents.DeleteResource(agentUri);

                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.NotModified;
            }
        }

        private Response CreatePerson()
        {
            Person person = ModelProvider.GetAgents().CreateResource<Person>(ModelProvider.CreateUri<Person>());

            return Response.AsJsonSync(person);
        }

        private Response PutPerson(RequestStream stream)
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

            if (!string.IsNullOrEmpty(user.Name) && !string.IsNullOrEmpty(user.EmailAddress))
            {
                user.Commit();

                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
        }

        private Response GetPersonPhoto(Uri agentUri)
        {
            try
            {
                string uri = FileNameEncoder.Encode(agentUri.AbsoluteUri);
                string file = Path.Combine(PlatformProvider.AvatarsFolder, uri + ".jpg");

                if (!File.Exists(file))
                {
                    return HttpStatusCode.NoContent;
                }

                FileStream fileStream = new FileStream(file, FileMode.Open);

                StreamResponse response = new StreamResponse(() => fileStream, MimeTypes.GetMimeType(file));
                response.Headers["Allow-Control-Allow-Origin"] = "127.0.0.1";

                return response.AsAttachment(file);
            }
            catch (IOException ex)
            {
                PlatformProvider.Logger.LogError(ex);

                return HttpStatusCode.InternalServerError;
            }
        }

        private Response PutPersonPhoto(Uri agentUri, RequestStream stream)
        {
            try
            {
                string uri = FileNameEncoder.Encode(agentUri.AbsoluteUri);
                string file = Path.Combine(PlatformProvider.AvatarsFolder, uri + ".jpg");

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

        #endregion
    }
}
