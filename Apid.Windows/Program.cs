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
// Copyright (c) Semiodesk GmbH 2017

using Artivity.Api;
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
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            RunWindowsService(args);
        }

        protected void RunWindowsService(string[] args)
        {
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
            ILogger log = new Logger();
            log.LogFatal("Unhandled Exception: \n" + e.ToString());
        }

        #endregion
    }
}
