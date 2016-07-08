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

using System;
using System.Reflection;
using System.IO;
using AppKit;
using WebKit;
using Foundation;
using System.Collections.Generic;

namespace Artivity.Journal.Mac
{
    public partial class ViewController : NSViewController
    {
        #region Members

#if DEBUG
        string Port = "8262";
#else
        string Port = "8272";
#endif

        bool errorHandler = false;

        #endregion

        #region Constructors

        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        #endregion

        #region Methods

        public override void ViewDidLayout()
        {
            

        }
 
        public override void ViewDidLoad()
        {
            // Handle connection errors.
            Browser.FailedProvisionalLoad += OnBrowserLoadError;

            // Allow to open new browser windows.
            Browser.DecidePolicyForNavigation += (object sender, WebNavigationPolicyEventArgs e) =>
            {
                WebView.DecideUse(e.DecisionToken);
            };

            Browser.DecidePolicyForNewWindow += (object sender, WebNewWindowPolicyEventArgs e) =>
            {
                Browser.MainFrame.LoadRequest(e.Request);
            };


            Browser.UIRunOpenPanelForFileButton += (object sender, WebViewRunOpenPanelEventArgs e) =>
            {
                var panel = NSOpenPanel.OpenPanel;
                panel.CanChooseFiles = true;
                panel.CanChooseDirectories = false;
                panel.AllowedFileTypes = new string[] { "png", "jpg", "jpeg" };
                panel.AllowsMultipleSelection = false;
                panel.ParentWindow = this.View.Window;

                if (panel.RunModal() == 1)
                {
                    string[] result = new string[panel.Urls.Length];

                    for (int i = 0; i < panel.Urls.Length; i++)
                    {
                        NSUrl url = panel.Urls[i];

                        result[i] = url.Path;
                    }

                    e.ResultListener.ChooseFilename(result[0]);
                }
                else
                {
                    e.ResultListener.Cancel();
                }
            };

            Browser.WantsLayer = false;
            
            // Initially try to load the journal app.
            OpenJournal();
        }

        private void OpenJournal()
        { 
            Browser.MainFrame.LoadRequest(new NSUrlRequest(new NSUrl(string.Format("http://localhost:{0}/artivity/app/journal/1.0/", Port))));
        }

        private void OnBrowserLoadError(object sender, WebFrameErrorEventArgs e)
        {
            if (!errorHandler)
            {
                Browser.UIMouseDidMoveOverElement += MouseMove;
                errorHandler = true;
            }
            NSUrl url = NSBundle.MainBundle.GetUrlForResource("error-no-apid-connection", "html", "error");
            Browser.MainFrame.LoadRequest(new NSUrlRequest(url));
        }

        void MouseMove(object sender, WebViewMouseMovedEventArgs args)
        {
            NSEvent ev = NSApplication.SharedApplication.CurrentEvent;
            if (ev.Type == NSEventType.LeftMouseUp)
            {
                Browser.UIMouseDidMoveOverElement -= MouseMove;
                errorHandler = false;
                OpenJournal();
            }
        }


        private void ShowStaticPage(string name)
        {
            // NOTE: This somehow only works the first time. Any subsequent requests to the
            // registered URL fail for an unknown reason.
            string assemblyName = "Artivity.Apid.Modules.Journal";

            // Read the connection error page from the artivity journal module assembly.
            Assembly assembly = Assembly.Load(assemblyName);
            string resourceName = assemblyName + "." + name;

            // Read the page into a stream and display it in the browser window.
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string html = reader.ReadToEnd();

                    Browser.MainFrame.LoadHtmlString(html, new NSUrl("http://localhost/"));
                    //Browser.

                }
            } 
        }


        #endregion
    }
}
