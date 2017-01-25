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

using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Repositories
{
    public class ResourceRepository<T> : IDisposable where T : Resource
    {
        #region Members

        protected IModel Model;

        protected bool EnableInferencing = false;

        #endregion

        #region Constructors

        public ResourceRepository(IModel model)
        {
            Model = model;
        }

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

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
