using Artivity.Api.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artivity.WinService
{
    class Program
    {
        #region Members
        public static bool LoggingEnabled = false;

        #endregion

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
            ArtivityService Service = new ArtivityService(ServiceName);
            Service.CreateInstaller(ServiceName, System.ServiceProcess.ServiceAccount.User, System.ServiceProcess.ServiceStartMode.Automatic);

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
                    var  ServiceThread = new Thread(Service.Start);
                    ServiceThread.Start();
                    Console.WriteLine("Press a key to stop...");
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
            Console.Write("ERROR Unhandled Exception: \n" + e.ToString());
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
