using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Api.Http.Repositories
{
    public class ResourceRepository<T> : IDisposable where T : Resource
    {
        #region Members
        protected IModel Model;
        protected bool EnableInferencing = false;
        #endregion

        #region Constructor
        public ResourceRepository(IModel model)
        {
            Model = model;
        }
        #endregion

        #region Destructor
        ~ResourceRepository()
        {
            Dispose(false);
        }
        #endregion

        #region Methods

        public T AddNew(Uri uri)
        {
            if (!Model.ContainsResource(uri))
            {
                return Model.CreateResource<T>(uri);
            }
            else
                return null;
        }

        public T Add(T item)
        {
            if (!Model.ContainsResource(item.Uri))
            {
                return Model.AddResource<T>(item);
            }
            else
                return Model.GetResource<T>(item.Uri);
        }

        public IEnumerable<T> List()
        {
            return Model.GetResources<T>(EnableInferencing);
        }

        public void Remove(T item)
        {
            Model.DeleteResource(item.Uri);
        }

        public void Update(T item)
        {
            item.Commit();
        }

        public T FindByUri(Uri u)
        {
            if (Model.ContainsResource(u))
                return Model.GetResource<T>(u);

            return null;
        }

        public void Clear()
        {
            Model.Clear();
        }

        public void Dispose(bool safeToFreeManagedObject)
        {
            if (safeToFreeManagedObject)
            {

            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
