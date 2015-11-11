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

using System;
using Semiodesk.Trinity;

namespace Artivity.DataModel
{
	public static class Models
	{
        #region Members

        public static readonly string DefaultStore = "virt0";

        public static readonly Uri Agents;

        public static readonly Uri Activities;

        public static readonly Uri WebActivities;

        #endregion

        static Models()
        {
            string username = Environment.UserName;

            Agents = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/agents", username));
            Activities = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/activities", username));
            WebActivities = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/activities/web", username));
        }

        #region Methods

        private static IModel GetModel(IStore store, Uri uri)
        {
            if(store == null)
            {
                store = StoreFactory.CreateStoreFromConfiguration(DefaultStore);
            }

            return store.GetModel(uri);
        }

        public static IModelGroup GetAll()
        {
            IStore store = StoreFactory.CreateStoreFromConfiguration(DefaultStore);

            IModelGroup result = store.CreateModelGroup();
            result.Add(GetAgents(store));
            result.Add(GetActivities(store));
            result.Add(GetWebActivities(store));

            return result;
        }

        public static IModel GetAllActivities()
        {
            IStore store = StoreFactory.CreateStoreFromConfiguration(DefaultStore);

            IModelGroup result = store.CreateModelGroup();
            result.Add(GetActivities(store));
            result.Add(GetWebActivities(store));

            return result;
        }

        public static IModel GetAgents(IStore store = null)
        {
            return GetModel(store, Agents);
        }

        public static IModel GetActivities(IStore store = null)
        {
            return GetModel(store, Activities);
        }

        public static IModel GetWebActivities(IStore store = null)
        {
            return GetModel(store, WebActivities);
        }

        #endregion
	}
}
