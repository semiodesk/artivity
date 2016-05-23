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
using System.Threading;
using AppKit;
using CoreFoundation;
using System.IO;

namespace Artivity.Apid.Mac
{
    class Program : ProgramBase
    {
        #region Members

        #endregion

        #region Methods

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run(args);
        }

        public bool Run(string[] args)
        {
            _args = args;
            var applicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var username = Environment.UserName;
            platform = new MacPlatformProvider(applicationData, userFolder, username);

            overwriteLogging = true;
            logConfigFile = Path.Combine(platform.DeploymentDir, "log.config");


            if (!Initialize())
                return false;

            // Initialize the Xamarin Mac application.
            NSApplication.Init();

            // Reigster the platform specific file system watcher.
            FileSystemWatcherFactory.CreateHandler(() => { return new FSEventsFileSystemWatcher(); });

            Thread fsThread = new Thread(FileSystemWatcherThread);
            fsThread.Start();

            DispatchQueue.MainIteration();
            fsThread.Join();
            return true;
        }

        protected void FileSystemWatcherThread()
        {
            // Start the service.
            Run();
            DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    DispatchQueue.CurrentQueue.Suspend();
                });
        }

        #endregion
    }
}
