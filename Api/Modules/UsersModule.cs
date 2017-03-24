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

namespace Artivity.Apid.Modules
{
    public class UsersModule : ModuleBase
    {
        #region Constructors

        public UsersModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/agents/users", modelProvider, platformProvider)
        {
            Get["/"] = parameters =>
            {
                if(Request.Query.Count == 0)
                {
                    return GetUserAgents();
                }
                else if(IsUri(Request.Query.agentUri))
                {
                    UriRef agentUri = new UriRef(Request.Query.agentUri);

                    return GetUserAgent(agentUri);
                }
                //else if (!string.IsNullOrEmpty(Request.Query.type))
                //{
                //    string typeUri = Request.Query.typeUri;

                //    return GetUserAgentsFromType(type);
                //}
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };

            Post["/"] = parameters =>
            {
                return PostUserAgent(Request.Body);
            };

            Get["/photo"] = parameters =>
            {
                return GetUserAgentPhoto();
            };

            Post["/photo"] = parameters =>
            {
                RequestStream stream = Request.Body;

                return PostUserAgentPhoto(stream);
            };
        }

        #endregion

        #region Methods

        private Response GetUserAgents()
        {
            IModel model = ModelProvider.GetAgents();

            return HttpStatusCode.NotImplemented;
        }

        private Response GetUserAgent(Uri agentUri)
        {
            UriRef uid = new UriRef(PlatformProvider.Config.Uid);

            IModel model = ModelProvider.GetAgents();

            if (model.ContainsResource(uid))
            {
                Person user = model.GetResource<Person>(uid);

                return Response.AsJsonSync(user);
            }
            else
            {
                Association association = CreateUserAssociation(uid);

                return Response.AsJsonSync(association.Agent);
            }
        }

        private Response GetUserAgentsFromType(string type)
        {
            return HttpStatusCode.NotImplemented;
        }

        private Association CreateUserAssociation(UriRef uid)
        {
            // See if there is already a person defined..
            Person user;

            IModel model = ModelProvider.GetAgents();

            if (!model.ContainsResource(uid))
            {
                PlatformProvider.Logger.LogInfo("Creating new user profile..");

                // If not, create one.
                user = model.CreateResource<Person>(uid);
                user.Commit();
            }
            else
            {
                PlatformProvider.Logger.LogInfo("Upgrading user profile..");

                user = model.GetResource<Person>(uid);
            }

            // Add the user role association.
            Association association = model.CreateResource<Association>();
            association.Agent = user;
            association.Role = new Role(art.AccountOwnerRole);
            association.Commit();

            return association;
        }

        private Response PostUserAgent(RequestStream stream)
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
            catch (IOException ex)
            {
                PlatformProvider.Logger.LogError(ex);

                return HttpStatusCode.InternalServerError;
            }
        }

        private Response PostUserAgentPhoto(RequestStream stream)
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

        #endregion
    }

    enum UserRoles { AccountOwner, Contact };
}
