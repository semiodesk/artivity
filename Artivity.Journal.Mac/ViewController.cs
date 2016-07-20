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
using System.Reflection;
using System.IO;
using AppKit;
using WebKit;
using Foundation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CoreGraphics;
using ObjCRuntime;

namespace Artivity.Journal.Mac
{
    public partial class ViewController : NSViewController
    {
        #region Members

        #if DEBUG
        public static string Port = "8262";
        #else
        public static string Port = "8272";
        #endif

        RetryListener retryListener;

        #endregion

        #region Constructors

        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        #endregion

        #region Methods

        public override void ViewDidLoad()
        {
            retryListener = new RetryListener();
            retryListener.Controller = this;

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
                NSOpenPanel panel = NSOpenPanel.OpenPanel;
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

            Browser.DrawsBackground = false;

            if (Program.IsApidAvailable(Port))
            {
                // Initially try to load the journal app.
                OpenJournal();
            }
            else
            {
                OpenLoadingPage();
            }
        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();

            if (View.Layer.BackgroundColor == null)
            {
                var c2 = new CGColor(CGColorSpace.CreateDeviceRGB(), new nfloat[] { new nfloat(51.0 / 255), new nfloat(51.0 / 255), new nfloat(51.0 / 255), new nfloat(1.0) });

                View.Layer.BackgroundColor = c2;
            }

            //Browser.Layer.BackgroundColor = new CoreGraphics.CGColor(new nfloat(29.0/255), new nfloat(29.0/255), new nfloat(29.0/255), new nfloat(1.0));
        }

        private async void TestForServer(TimeSpan interval, int count, CancellationToken cancellationToken)
        {
            try
            {
                bool available = false;

                for (int i = 0; i < count; i++)
                {
                    available = Program.IsApidAvailable(Port);

                    if (available)
                        break;
                    
                    Task task = Task.Delay(interval, cancellationToken);

                    await task;
                }

                this.InvokeOnMainThread(() => 
                {
                    if (available)
                    {
                        OpenJournal();
                    }
                    else
                    {
                        ShowError();
                    }
                });

            }
            catch (TaskCanceledException)
            {
                return;
            }
        }

        private void OpenJournal()
        {
            Browser.HomeUrl = new NSUrl(string.Format("http://localhost:{0}/artivity/app/journal/1.0/", Port));
                                        
            Browser.NavigateHome();
        }

        private void OpenLoadingPage()
        {
            ShowStaticPage("error.index.html");

            CancellationToken token = new CancellationToken();

            TestForServer(TimeSpan.FromSeconds(5), 4, token);
        }

        private void OnBrowserLoadError(object sender, WebFrameErrorEventArgs e)
        {
            ShowStaticPage("error.index.html");

            CancellationToken token = new CancellationToken();

            TestForServer(TimeSpan.FromSeconds(5), 1, token);
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
                }
            } 
        }

        private void ShowError()
        {
            Browser.MainFrame.WindowObject.CallWebScriptMethod("showConnectionError", new NSObject[] { });

            var dom = Browser.MainFrameDocument;
            var elem = dom.GetElementById("retry");

            elem.AddEventListener("click", retryListener, true);
        }

        public void Retry()
        {
            OpenLoadingPage();
        }

        #endregion
    }

    public class RetryListener : DomEventListener
    {
        public ViewController Controller { get; set; }
        
        public override void HandleEvent(DomEvent evt)
        {
            if (Controller != null)
            {
                Controller.Retry();
            }
        }
    }
}
