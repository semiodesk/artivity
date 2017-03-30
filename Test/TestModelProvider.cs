using Artivity.DataModel;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtivityTest
{
    public class TestModelProvider : IModelProvider
    {
        public TestModelProvider(IStore store, string connectionString, string nativeConnectionString)
        {
            Store = store;
            ConnectionString = connectionString;

            UriRef baseUri = new UriRef("http://artivity.io/test");
            Agents = new UriRef(baseUri, "/agents");
            Activities = new UriRef(baseUri, "/activities");
            WebActivities = new UriRef(baseUri, "/webactivities");
            Default = Activities;
        }


        public IStore Store
        {
            get;
            set;
        }

        public string ConnectionString
        {
            get;
            set;
        }

        public string NativeConnectionString
        {
            get;
            set;
        }

        public Semiodesk.Trinity.UriRef Agents
        {
            get;
            set;
        }

        public Semiodesk.Trinity.UriRef Activities
        {
            get;
            set;
        }

        public Semiodesk.Trinity.UriRef WebActivities
        {
            get;
            set;
        }

        public Semiodesk.Trinity.UriRef Default
        {
            get;
            set;
        }

        public string Uid
        {
            get;
            set;
        }

        public bool CheckAgents()
        {
            return true;
        }

        public bool CheckOntologies()
        {
            return true;
        }

        public void InitializeAgents()
        {
            
        }

        public int ReleaseStore()
        {
            return 0;
        }

        public Semiodesk.Trinity.IModelGroup GetAll()
        {
            throw new NotImplementedException();
        }

        public Semiodesk.Trinity.IModelGroup GetAllActivities()
        {
            throw new NotImplementedException();
        }

        public Semiodesk.Trinity.IModel GetAgents()
        {
            return Store.GetModel(Agents);
        }

        public Semiodesk.Trinity.IModel GetActivities()
        {
            return Store.GetModel(Activities);
        }

        public Semiodesk.Trinity.IModel GetWebActivities()
        {
            return Store.GetModel(WebActivities);
        }

        public Semiodesk.Trinity.IModelGroup CreateModelGroup(params Uri[] models)
        {
            throw new NotImplementedException();
        }

        public IModelSynchronizationState GetModelSynchronizationState(IPerson user)
        {
            throw new NotImplementedException();
        }


        public string RenderingQueryModifier
        {
            get { throw new NotImplementedException(); }
        }

        public string GetFilesQueryModifier
        {
            get { throw new NotImplementedException(); }
        }


        public void SetProject(string project)
        {
            throw new NotImplementedException();
        }
    }
}
