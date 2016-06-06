﻿// LICENSE:
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
//
// Copyright (c) Semiodesk GmbH 2015

using AppKit;
using Foundation;
using Sparkle;
using Artivity.Apid;


namespace Artivity.Journal.Mac
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        private SUUpdater SUUpdater; 

        public AppDelegate()
        {
            
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            SUUpdater = new SUUpdater();
            SUUpdater.Delegate = new SparkleDelegate();
            SUUpdater.AutomaticallyChecksForUpdates = false;
        }

        public override bool ApplicationShouldHandleReopen(NSApplication sender, bool hasVisibleWindows)
        {
            if (sender.Windows.Length == 0)
            {
                return false;
            }

            if (!hasVisibleWindows)
            {
                sender.Windows[0].OrderFront(sender);
            }
            else
            {
                sender.Windows[0].MakeKeyAndOrderFront(sender);
            }

            return true;
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        partial void checkForUpdate(Foundation.NSObject sender)
        {
            Logger.LogInfo("Checking for updates");
            SUUpdater.CheckForUpdates(sender);
        }


 
    }
}

