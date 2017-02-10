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

using Artivity.Api.Platform;
using System;
using System.IO;

namespace Artivity.Apid
{
    public class ProgramBase
    {
        #region Members

        private bool _loggingInitialized = false;

        protected string[] Args;

        protected Options Options;

        protected HttpService Service;

        protected bool Initialized;

        protected bool OverwriteLogging;

        protected string LogConfigFile;

        protected IPlatformProvider Platform;

        #endregion

        #region Constructors

        public ProgramBase()
        {
        }

        #endregion

        #region Methods

        protected bool Run()
        {
            if (!Initialized && !Initialize())
            {
                return false;
            }

            Service.Start();

            return true;
        }

        protected bool Initialize()
        {
            if (!InitializeOptions())
            {
                return false;
            }

            InitializeLogging();
            InitializeService();

            EnableShutdownOnSigInt();

            Initialized = true;

            return true;
        }

        protected bool InitializeOptions()
        {
            if (Options == null)
            {
                Options = new Options();

                return CommandLine.Parser.Default.ParseArguments(Args, Options);
            }

            return true;
        }


        protected void InitializeLogging()
        {
            if (_loggingInitialized)
                return;
            
            bool consoleLogging = true;

            string logFile = null;

            if (OverwriteLogging)
            {
                logFile = LogConfigFile;
            }
            else if (Options.LogConfig != null)
            {
                logFile = Options.LogConfig;
            }

            if (File.Exists(logFile) && !Options.NoLog)
            {
                try
                {
                    FileInfo logFileConfig = new FileInfo(logFile);

                    log4net.Config.XmlConfigurator.Configure(logFileConfig);

                    consoleLogging = false;
                }
                catch (Exception ex)
                {
                    Platform.Logger.LogError(ex.Message);
                }
            }

            if (consoleLogging)
            {
                var layout = new log4net.Layout.PatternLayout();
                layout.ConversionPattern = layout.ConversionPattern = "%date{g} [%-5level] – %message%newline";
                layout.ActivateOptions();

                var appender = new log4net.Appender.ConsoleAppender();
                appender.Name = "ConsoleAppender";
                appender.Layout = layout;

                log4net.Config.BasicConfigurator.Configure(appender);
            }

            _loggingInitialized = true;
        }

        protected void InitializeService()
        {
            Service = new HttpService();
            Service.PlatformProvider = Platform;
            Service.UpdateOntologies = Options.Update;
        }

        protected void EnableShutdownOnSigInt()
        {
            // Listen to SIGINT for cancelling the daemon.
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                Platform.Logger.LogInfo("Received SIGINT. Shutting down.");

                Service.Stop();

            };
        }

        #endregion
    }
}
