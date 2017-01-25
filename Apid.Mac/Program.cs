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

using Artivity.Apid.IO;
using Artivity.Apid.Mac.Platform;
using Artivity.Apid.Platform;
using Artivity.Apid.Plugins;
using System;
using System.IO;
using System.Threading;
using AppKit;
using Mono.Unix;
using Mono.Unix.Native;

namespace Artivity.Apid.Mac
{

    public class Program : ProgramBase
    {
        NSApplication app;

        public bool Run (Options opts)
        {
            PluginCheckerFactory.RegisterType<Artivity.Apid.Mac.Platform.PluginChecker>();
            ThumbnailProviderFactory.RegisterType<Artivity.Apid.Mac.Platform.ThumbnailProvider>();

            var applicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var username = Environment.UserName;

            Platform = new MacPlatformProvider(applicationData, userFolder, username);
            
#if DEBUG
            LogConfigFile = Path.Combine(Platform.DeploymentDir, "log.config.debug");
#else
            LogConfigFile = Path.Combine(Platform.DeploymentDir, "log.config");
#endif

            Options = opts;
            OverwriteLogging = true;
            InitializeLogging();

            Thread signal_thread = new Thread (WaitSIGINT);
            signal_thread.Start();

            if (!Initialize())
            {
                return false;
            }

            // Initialize the Xamarin Mac application.
            NSApplication.Init();

            // Reigster the platform specific file system watcher.
            FileSystemWatcherFactory.CreateHandler(() => { return new FSEventsFileSystemWatcher (); });

            Thread thread = new Thread(ServiceThread);
            thread.Start();

            app = NSApplication.SharedApplication;
            app.Delegate = null;
            app.Run();

            Console.WriteLine ("Waiting for service to end..");

            thread.Join();

            Console.WriteLine ("All threads stopped; exiting.");

            return true;
        }

        protected void ServiceThread()
        {
            // Start the service.
            Run ();
        }

        protected void WaitSIGINT()
        {
            Platform.Logger.LogInfo("Establishing signal interception...");

            UnixSignal[] signals = new UnixSignal []
            {
                new UnixSignal (Signum.SIGINT),
                new UnixSignal (Signum.SIGUSR1),
                new UnixSignal (Signum.SIGHUP),
                new UnixSignal (Signum.SIGTERM)
            };

            while(true) {
                // Wait for a signal to be delivered
                int index = UnixSignal.WaitAny(signals, -1);

                Signum signal = signals[index].Signum;

                Platform.Logger.LogInfo(string.Format("Received signal {0}. Shutting down!", signal));

                // When we are finished we tell the DispatcherQueue to stop.
                Service.Stop (true);

                app.BeginInvokeOnMainThread(() =>
                {
                    app.Terminate(app);
                });

                return;
            }
        }
    }
}
