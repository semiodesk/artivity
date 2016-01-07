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
using System.Threading;
using Semiodesk.Trinity;
using Artivity.DataModel;
using CommandLine;
using Nancy.Hosting.Self;
using Semiodesk.TinyVirtuoso;
using System.Runtime.InteropServices;
using System.Configuration;

namespace Artivity.Api.Http
{
    public class HttpService
    {
        #region Members
        public const int Port = 8272;
        public bool UpdateModels { get; set; }
        protected Thread ServiceThread;
        AutoResetEvent _wait = new AutoResetEvent(false);
        AutoResetEvent _finalize = new AutoResetEvent(false);

        Virtuoso _instance = null;
        TinyVirtuoso _tinyVirtuoso;
        NancyHost _host;

        #endregion

        public void Start(bool blocking = true)
        {
            SemiodeskDiscovery.Discover();

            string version = typeof(HttpService).Assembly.GetName().Version.ToString();

            Console.WriteLine("Artivity Logging Service, Version {0}", version);
            Console.WriteLine();

            InitializeDatabase();

            // Start the daemon in a new thread.

            ServiceThread = new Thread(ServiceProcess);
            ServiceThread.Start();
            if (blocking)
                ServiceThread.Join();
            
            

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
            config.UnhandledExceptionCallback = new Action<Exception>((ex) => { Console.WriteLine(ex); });

            using (_host = new NancyHost(config, new Uri("http://localhost:" + Port)))
            {
                
                try
                {
                    _host.Start();

                    Logger.LogInfo("Started listening on port {0}..", Port);

                    using (var monitor = FileSystemMonitor.Instance)
                    {
                        //monitor.Initialize();
                        //monitor.Start();

                        _wait.WaitOne();
                    }
                }
                catch (Exception e)
                {
                }
                finally
                {
                    Logger.LogInfo("Stopped listening on port {0}..", Port);
                }
            }

            _finalize.Set();
        }

        private string GetConnectionStringFromConfiguration()
        {
            string name = "virt0";
            foreach (ConnectionStringSettings setting in ConfigurationManager.ConnectionStrings)
            {
                if (!string.IsNullOrEmpty(name) && setting.Name != name)
                    continue;

                return setting.ConnectionString;
                
            }
            return null;
        }

        private void InitializeModels()
        {
            IStore store = StoreFactory.CreateStore(Models.ConnectionString);


            if (UpdateModels)
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
            
            if (System.Environment.OSVersion.Platform != PlatformID.Unix || IsRunningOnMac())
            {
                Console.WriteLine("Starting Virtuoso database...");
                // We are running on Windows or Mac -> We need to start the TinyVirtuoso
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                _tinyVirtuoso = new TinyVirtuoso(Path.Combine(appData, "Artivity"));
                _instance = _tinyVirtuoso.GetOrCreateInstance("Data");
                _instance.Configuration.Parameters.ServerPort = "localhost:8273";
                _instance.Configuration.SaveConfigFile();
                _instance.RemoveLock();
                _instance.Start();

                Models.ConnectionString = _instance.GetTrinityConnectionString() + ";rule=urn:semiodesk/ruleset";

            }else
            {
                // We are running on Linux
                Models.ConnectionString = GetConnectionStringFromConfiguration();
            }
            
        }

        [DllImport("libc")]
        static extern int uname(IntPtr buf);
        static bool IsRunningOnMac()
        {
            IntPtr buf = IntPtr.Zero;
            try
            {
                buf = Marshal.AllocHGlobal(8192);
                // This is a hacktastic way of getting sysname from uname ()
                if (uname(buf) == 0)
                {
                    string os = Marshal.PtrToStringAnsi(buf);
                    if (os == "Darwin")
                        return true;
                }
            }
            catch
            {
            }
            finally
            {
                if (buf != IntPtr.Zero)
                    Marshal.FreeHGlobal(buf);
            }
            return false;
        }
    }
	
}
