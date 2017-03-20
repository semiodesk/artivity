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
using System.Text;

namespace Artivity.Api.Modules
{
    public abstract class EntityModuleBase<T> : ModuleBase where T : Resource
    {
        #region Members

        private bool _create;

        private bool _retrieve;

        private bool _update;

        private bool _delete;

        private bool _initQueryCalled = false;

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

        public EntityModuleBase(IModelProvider modelProvider, IPlatformProvider platformProvider, bool create = true, bool retrieve = true, bool update = true, bool delete = true)
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

        private static string GetTypename()
        {
            return typeof(T).Name;
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
                Get["/"] = parameters =>
                {
                    return GetEntity();
                };
            }

            if (_create)
            {
                // Create a new resource
                Get["/new"] = parameters =>
                {
                    if (PlatformProvider.RequiresAuthentication)
                    {
                        this.RequiresAuthentication();
                    }

                    LoadCurrentUser();

                    if (UserModel == null)
                    {
                        return HttpStatusCode.InternalServerError;
                    }

                    Uri uri = CreateUri();

                    T entity = UserModel.CreateResource<T>(uri);

                    OnEntityCreated(entity);

                    return Response.AsJsonSync(entity);
                };
            }

            if (_update)
            {
                // Update entity. Check if user should be able to do that
                Put["/"] = parameters =>
                {
                    if (PlatformProvider.RequiresAuthentication)
                    {
                        this.RequiresAuthentication();
                    }

                    LoadCurrentUser();

                    if (UserModel == null)
                    {
                        return HttpStatusCode.InternalServerError;
                    }

                    Uri uri = new Uri(Request.Query["uri"]);

                    OnBeforeEntityUpdated(uri);

                    T entity = Bind<T>(ModelProvider.Store, Request.Body);
                    entity.Commit();

                    OnEntityUpdated(entity);

                    return Response.AsJsonSync(new { success = true });
                };
            }

            if (_delete)
            {
                Delete["/"] = parameters =>
                {
                    if (PlatformProvider.RequiresAuthentication)
                    {
                        this.RequiresAuthentication();
                    }

                    LoadCurrentUser();

                    Uri uri = new Uri(Request.Query["uri"]);

                    if (UserModel == null)
                    {
                        return HttpStatusCode.InternalServerError;
                    }

                    OnBeforeEntityDeleted(uri);

                    IResource resource = UserModel.GetResource(uri);
                    resource.AddProperty(art.deleted, DateTime.UtcNow);
                    resource.Commit();

                    return Response.AsJsonSync(new { success = true });
                };
            }
        }

        public virtual void OnEntityCreated(T entity) { }

        public virtual void OnBeforeEntityUpdated(Uri uri) { }

        public virtual void OnEntityUpdated(T entity) { }

        public virtual void OnBeforeEntityDeleted(Uri uri) { }

        /// <summary>
        /// This method initializes the Authentication and loads the current user.
        /// If you overload it, return null if you want to continue processing the request.
        /// </summary>
        /// <returns></returns>
        protected virtual Response InitQuery()
        {
            if (PlatformProvider.RequiresAuthentication)
            {
                this.RequiresAuthentication();
            }

            LoadCurrentUser();

            if (UserModel == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            _initQueryCalled = true;

            return null;
        }

        /// <summary>
        /// Processes the regular get function for this endpoint.
        /// Takes an 'uri' parameter if you want a specific entity.
        /// Returns a list of entities if no parameter was specified.
        /// </summary>
        /// <returns></returns>
        protected virtual Response GetEntity()
        {
            if( !_initQueryCalled )
            {
                var res = InitQuery();
                if (res != null)
                    return res;
            }

            int offset = -1;
            int limit = -1;
            GetOffsetLimit(out offset, out limit);

            if (Request.Query["uri"] != null && !string.IsNullOrEmpty(Request.Query["uri"]))
            {
                Uri uri = new Uri(Request.Query["uri"]);

                T entity = UserModel.GetResource<T>(uri);

                return Response.AsJsonSync(entity);
            }
            else
            {
                T t = (T)Activator.CreateInstance(typeof(T), new Uri("http://localhost"));

                ResourceQuery query = new ResourceQuery(t.GetTypes());
                query.Where(art.deleted, DateTime.MinValue);
 
                var queryResult = UserModel.ExecuteQuery(query);
                int count = queryResult.Count();

                List<T> list = queryResult.GetResources<T>(offset, limit).ToList();

                return Response.AsJsonSync(new Dictionary<string, object> { { "success", true }, { "data", list } });
            }
        }

        protected void GetOffsetLimit(out int offset, out int limit)
        {
            offset = 0;
            limit = 10;
            if (Request.Query["offset"])
                offset = (int)Request.Query["offset"];
            if (Request.Query["limit"])
                limit = (int)Request.Query["limit"];
        }
        #endregion
    }
}