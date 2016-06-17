using Artivity.Apid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artivity.WinService
{
    class Program
    {
        #region Constructor

        public Program()
        {

        }

        #endregion

        #region Methods

        public static FileInfo GetCurrentAssembly()
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location);
        }

        public static void Main(string[] args)
        {
            Program p = new Program();
            p.Run(args);
        }

        public void Run(string[] args)
        {
            EnableLogging();

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            RunWindowsService(args);
        }

        protected void RunWindowsService(string[] args)
        {
            //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            string ServiceName = "Artivity Service";

#if DEBUG
            string logConfig = "debug.log.config";
#else
            string logConfig = "log.config";
#endif

            ArtivityService Service = new ArtivityService(ServiceName, logConfig);
            Service.CreateInstaller(ServiceName, ServiceAccount.LocalSystem, ServiceStartMode.Automatic);

            string opt = null;

            // check for argumenst
            if (args.Length > 0)
            {
                opt = args[0];

                if (opt != null && opt.ToLower() == "-install")
                {
                    Service.Install();
                }
                else if (opt != null && opt.ToLower() == "-uninstall")
                {
                    Service.Uninstall();
                }
                else if (opt != null && opt.ToLower() == "-debug")
                {
                    // We don't want plugin checks in debug
                    Service.AutoPluginChecker = false;

                    Thread ServiceThread = new Thread(Service.Start);
                    ServiceThread.Start();

                    Console.Read();

                    Service.Stop();
                    Service.Dispose();

                    ServiceThread.Join();
                }

            }

            if (opt == null) // e.g. ,nothing on the command line
            {
                Service.Run();
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.LogFatal("Unhandled Exception: \n" + e.ToString());
        }

        private void EnableLogging()
        {
            /*
            FileInfo logConf = new FileInfo(Path.Combine(GetCurrentAssembly().DirectoryName, "ubiquity.log.xml"));
            if (logConf.Exists)
            {
                XmlConfigurator.Configure(logConf);
                var logger = log4net.LogManager.GetLogger(typeof(UbiquityService));
                logger.Debug("Starting logging");

            }*/
        }

        #endregion
    }
}
