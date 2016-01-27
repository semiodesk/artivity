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

        private static IStore _store = null;

        public static string ConnectionString;

        public static string NativeConnectionString;

        public static Uri Agents;
        public static IModel AgentsModel;

        public static Uri Activities;
        public static IModel ActivitiesModel;

        public static Uri WebActivities;
        public static IModel WebActivitiesModel;

        public static Uri Monitoring;
        public static IModel MonitoringModel;

        #endregion

        static Models()
        {
            string username = Environment.UserName;

            Agents = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/agents", username));
            Activities = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/activities", username));
            WebActivities = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/activities/web", username));
            Monitoring = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/monitoring", username));
        }

        #region Methods

        public static void InitializeStore()
        {
            _store = StoreFactory.CreateStore(Models.ConnectionString);
            AgentsModel = _store.GetModel(Agents);
            ActivitiesModel = _store.GetModel(Activities);
            WebActivitiesModel = _store.GetModel(WebActivities);
            MonitoringModel = _store.GetModel(Monitoring);
        }

        public static bool Exists(Uri uri)
        {
            return _store.ContainsModel(uri);
        }

        public static IModelGroup GetAll()
        {
            IModelGroup result = _store.CreateModelGroup();
            result.Add(GetAgents(_store));
            result.Add(GetActivities(_store));
            result.Add(GetWebActivities(_store));
            result.Add(GetMonitoring());

            return result;
        }

        public static IModel GetAllActivities()
        {
            IModelGroup result = _store.CreateModelGroup();
            result.Add(GetAgents(_store));
            result.Add(GetActivities(_store));
            result.Add(GetWebActivities(_store));

            return result;
        }

        public static IModel GetAgents(IStore store = null)
        {
            return AgentsModel;
        }

        public static IModel GetActivities(IStore store = null)
        {
            return ActivitiesModel;
        }

        public static IModel GetWebActivities(IStore store = null)
        {
            return WebActivitiesModel;
        }

        public static IModel GetMonitoring(IStore store = null)
        {
            return MonitoringModel;
        }

        #endregion
	}
}
