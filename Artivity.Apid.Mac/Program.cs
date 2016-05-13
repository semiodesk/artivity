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

        protected override bool Run(string[] args)
        {
            _args = args;

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
            base.Run(_args);
            DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    DispatchQueue.CurrentQueue.Suspend();
                });
        }

        #endregion
    }
}