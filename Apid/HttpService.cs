﻿// LICENSE:
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

using Artivity.Api.Platforms;
using Artivity.Apid.IO;
using Artivity.Apid.Platforms;
using Artivity.Apid.Plugin;
using Artivity.DataModel;
using Semiodesk.Trinity;
using Semiodesk.TinyVirtuoso;
using System;
using System.IO;
using System.Configuration;
using System.Threading;
using Nancy.Hosting.Self;
using Nancy.TinyIoc;

namespace Artivity.Apid
{
    public class HttpService
    {
        #region Members

        /// <summary>
        /// The HTTP service port.
        /// </summary>
#if DEBUG
        private int _servicePort = 8262;
#else
        private int _servicePort = 8272;
#endif

        /// <summary>
        /// The Virtuoso database port.
        /// </summary>
#if DEBUG
        private int _virtuosoPort = 8263;
#else
        private int _virtuosoPort = 8273;
#endif

        /// <summary>
        /// Get or set the REST API service port.
        /// </summary>
        /// <remarks>
        /// This property can only be set if the service has not been started yet.
        /// </remarks>
        public int ServicePort
        {
            get { return _servicePort; }
            private set
            {
                if(!IsRunning)
                {
                    _servicePort = value;
                }
            }
        }

        /// <summary>
        /// The thread for the HTTP service.
        /// </summary>
        protected Thread ServiceThread;

        /// <summary>
        /// The host of the Nancy HTTP service.
        /// </summary>
        private NancyHost _serviceHost;

        /// <summary>
        /// Handle used to keep the service threads alive.
        /// </summary>
        private AutoResetEvent _wait = new AutoResetEvent(false);

        /// <summary>
        /// The Virtuoso database port.
        /// </summary>
        /// <remarks>
        /// This property can only be set if the service has not been started yet.
        /// </remarks>
        public int VirtuosoPort 
        { 
            get { return _virtuosoPort; } 
            set 
            {
                if(!IsRunning)
                {
                    _virtuosoPort = value;
                }
            } 
        }

        /// <summary>
        /// The TinyVirtuoso database instance manager.
        /// </summary>
        private TinyVirtuoso _virtuoso;

        /// <summary>
        /// The OpenLink Virtuoso database instance.
        /// </summary>
        private Virtuoso _virtuosoInstance = null;

        /// <summary>
        /// Indicates if the ontologies used for inferencing should be updated from the copies on the hard drive.
        /// </summary>
        public bool UpdateOntologies { get; set; }

        /// <summary>
        /// Indicates that the service is running.
        /// </summary>
        public bool IsRunning { get { return ServiceThread != null && ServiceThread.IsAlive; } }

        public IModelProvider ModelProvider { get; set; }

        public string Username { get; set; }

        public string ApplicationData { get; set; }

        public string UserFolder { get; set; }

        public IPlatformProvider PlatformProvider { get; set; }

        public AutoResetEvent DatabaseStarted { get; private set; }

        private bool _started = false;

        private PluginChecker _pluginChecker = null;

        private IProgramInstallationWatchdog _watchdog;

        #endregion

        #region Constructor

        public HttpService()
        {
            DatabaseStarted = new AutoResetEvent(false);
            ApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Username = Environment.UserName;
        }   

        #endregion

        #region Methods

        public void Start(bool blocking = true)
        {
            string version = typeof(HttpService).Assembly.GetName().Version.ToString();

            SemiodeskDiscovery.Discover();

            if (PlatformProvider == null)
            {
                PlatformProvider = new PlatformProvider(ApplicationData, UserFolder, Username);
            }

            PlatformProvider.Logger.LogInfo("--- Artivity API Service, Version {0} ---", version);


            // Make sure the database is started.
            StartDatabase();

            if (PlatformProvider.Config.IsNew || UpdateOntologies)
            {
                LoadOntologies();
            }

            // Test for SoftwareAgents 
            InitializeSoftwareAgentPlugins();

            // Start the daemon in a new thread.
            ServiceThread = new Thread(StartService);
            ServiceThread.Start();

            if (blocking)
            {
                ServiceThread.Join();
            }
        }

        public void Stop(bool waitForEnd = true)
        {
            PlatformProvider.Logger.LogInfo("Stopping service..");

            StopWatchdog();

            if (ServiceThread != null && ServiceThread.IsAlive)
            {
                _wait.Set();

                if (waitForEnd)
                {
                    ServiceThread.Join();
                }
            }

            StopDatabase();
        }

        private void StartService()
        {
            UserConfig userConfig = PlatformProvider.Config;

            ModelProvider.Uid = userConfig.GetUserId();

            Bootstrapper customBootstrapper = new Bootstrapper();
            customBootstrapper.PlatformProvider = PlatformProvider;
            customBootstrapper.ModelProvider = ModelProvider;
            customBootstrapper.PluginChecker = _pluginChecker;

            HostConfiguration hostConfig = new HostConfiguration();
            hostConfig.RewriteLocalhost = true;
            hostConfig.UnhandledExceptionCallback = new Action<Exception>((ex) =>
            {
                PlatformProvider.Logger.LogError(ex.Message, ex);
            });

            using (_serviceHost = new NancyHost(customBootstrapper, hostConfig, new Uri("http://localhost:" + _servicePort)))
            {
                try
                {
                    // Start the Nancy service host.
                    _serviceHost.Start();

                    PlatformProvider.Logger.LogInfo("Started HTTP service on port {0}", _servicePort);

                    using (var monitor = FileSystemMonitor.Instance)
                    {
                        // Start the file system change monitor.
                        monitor.Initialize(ModelProvider, PlatformProvider);
                        monitor.Enable();

                        _wait.WaitOne();

                        monitor.Disable();
                        monitor.Dispose();
                    }

                    _serviceHost.Stop();
                }
                catch (Exception ex)
                {
                    PlatformProvider.Logger.LogError(ex);
                }
                finally
                {
                    PlatformProvider.Logger.LogInfo("Stopped HTTP service on port {0}", _servicePort);
                }
            }

            customBootstrapper.Dispose();

            TinyIoCContainer.Current.Dispose();
        }

        private void StartDatabase()
        {
            string uid = PlatformProvider.Config.GetUserId();

            if (PlatformProvider.IsWindows || PlatformProvider.IsMac)
            {
                string deploymentDir = PlatformProvider.DeploymentDir;

                // We are running on Windows or Mac. Start the database using TinyVirtuoso..
                PlatformProvider.Logger.LogInfo("Starting the OpenLink Virtuoso database..");
                PlatformProvider.Logger.LogInfo("Database folder: {0}", PlatformProvider.DatabaseFolder);
                PlatformProvider.Logger.LogInfo("Deployment folder: {0}", PlatformProvider.DeploymentDir);

                // The database is started in the user's application data folder on port 8273..
                _virtuoso = new TinyVirtuoso(PlatformProvider.ArtivityDataFolder, deploymentDir, false);
                _virtuosoInstance = _virtuoso.GetOrCreateInstance(PlatformProvider.DatabaseName);
                _virtuosoInstance.Configuration.Parameters.ServerPort = string.Format("127.0.0.1:{0}", _virtuosoPort);
                _virtuosoInstance.Configuration.SaveConfigFile();
                _virtuosoInstance.RemoveLock();
                _virtuosoInstance.Start(true, TimeSpan.FromSeconds(30));

                string connectionString = _virtuosoInstance.GetTrinityConnectionString() + ";rule=urn:semiodesk/ruleset";
                connectionString = connectionString.Replace("localhost", "127.0.0.1");

                string nativeConnectionString = _virtuosoInstance.GetAdoNetConnectionString();
                nativeConnectionString = nativeConnectionString.Replace("localhost", "127.0.0.1");

                ModelProvider = ModelProviderFactory.CreateModelProvider(connectionString, nativeConnectionString, uid);

                if (!ModelProvider.CheckOntologies())
                {
                    LoadOntologies();
                }

                PlatformProvider.Logger.LogDebug("Database connection: {0}", nativeConnectionString);
                PlatformProvider.Logger.LogInfo("Started database on port {0}", _virtuosoPort);
            }
            else
            {
                // We are running on Linux..
                ModelProvider = ModelProviderFactory.CreateModelProvider(GetConnectionStringFromConfiguration(), "", uid);
            }

            if (!ModelProvider.CheckAgents())
            {
                ModelProvider.InitializeAgents();
            }

            _started = true;

            DatabaseStarted.Set();
        }

        private void InitializeSoftwareAgentPlugins()
        {
            PlatformProvider.Logger.LogInfo("Initializing software agent plugins..");

            DirectoryInfo pluginDirectory = new DirectoryInfo(PlatformProvider.PluginDir);

            _pluginChecker = PluginCheckerFactory.CreatePluginChecker(PlatformProvider, ModelProvider, pluginDirectory);
            _pluginChecker.CheckPlugins();

            if (PlatformProvider.CheckForNewSoftwareAgents)
            {
                _watchdog = ProgramInstallationWatchdogFactory.CreateWatchdog();
                _watchdog.ProgrammInstalledOrRemoved += OnProgramInstalled;

                StartWatchdog();
            }
        }

        private void OnProgramInstalled(object sender, EventArgs entry)
        {
            _pluginChecker.CheckPlugins(PlatformProvider.AutomaticallyInstallSoftwareAgentPlugins);
        }

        private void StartWatchdog()
        {
            if (_watchdog != null)
            {
                _watchdog.Start();
            }
        }

        private void StopWatchdog()
        {
            if (_watchdog != null) 
            {
                _watchdog.Stop();
                _watchdog.Dispose();
            }
        }

        private void StopDatabase()
        {
            if (_virtuosoInstance.ProcessRunning)
            {
                PlatformProvider.Logger.LogInfo("Stopping Database..");

                _virtuosoInstance.Stop(false);
            }
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

        private void LoadOntologies()
        {
            using (IStore store = StoreFactory.CreateStore(ModelProvider.ConnectionString))
            {
                PlatformProvider.Logger.LogInfo("Loading ontologies..");

                FileInfo f = new FileInfo(System.Reflection.Assembly.GetAssembly(typeof(HttpService)).Location);

                var dir = f.Directory;

                string config = Path.Combine(dir.FullName, string.Format("{0}.config", f.Name));

                try
                {
                    store.LoadOntologySettings(config, PlatformProvider.OntologyDir);
                }
                catch (Exception e)
                {
                    PlatformProvider.Logger.LogError(e.Message);
                }
            }
        }

        /// <summary>
        /// This method checks if the database has been started. If not, it will block until it starts.
        /// </summary>
        public void TestDatabaseStarted()
        {
            if (_started)
            {
                return;
            }
            else
            {
                DatabaseStarted.WaitOne();
            }
        }

        #endregion
    }
}