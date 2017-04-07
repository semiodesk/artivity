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

using Artivity.DataModel.ObjectModel;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Artivity.DataModel
{
    public class ModelProvider : IModelProvider
    {
        #region Members

        private bool _initialized = false;

        private Uri _synchronizationStateUrl;

        private Dictionary<int, IStore> _stores = new Dictionary<int, IStore>();

        object _lock = new object();

        public IStore Store 
        {
            get
            {
                int id = Thread.CurrentThread.ManagedThreadId;

                lock(_lock)
                {
                    if (_stores.ContainsKey(id))
                    {
                        return _stores[id];
                    }
                    else
                    {
                        var store = StoreFactory.CreateStore(ConnectionString);

                        _stores.Add(id, store);

                        return store;
                    }
                }
            }
        }

        public string ConnectionString { get; set; }

        public string NativeConnectionString { get; set; }

        public string Uid { get; set; }

        public UriRef Default { get; set; }

        public UriRef Agents { get; set; }

        public UriRef Activities { get; set; }

        public UriRef WebActivities { get; set; }

        public string RenderingQueryModifier { get; set; }

        public string GetFilesQueryModifier { get; set; }

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

                string baseUrl = string.Format("http://localhost:8890/artivity/1.0/{0}", Uid);

                Default = new UriRef(baseUrl);
                Agents = new UriRef(baseUrl + "/agents");
                Activities = new UriRef(baseUrl + "/activities");
                WebActivities = new UriRef(baseUrl + "/activities/web");

                RenderingQueryModifier = "BIND( CONCAT('http://localhost:8262/artivity/api/1.0/renderings?uri=', ?entityStub, '&file=', STR(?f) ) as ?file ).";
                GetFilesQueryModifier = "BIND( CONCAT('http://localhost:8262/artivity/api/1.0/renderings/thumbnails?entityUri=', ?entityUri) as ?p).";

                IModel model = GetDefault();

                _synchronizationStateUrl = new UriRef(baseUrl + "#sync");

                if (!model.ContainsResource(_synchronizationStateUrl))
                {
                    ModelSynchronizationState state = model.CreateResource<ModelSynchronizationState>(_synchronizationStateUrl);
                    state.LastLocalRevision = 0;
                    state.LastRemoteRevision = 0;
                    state.Commit();
                }

                _initialized = true;
            }
        }

        public void InitializeAgents()
        {
            IModel model = GetAgents();

            // Create a default user..
            User user = model.CreateResource<User>(new UriRef("urn:art:uid:" + Uid));
            user.Commit();

            Association association = model.CreateResource<Association>();
            association.Agent = user;
            association.Role = new Role(art.AccountOwnerRole);
            association.Commit();
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

            Uri userUri = new UriRef("urn:art:uid:" + Uid);

            if ( !model.ContainsResource(userUri))
                return false;

            User user = model.GetResource<User>(userUri);

            ResourceQuery q = new ResourceQuery(prov.Association);
            q.Where(prov.agent, user);
            q.Where(prov.hadRole, art.AccountOwnerRole);

            var res = model.ExecuteQuery(q).GetResources<Association>();
            if (!res.Any())
            {
                Association association = model.CreateResource<Association>();
                association.Agent = user;
                association.Role = new Role(art.AccountOwnerRole);
                association.Commit();
            }
            return true;
        }

        public IModelGroup CreateModelGroup(params Uri[] models)
        {
            return Store.CreateModelGroup(models);
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

        public IModel GetDefault()
        {
            return Store.GetModel(Default);
        }

        public IModelSynchronizationState GetModelSynchronizationState(IPerson user)
        {
            return Store.GetModel(Default).GetResource<ModelSynchronizationState>(_synchronizationStateUrl); ;
        }

        public int ReleaseStore()
        {
            int id = Thread.CurrentThread.ManagedThreadId;

            lock(_lock)
            {
                if (_stores.ContainsKey(id))
                {
                    IStore store = _stores[id];

                    try
                    {
                        store.Dispose();
                    }
                    catch (Exception)
                    {
                    }

                    _stores.Remove(id);
                }
            }

            return id;
        }

        public void SetProject(string x) { }

        public void SetOwner(string x) { }


        private static string GetTypename<T>()
        {
            return typeof(T).Name.ToLowerInvariant();
        }

        private static Uri CreateDefaultUri<T>(string guid)
        {
            return new Uri(string.Format("http://artivity.io/{0}/{1}", GetTypename<T>(), guid));
        }

        public Uri CreateUri<T>(string guid)
        {
            return CreateDefaultUri<T>(guid);
        }

        public Uri CreateUri<T>()
        {
            return CreateDefaultUri<T>(Guid.NewGuid().ToString());
        }

        #endregion
    }
}
