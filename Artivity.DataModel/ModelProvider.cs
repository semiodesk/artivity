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
using System.Collections.Generic;
using System.Threading;

namespace Artivity.DataModel
{
    public class ModelProvider : Artivity.DataModel.IModelProvider
    {
        #region Members

        private Dictionary<int, IStore> _stores = new Dictionary<int, IStore>();

        public IStore Store 
        {
            get
            {
                int id = Thread.CurrentThread.ManagedThreadId;

                if (_stores.ContainsKey(id))
                {
                    return _stores[id];
                }
                else
                {
                    var store = StoreFactory.CreateStore(ConnectionString);

                    _stores[id] = store;

                    return store;
                }
            }
        }

        public string ConnectionString { get; set; }

        public string NativeConnectionString { get; set; }

        public Uri Agents { get; set; }

        public IModel AgentsModel { get { return Store.GetModel(Agents); } }

        public Uri Activities { get; set; }

        public IModel ActivitiesModel { get { return Store.GetModel(Activities); } }

        public Uri WebActivities { get; set; }

        public IModel WebActivitiesModel { get { return Store.GetModel(WebActivities); } }

        public Uri Monitoring { get; set; }

        public IModel MonitoringModel { get { return Store.GetModel(Monitoring); } }

        public string Username { get; set; }

        private bool UrisLoaded = false;

        #endregion

        #region Constructor

        internal ModelProvider() {}

        #endregion

        #region Methods

        void LoadModelUris()
        {
            if (string.IsNullOrEmpty(Username))
            {
                Username = Environment.UserName;
            }

            Agents = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/agents", Username));
            Activities = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/activities", Username));
            WebActivities = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/activities/web", Username));
            Monitoring = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/monitoring", Username));
            UrisLoaded = true;
        }

        public void InitializeStore()
        {
            if (!UrisLoaded)
            {
                LoadModelUris();
            }
        }

        public bool CheckAgents()
        {
            IModel model = GetAgents();

            return !model.IsEmpty;
        }

        public void InitializeAgents()
        {
            IModel model = GetAgents();

            model.Clear();

            // Create a default user..
            Person user = model.CreateResource<Person>();
            user.Commit();

            Association association = model.CreateResource<Association>();
            association.Agent = user;
            association.Role = new Role(art.USER);
            association.Commit();

            // Create the default agents..
            InstallAgent(model, "application://inkscape.desktop/", "Inkscape", "inkscape", "#EE204E", true);
            InstallAgent(model, "application://krita.desktop/", "Krita", "krita", "#926EAE", true);
            InstallAgent(model, "application://chromium-browser.desktop/", "Chromium", "chromium-browser", "#1F75FE");
            InstallAgent(model, "application://firefox-browser.desktop/", "Firefox", "firefox", "#1F75FE");
            InstallAgent(model, "http://adobe.com/products/photoshop", "Adobe Photoshop", "photoshop.exe", "#EE2000", true);
            InstallSoftwareAssociation(model, "http://adobe.com/products/photoshop", "2015");
            InstallAgent(model, "http://adobe.com/products/illustrator", "Adobe Illustrator", "illustrator.exe", "#EE2000", true);
            InstallSoftwareAssociation(model, "http://adobe.com/products/illustrator", "2015");
            InstallSoftwareAssociation(model, "http://adobe.com/products/illustrator", "2015.2");
        }

        public void InstallAgent(IModel model, string uri, string name, string executableName, string colour, bool captureEnabled = false)
        {
            UriRef agentUri = new UriRef(uri);

            if (!model.ContainsResource(agentUri))
            {
                //Logger.LogInfo("Installing agent {0}", name);

                SoftwareAgent agent = model.CreateResource<SoftwareAgent>(agentUri);
                agent.Name = name;
                agent.ExecutableName = executableName;
                agent.IsCaptureEnabled = captureEnabled;
                agent.ColourCode = colour;
                agent.Commit();
            }
            else
            {
                bool modified = false;

                SoftwareAgent agent = model.GetResource<SoftwareAgent>(agentUri);

                if (agent.Name != name)
                {
                    agent.Name = name;

                    modified = true;
                }

                if (string.IsNullOrEmpty(agent.ColourCode))
                {
                    agent.ColourCode = colour;

                    modified = true;
                }

                if (modified)
                {
                    //Logger.LogInfo("Updating agent {0}", name);

                    agent.Commit();
                }
            }
        }

        public void InstallSoftwareAssociation(IModel model, string uri, string version)
        {
            UriRef associationUri = new UriRef(uri + "#" + version);

            if (!model.ContainsResource(associationUri))
            {
                SoftwareAssociation association = model.CreateResource<SoftwareAssociation>(associationUri);
                association.Agent = new SoftwareAgent(new UriRef(uri));
                association.Role = new Role(new UriRef(ART.SOFTWARE));
                association.Commit();
            }
        }

        public void ReleaseStore()
        {
            int id = Thread.CurrentThread.ManagedThreadId;

            if (_stores.ContainsKey(id))
            {
                var x = _stores[id];
                x.Dispose();
                _stores.Remove(id);
            }
        }

        public IModelGroup GetAll()
        {
            IModelGroup result = Store.CreateModelGroup();
            result.Add(GetAgents(Store));
            result.Add(GetActivities(Store));
            result.Add(GetWebActivities(Store));
            result.Add(GetMonitoring());

            return result;
        }

        public IModel GetAllActivities()
        {
            IModelGroup result = Store.CreateModelGroup();
            result.Add(GetAgents(Store));
            result.Add(GetActivities(Store));
            result.Add(GetWebActivities(Store));

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
