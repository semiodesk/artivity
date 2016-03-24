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
//  Sebastian Faubel <sebastian@semiodesk.com>
//  Moritz Eberl <moritz@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Linq;
using CommandLine;
using Nancy.Hosting.Self;
using Semiodesk.Trinity;
using Semiodesk.TinyVirtuoso;
using Artivity.DataModel;
using log4net;
using Nancy.TinyIoc;


namespace Artivity.Api.Http
{
    public class HttpService
    {
        #region Members

        /// <summary>
        /// The REST API service port.
        /// </summary>
        #if DEBUG
        private int _servicePort = 8262;
        #else
        private int _servicePort = 8272;
        #endif

        /// <summary>
        /// The REST API service port.
        /// This property can only be set if the service has not been started yet.
        /// </summary>
        public int ServicePort
        {
            get
            {
                return _servicePort; 
            }
            set 
            { 
                if( !IsRunning )
                    _servicePort = value; 
            }
        }

        /// <summary>
        /// The REST API service thread.
        /// </summary>
        protected Thread ServiceThread;

        private NancyHost _serviceHost;

        private AutoResetEvent _wait = new AutoResetEvent(false);

        /// <summary>
        /// The Virtuoso database port.
        /// </summary>
        #if DEBUG
        private int _virtuosoPort = 8263;
        #else
        private int _virtuosoPort = 8273;
        #endif

        /// <summary>
        /// The Virtuoso database port.
        /// This property can only be set if the service has not been started yet.
        /// </summary>
        public int VirtuosoPort 
        { 
            get 
            { 
                return _virtuosoPort; 
            } 
            set 
            { 
                if( !IsRunning ) 
                    _virtuosoPort = value; 
            } 
        }

        private TinyVirtuoso _virtuoso;

        private Virtuoso _virtuosoInstance = null;

        /// <summary>
        /// Indicates if the ontologies used for inferencing should be updated from the copies on the hard drive.
        /// </summary>
        public bool UpdateOntologies { get; set; }

        /// <summary>
        /// Indicates that the service is running.
        /// </summary>
        public bool IsRunning { get { return ServiceThread != null && ServiceThread.IsAlive; } }

        public string ApplicationData { get; set; }

        public IModelProvider ModelProvider { get; set; }

        public string Username {get;set;}
        #endregion

        #region Constructor
        public HttpService()
        {
            ApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }   
        #endregion

        #region Methods
        public void Start(bool blocking = true)
        {
            SemiodeskDiscovery.Discover();

            string version = typeof(HttpService).Assembly.GetName().Version.ToString();

            Logger.LogInfo("Artivity Logging Service, Version {0}", version);

            
            // Make sure the database is started.
            InitializeDatabase();
           

            // Start the daemon in a new thread.
            ServiceThread = new Thread(ServiceProcess);
            ServiceThread.Start();

            if (blocking)
            {
                ServiceThread.Join();
            }
        }

        public void Stop(bool waitForEnd = true)
        {
            if (ServiceThread != null && ServiceThread.IsAlive)
            {
                _wait.Set();
                if (waitForEnd)
                    ServiceThread.Join();

                Logger.LogInfo("Stopping HttpService");
            }
        }

        private void ServiceProcess()
        {

            if (string.IsNullOrEmpty(Username))
                Username = Environment.UserName;
            ModelProvider.Username = Username;

            //throw new Exception("Test");
            // Make sure the models are all set up.
            InitializeModels();

            Bootstrapper customBootstrapper = new Bootstrapper();
            customBootstrapper.ModelProvider = ModelProvider;
            HostConfiguration config = new HostConfiguration();
            config.RewriteLocalhost = true;
            config.UnhandledExceptionCallback = new Action<Exception>((ex) =>
            {
                Logger.LogError(ex.Message, ex);
            });

            using (_serviceHost = new NancyHost(customBootstrapper, config, new Uri("http://localhost:" + _servicePort)))
            {
                try
                {
                   
                    _serviceHost.Start();

                    Logger.LogInfo("Started listening on port {0}..", _servicePort);
                    //Logger.LogInfo("Started listening on port {0}..", _servicePort);

                    using (var monitor = FileSystemMonitor.Instance)
                    {
                        _wait.WaitOne();

                    }
                    _serviceHost.Stop();



                }
                finally
                {
                    
                    Logger.LogInfo("Stopped listening on port {0}..", _servicePort);
                }

            }
            customBootstrapper.Dispose();
            TinyIoCContainer.Current.Dispose();
        }

        private string GetConnectionStringFromConfiguration()
        {
            string configName = "virt0";

            foreach (ConnectionStringSettings s in ConfigurationManager.ConnectionStrings)
            {
                if (s.Name == configName)
                {
                    return s.ConnectionString;
                }
            }

            return null;
        }

        private void InitializeModels()
        {
            IStore store = StoreFactory.CreateStore(ModelProvider.ConnectionString);

            if (UpdateOntologies)
            {
                Logger.LogInfo("Updating ontologies.");

                store.LoadOntologySettings();
            }

            if (!store.ContainsModel(ModelProvider.Agents))
            {
                Logger.LogInfo("Creating model {0}..", ModelProvider.Agents);

                store.CreateModel(ModelProvider.Agents);
            }

            if (!store.ContainsModel(ModelProvider.Activities))
            {
                Logger.LogInfo("Creating model {0}..", ModelProvider.Activities);

                store.CreateModel(ModelProvider.Activities);
            }

            if (!store.ContainsModel(ModelProvider.WebActivities))
            {
                Logger.LogInfo("Creating model {0}..", ModelProvider.WebActivities);

                store.CreateModel(ModelProvider.WebActivities);
            }

            if (!store.ContainsModel(ModelProvider.Monitoring))
            {
                Logger.LogInfo("Creating model {0}..", ModelProvider.Monitoring);

                store.CreateModel(ModelProvider.Monitoring);
            }

            store.Dispose();
        }

        private void InitializeDatabase()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix ||  Platform.IsRunningOnMac())
            {
                #if !DEBUG
                // We are running on Windows or Mac. Start the database using TinyVirtuoso..
                Logger.LogInfo("Starting the OpenLink Virtuoso database on port {0}...", _virtuosoPort);

                string appData = ApplicationData;
                Logger.LogInfo("Database is going to be stored in {0}", Path.Combine(appData, "Artivity", "Data"));

                // The database is started in the user's application data folder on port 8273..
                _virtuoso = new TinyVirtuoso(Path.Combine(appData, "Artivity"));
                _virtuosoInstance = _virtuoso.GetOrCreateInstance("Data");
                _virtuosoInstance.Configuration.Parameters.ServerPort = string.Format("localhost:{0}", _virtuosoPort);
                _virtuosoInstance.Configuration.SaveConfigFile();
                _virtuosoInstance.RemoveLock();
                _virtuosoInstance.Start(false);
                Thread.Sleep(5000);

                string connectionString = _virtuosoInstance.GetTrinityConnectionString() + ";rule=urn:semiodesk/ruleset";
                string nativeConnectionString = _virtuosoInstance.GetAdoNetConnectionString();

                ModelProvider = ModelProviderFactory.CreateModelProvider(connectionString, nativeConnectionString);
                Logger.LogInfo(nativeConnectionString);
                Logger.LogInfo("Virtuoso started!");
                #else
                ModelProvider = ModelProviderFactory.CreateModelProvider("provider=virtuoso;host=localhost;port=8263;uid=dba;pw=dba;rule=urn:semiodesk/ruleset", " Server=localhost:8263;uid=dba;pwd=dba;Charset=utf-8");
                #endif
            }
            else
            {
                // We are running on Linux..
                ModelProvider = ModelProviderFactory.CreateModelProvider(GetConnectionStringFromConfiguration(), null);
            }
        }


        #endregion
    }
}
