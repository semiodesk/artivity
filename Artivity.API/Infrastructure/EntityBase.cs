using Artivity.Apid.Platforms;
using Artivity.DataModel;
using Nancy;
using Nancy.Security;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Artivity.Apid
{

    public abstract class EntityBase<T> : ModuleBase where T : Resource
    {


        #region Constructors
        public EntityBase(string path, IModelProvider modelProvider, IPlatformProvider platformProvider, bool create = true, bool retrieve = true, bool update = true, bool delete = true)
            : base(path, modelProvider, platformProvider)
        {
            _create = create;
            _retrieve = retrieve;
            _update = update;
            _delete = delete;
            Initialize();
        }

        public EntityBase(ModelProvider modelProvider, IPlatformProvider platformProvider , bool create = true, bool retrieve = true, bool update = true, bool delete = true)
            : base(GetTypename(), modelProvider, platformProvider)
        {
            _create = create;
            _retrieve = retrieve;
            _update = update;
            _delete = delete;
            Initialize();
        }
        #endregion

        #region Member
        bool _create;
        bool _retrieve;
        bool _update;
        bool _delete;
        #endregion

        #region Methods

        public virtual void EntityCreated(T entity)
        { }

        public virtual void EntityAboutToBeUpdated(Uri uri)
        { }

        public virtual void EntityUpdated(T entity)
        { }

        public virtual void EntityAboutToBeDeleted(Uri uri)
        { }

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
                    if( PlatformProvider.RequiresAuthentication )
                        this.RequiresAuthentication();
                    LoadCurrentUser();

                    if (UserModel == null)
                        return Response.AsJson("", HttpStatusCode.InternalServerError);

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
                        this.RequiresAuthentication();
                    LoadCurrentUser();

                    if (UserModel == null)
                        return Response.AsJson("", HttpStatusCode.InternalServerError);

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
                        this.RequiresAuthentication();
                    LoadCurrentUser();

                    if (UserModel == null)
                        return Response.AsJsonSync("", HttpStatusCode.InternalServerError);

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
                        this.RequiresAuthentication();
                    LoadCurrentUser();

                    Uri uri = new Uri(Request.Query["uri"]);

                    if (UserModel == null)
                        return Response.AsJsonSync("", HttpStatusCode.InternalServerError);

                    EntityAboutToBeDeleted(uri);
                    UserModel.DeleteResource(uri);
                    return Response.AsJsonSync(new Dictionary<string, object> { { "success", true } });
                };
            }
        }


        #endregion


    }
}
