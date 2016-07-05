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
using System.Threading;

namespace Artivity.DataModel
{
    public class ModelProvider : IModelProvider
    {
        #region Members

        private bool _initialized = false;

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

        public string Uid { get; set; }

        public Uri Agents { get; set; }

        public Uri Activities { get; set; }

        public Uri WebActivities { get; set; }

        #endregion

        #region Constructor

        internal ModelProvider() {}

        #endregion

        #region Methods

        public void InitializeStore()
        {
            if (!_initialized)
            {
                if (string.IsNullOrEmpty(Uid))
                {
                    throw new Exception("Cannot initialize RDF store: UID must not be empty. Is your config.json valid?");
                }

                Agents = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/agents", Uid));
                Activities = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/activities", Uid));
                WebActivities = new Uri(string.Format("http://localhost:8890/artivity/1.0/{0}/activities/web", Uid));

                _initialized = true;
            }
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
            InstallAgent(model, "application://firefox-browser.desktop/", "Firefox", "firefox", "#1F75FE");
        }

        public void InstallAgent(IModel model, string uri, string name, string executableName, string colour, bool captureEnabled = false)
        {
            UriRef agentUri = new UriRef(uri);

            if (!model.ContainsResource(agentUri))
            {
                //Logger.LogInfo("Installing agent {0}", name);

                SoftwareAgent agent = model.CreateResource<SoftwareAgent>(agentUri);
                agent.Name = name;
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

        public bool CheckOntologies()
        {
            IModel model = Store.GetModel(art.Namespace);

            return !model.IsEmpty;
        }

        public bool CheckAgents()
        {
            IModel model = GetAgents();

            return !model.IsEmpty;
        }

        public IModelGroup GetAll()
        {
            IModelGroup result = Store.CreateModelGroup();
            result.Add(GetAgents());
            result.Add(GetActivities());
            result.Add(GetWebActivities());

            return result;
        }

        public IModelGroup GetAllActivities()
        {
            IModelGroup result = Store.CreateModelGroup();
            result.Add(GetAgents());
            result.Add(GetActivities());
            result.Add(GetWebActivities());

            return result;
        }

        public IModel GetAgents()
        {
            return Store.GetModel(Agents);
        }

        public IModel GetActivities()
        {
            return Store.GetModel(Activities);
        }

        public IModel GetWebActivities()
        {
            return Store.GetModel(WebActivities);
        }

        public void ReleaseStore()
        {
            int id = Thread.CurrentThread.ManagedThreadId;

            if (_stores.ContainsKey(id))
            {
                IStore store = _stores[id];

                store.Dispose();

                _stores.Remove(id);
            }
        }

        #endregion
    }
}
