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
using System.Runtime.InteropServices;
using System.Linq;
using CommandLine;
using Nancy.Hosting.Self;
using Semiodesk.Trinity;
using Semiodesk.TinyVirtuoso;
using Artivity.DataModel;

namespace Artivity.Api.Http
{
    public class HttpService
    {
        #region Members

        /// <summary>
        /// The REST API service port.
        /// </summary>
        public const int ServicePort = 8272;

        /// <summary>
        /// The REST API service thread.
        /// </summary>
        protected Thread ServiceThread;

        private NancyHost _serviceHost;

        private AutoResetEvent _wait = new AutoResetEvent(false);

        private AutoResetEvent _finalize = new AutoResetEvent(false);

        /// <summary>
        /// The Virtuoso database port.
        /// </summary>
        public const int VirtuosoPort = 8273;

        private TinyVirtuoso _virtuoso;

        private Virtuoso _virtuosoInstance = null;

        /// <summary>
        /// Indicates if the ontologies used for inferencing should be updated from the copies on the hard drive.
        /// </summary>
        public bool UpdateOntologies { get; set; }

        #endregion

        public void Start(bool blocking = true)
        {
            SemiodeskDiscovery.Discover();

            string version = typeof(HttpService).Assembly.GetName().Version.ToString();

            Console.WriteLine("Artivity Logging Service, Version {0}", version);
            Console.WriteLine();

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

        public void Stop()
        {
            _wait.Set();
            _finalize.WaitOne();
        }

        private void ServiceProcess()
        {
            // Make sure the models are all set up.
            InitializeModels();

            Models.InitializeStore();

            HostConfiguration config = new HostConfiguration();
            config.RewriteLocalhost = true;
            config.UnhandledExceptionCallback = new Action<Exception>((ex) =>
            {
                Logger.LogError(ex.Message);
            });

            using (_serviceHost = new NancyHost(config, new Uri("http://localhost:" + ServicePort)))
            {
                try
                {
                    _serviceHost.Start();

                    Logger.LogInfo("Started listening on port {0}..", ServicePort);

                    using (var monitor = FileSystemMonitor.Instance)
                    {
                        _wait.WaitOne();
                    }
                }
                finally
                {
                    Logger.LogInfo("Stopped listening on port {0}..", ServicePort);
                }
            }

            _finalize.Set();
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
            IStore store = StoreFactory.CreateStore(Models.ConnectionString);

            if (UpdateOntologies)
            {
                Logger.LogInfo("Updating ontologies.");

                store.LoadOntologySettings();
            }

            if (!store.ContainsModel(Models.Agents))
            {
                Logger.LogInfo("Creating model {0}..", Models.Agents);

                store.CreateModel(Models.Agents);
            }

            if (!store.ContainsModel(Models.Activities))
            {
                Logger.LogInfo("Creating model {0}..", Models.Activities);

                store.CreateModel(Models.Activities);
            }

            if (!store.ContainsModel(Models.WebActivities))
            {
                Logger.LogInfo("Creating model {0}..", Models.WebActivities);

                store.CreateModel(Models.WebActivities);
            }

            if (!store.ContainsModel(Models.Monitoring))
            {
                Logger.LogInfo("Creating model {0}..", Models.Monitoring);

                store.CreateModel(Models.Monitoring);
            }

            store.Dispose();
        }

        private void InitializeDatabase()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix || IsRunningOnMac())
            {
                // We are running on Windows or Mac. Start the database using TinyVirtuoso..
                Logger.LogInfo("Starting the OpenLink Virtuoso database on port {0}...", VirtuosoPort);

                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                // The database is started in the user's application data folder on port 8273..
                _virtuoso = new TinyVirtuoso(Path.Combine(appData, "Artivity"));
                _virtuosoInstance = _virtuoso.GetOrCreateInstance("Data");
                _virtuosoInstance.Configuration.Parameters.ServerPort = string.Format("localhost:{0}", VirtuosoPort);
                _virtuosoInstance.Configuration.SaveConfigFile();
                _virtuosoInstance.RemoveLock();
                _virtuosoInstance.Start();

                Models.ConnectionString = _virtuosoInstance.GetTrinityConnectionString() + ";rule=urn:semiodesk/ruleset";
            }
            else
            {
                // We are running on Linux..
                Models.ConnectionString = GetConnectionStringFromConfiguration();
            }
        }

        static bool IsRunningOnMac()
        {
            string os = string.Empty;

            IntPtr buffer = IntPtr.Zero;

            try
            {
                buffer = Marshal.AllocHGlobal(8192);

                // This is a hacktastic way of getting sysname from uname()..
                if (uname(buffer) == 0)
                {
                    os = Marshal.PtrToStringAnsi(buffer).ToLowerInvariant();
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }

            return os == "darwin";;
        }

        [DllImport("libc")]
        static extern int uname(IntPtr buf);
    }
}
