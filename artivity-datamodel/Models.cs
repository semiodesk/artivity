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
	public class ModelProviderFactory
	{
        public static ModelProvider CreateModelProvider(string connectionString, string nativeConectionString, string username=null)
        {
            ModelProvider p = new ModelProvider();
            p.ConnectionString = connectionString;
            p.NativeConnectionString = nativeConectionString;
            p.Username = username;
            p.InitializeStore();
            return p;
        }
	}

    public class ModelProvider : Artivity.DataModel.IModelProvider
    {
        #region Members

        private IStore _store = null;

        public string ConnectionString { get; set; }

        public string NativeConnectionString { get; set; }

        public Uri Agents { get; set; }
        public IModel AgentsModel { get; set; }

        public Uri Activities { get; set; }
        public IModel ActivitiesModel { get; set; }

        public Uri WebActivities { get; set; }
        public IModel WebActivitiesModel { get; set; }

        public Uri Monitoring { get; set; }
        public IModel MonitoringModel { get; set; }

        public string Username { get; set; }

        private bool UrisLoaded = false;

        #endregion

        #region Constructor
        internal ModelProvider()
        { }
        #endregion


        #region Methods
        void LoadModelUris()
        {
            if (string.IsNullOrEmpty(Username))
                Username = Environment.UserName;

            Agents = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/agents", Username));
            Activities = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/activities", Username));
            WebActivities = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/activities/web", Username));
            Monitoring = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/monitoring", Username));
            UrisLoaded = true;
        }

        public void InitializeStore()
        {
            if (!UrisLoaded)
                LoadModelUris();

            _store = StoreFactory.CreateStore(ConnectionString);
            AgentsModel = _store.GetModel(Agents);
            ActivitiesModel = _store.GetModel(Activities);
            WebActivitiesModel = _store.GetModel(WebActivities);
            MonitoringModel = _store.GetModel(Monitoring);
        }

        public bool Exists(Uri uri)
        {
            return _store.ContainsModel(uri);
        }

        public IModelGroup GetAll()
        {
            IModelGroup result = _store.CreateModelGroup();
            result.Add(GetAgents(_store));
            result.Add(GetActivities(_store));
            result.Add(GetWebActivities(_store));
            result.Add(GetMonitoring());

            return result;
        }

        public IModel GetAllActivities()
        {
            IModelGroup result = _store.CreateModelGroup();
            result.Add(GetAgents(_store));
            result.Add(GetActivities(_store));
            result.Add(GetWebActivities(_store));

            return result;
        }

        public IModel GetAgents(IStore store = null)
        {
            return AgentsModel;
        }

        public IModel GetActivities(IStore store = null)
        {
            return ActivitiesModel;
        }

        public IModel GetWebActivities(IStore store = null)
        {
            return WebActivitiesModel;
        }

        public IModel GetMonitoring(IStore store = null)
        {
            return MonitoringModel;
        }

        #endregion
    }
}
