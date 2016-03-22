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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Artivity.Api.Plugin;
using MonoDevelop.MacInterop;

namespace Artivity.Api.Http
{
    class Program
    {
        #region Members
        PluginChecker _checker;
        #endregion

        public static void Main(string[] args)
        {
            
            Program p = new Program ();
            p.Run (args);
        }

        public void Run(string[] args)
        {
            Options options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            bool consoleLogging = true;
            if(options.LogConfig != null && File.Exists(options.LogConfig))
            {
                try
                {
                    FileInfo logFileConfig = new FileInfo(options.LogConfig);
                    log4net.Config.XmlConfigurator.Configure(logFileConfig);
                    consoleLogging = false;
                }catch(Exception)
                {
                }
            }
            if (consoleLogging)
            {
                var appender = new log4net.Appender.ConsoleAppender();
                appender.Name = "ConsoleAppender";
                var layout = new log4net.Layout.PatternLayout();
                layout.ConversionPattern = layout.ConversionPattern = "%newline%date{g} %-5level – %message%newline";
                layout.ActivateOptions();
                appender.Layout = layout;
                log4net.Config.BasicConfigurator.Configure(appender);
            }

            #if !DEBUG
            _checker = PluginCheckerFactory.CreatePluginChecker();
            _checker.Check();
            IInstallationWatchdog wd = InstallationWatchdogFactory.CreateWatchdog ();
            wd.ProgrammInstalledOrRemoved += ProgramInstalled;
            wd.Start();
            #endif


            HttpService service = new HttpService();
            service.UpdateOntologies = options.Update;

            // Listen to SIGINT for cancelling the daemon.
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
                {
                    Logger.LogInfo("Received SIGINT. Shutting down.");

                    service.Stop();
#if !DEBUG
                    wd.Stop();
#endif
                };

            service.Start();
        }

        void ProgramInstalled(object sender, EventArgs entry)
        {
            _checker.Check();
        }
    }

}
