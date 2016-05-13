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

namespace Artivity.Apid
{
    public class ProgramBase
    {
        #region Members

        private PluginChecker _pluginChecker;
        private IInstallationWatchdog _watchdog;
        protected HttpService Service;
        protected Options Options;
        protected bool Initialized = false;

        // Commandline arguments
        protected string[] _args;

        #endregion

        protected bool InitializeOptions()
        {
            Options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(_args, Options))
            {
                return false;
            }
            return true;
        }

        protected void InitializeLogging()
        {
            bool consoleLogging = true;

            if(Options.LogConfig != null && File.Exists(Options.LogConfig))
            {
                try
                {
                    FileInfo logFileConfig = new FileInfo(Options.LogConfig);

                    log4net.Config.XmlConfigurator.Configure(logFileConfig);

                    consoleLogging = false;
                }
                catch(Exception)
                {
                }
            }

            if (consoleLogging)
            {
                var layout = new log4net.Layout.PatternLayout();
                layout.ConversionPattern = layout.ConversionPattern = "%date{g} %-5level – %message%newline";
                layout.ActivateOptions();

                var appender = new log4net.Appender.ConsoleAppender();
                appender.Name = "ConsoleAppender";
                appender.Layout = layout;

                log4net.Config.BasicConfigurator.Configure(appender);
            }
        }

        protected void InitializePluginChecker()
        {
            _pluginChecker = PluginCheckerFactory.CreatePluginChecker();
            _pluginChecker.Check();

            _watchdog = InstallationWatchdogFactory.CreateWatchdog();
            _watchdog.ProgrammInstalledOrRemoved += OnProgramInstalled;
            _watchdog.Start();
        }

        protected bool Initialize()
        {
            if (!InitializeOptions())
                return false;
                

            InitializeLogging();


            #if !DEBUG
            InitializePluginChecker();
            #endif

            InitializeService();

            ShutdownOnSIGINT();

            Initialized = true;
            return true;
        }

        protected virtual bool Run(string[] args)
        {
            if (!Initialized)
            {
                bool res = Initialize();
                if( !res)
                    return false;
            }
            
            Service.Start();
            return true;
        }

        protected void InitializeService()
        {
            Service = new HttpService();
            Service.UpdateOntologies = Options.Update;
        }

        protected void ShutdownOnSIGINT()
        {
            // Listen to SIGINT for cancelling the daemon.
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
                {
                    Logger.LogInfo("Received SIGINT. Shutting down.");

                    Service.Stop();
                    #if !DEBUG
                    StopWatchdog();
                    #endif
                };
        }

        protected void StopWatchdog()
        {
            _watchdog.Stop();
        }

        private void OnProgramInstalled(object sender, EventArgs entry)
        {
            _pluginChecker.Check();
        }
    }
}
