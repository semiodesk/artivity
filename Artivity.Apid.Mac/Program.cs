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
using System.IO;
using System.Threading;
using MonoMac.AppKit;
using MonoMac.CoreFoundation;

namespace Artivity.Apid.Mac
{
    public class Program : ProgramBase
    {
        public bool Run(Options opts)
        {
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

            if (!Initialize())
            {
                return false;
            }

            // Initialize the Xamarin Mac application.
            NSApplication.Init();

            // Reigster the platform specific file system watcher.
            FileSystemWatcherFactory.CreateHandler(() => { return new FSEventsFileSystemWatcher(); });

            Thread thread = new Thread(ServiceThread);
            thread.Start();

            DispatchQueue.MainIteration();
            thread.Join();

            return true;
        }

        protected void ServiceThread()
        {
            // Start the service.
            Run();

            // When we are finished we tell the DispatcherQueue to stop.
            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                DispatchQueue.CurrentQueue.Suspend();
            });
        }
    }
}
