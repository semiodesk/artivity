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
using Artivity.Api.Platform;
using Artivity.DataModel;
using Nancy;
using Nancy.Security;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Artivity.Api.Modules
{
    public abstract class EntityModuleBase<T> : ModuleBase where T : Resource
    {
        #region Members

        private bool _create;
        private bool _retrieve;
        private bool _update;
        private bool _delete;

        #endregion

        #region Constructors

        public EntityModuleBase(string path, IModelProvider modelProvider, IPlatformProvider platformProvider, bool create = true, bool retrieve = true, bool update = true, bool delete = true)
            : base(path, modelProvider, platformProvider)
        {
            _create = create;
            _retrieve = retrieve;
            _update = update;
            _delete = delete;
            Initialize();
        }

        public EntityModuleBase(ModelProvider modelProvider, IPlatformProvider platformProvider , bool create = true, bool retrieve = true, bool update = true, bool delete = true)
            : base(GetTypename(), modelProvider, platformProvider)
        {
            _create = create;
            _retrieve = retrieve;
            _update = update;
            _delete = delete;
            Initialize();
        }

        #endregion

        #region Methods

        public virtual void EntityCreated(T entity)
        {}

        public virtual void EntityAboutToBeUpdated(Uri uri)
        {}

        public virtual void EntityUpdated(T entity)
        {}

        public virtual void EntityAboutToBeDeleted(Uri uri)
        {}

        private static string GetTypename()
        {
            Type t = typeof(T);
            return t.Name;
        }

        protected virtual Uri CreateUri(string guid)
        {
            return new Uri(string.Format("http://artivity.io/{0}/{1}", typeof(T).Name, guid));
        }

        protected virtual Uri CreateUri()
        {
            return new Uri(string.Format("http://artivity.io/{0}/{1}", typeof(T).Name, Guid.NewGuid().ToString()));
        }

        private void Initialize()
        {
            if (_retrieve)
            {
                Get["/"] = Parameters =>
                {
                    if (PlatformProvider.RequiresAuthentication)
                    {
                        this.RequiresAuthentication();
                    }

                    LoadCurrentUser();

                    if (UserModel == null)
                    {
                        return Response.AsJsonSync("", HttpStatusCode.InternalServerError);
                    }

                    if (Request.Query["uri"] != null)
                    {
                        Uri uri = new Uri(Request.Query["uri"]);

                        T entity = UserModel.GetResource<T>(uri);
                        var resp = Response.AsJsonSync(entity);
                        return resp;
                    }
                    else
                    {
                        var list = UserModel.GetResources<T>().ToList();
                        var resp = Response.AsJsonSync(list);
                        return resp;
                    }
                };
            }

            if (_create)
            {
                // Create a new resource
                Get["/new"] = Parameters =>
                {
                    if (PlatformProvider.RequiresAuthentication)
                    {
                        this.RequiresAuthentication();
                    }

                    LoadCurrentUser();

                    if (UserModel == null)
                    {
                        return Response.AsJsonSync("", HttpStatusCode.InternalServerError);
                    }

                    Uri uri = CreateUri();
                    T entity = UserModel.CreateResource<T>(uri);
                    EntityCreated(entity);
                    var resp = Response.AsJsonSync(entity);
                    return resp;
                };
            }

            if (_update)
            {
                // Update entity. Check if user should be able to do that
                Put["/"] = Parameters =>
                {
                    if (PlatformProvider.RequiresAuthentication)
                    {
                        this.RequiresAuthentication();
                    }

                    LoadCurrentUser();

                    if (UserModel == null)
                    {
                        return Response.AsJsonSync("", HttpStatusCode.InternalServerError);
                    }

                    Uri uri = new Uri(Request.Query["uri"]);
                    T entity;

                    EntityAboutToBeUpdated(uri);
                    entity = Bind<T>(ModelProvider.Store, Request.Body);
                    
                    // Test if user is allowed to write into model
                    entity.Commit();
                    EntityUpdated(entity);
                    var resp = Response.AsJsonSync(new Dictionary<string, object> { { "success", true } });
                    return resp;
                };
            }

            if (_delete)
            {
                Delete["/"] = Parameters =>
                {
                    if (PlatformProvider.RequiresAuthentication)
                    {
                        this.RequiresAuthentication();
                    }

                    LoadCurrentUser();

                    Uri uri = new Uri(Request.Query["uri"]);

                    if (UserModel == null)
                    {
                        return Response.AsJsonSync("", HttpStatusCode.InternalServerError);
                    }

                    EntityAboutToBeDeleted(uri);
                    UserModel.DeleteResource(uri);

                    return Response.AsJsonSync(new Dictionary<string, object> { { "success", true } });
                };
            }
        }

        #endregion
    }
}
