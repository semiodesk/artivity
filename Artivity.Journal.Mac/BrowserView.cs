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
using System.Net;
using AppKit;
using WebKit;
using Foundation;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Artivity.Journal.Mac
{
    [Register("BrowserView")]
    public class BrowserView : WebView
    {
        #region Members

        public NSUrl HomeUrl { get; set; }

        #endregion

        #region Constructors

        public BrowserView(IntPtr handle) : base(handle)
        {
            RegisterForDraggedTypes(new string[] { "NSFilenamesPboardType" });
        }

        #endregion

        #region Methods

        public void NavigateHome()
        {
            MainFrame.LoadRequest(new NSUrlRequest(HomeUrl));
        }

        public override NSDragOperation DraggingEntered(NSDraggingInfo sender)
        {
            NSPasteboard pasteboard = sender.DraggingPasteboard;

            bool typeExists = (Array.IndexOf(pasteboard.Types, "NSFilenamesPboardType") >= 0);

            if (typeExists)
            {
                NSPasteboardItem[] items = pasteboard.PasteboardItems;

                if (GetAppBundles(items).Any())
                {
                    MainFrame.WindowObject.CallWebScriptMethod("showOverlay", new NSObject[] { new NSString("#msg-add-app") });

                    sender.AnimatesToDestination = true;

                    return NSDragOperation.Link;
                }
                else if (GetArchiveFiles(items).Any())
                {
                    MainFrame.WindowObject.CallWebScriptMethod("showOverlay", new NSObject[] { new NSString("#msg-import") });

                    sender.AnimatesToDestination = true;

                    return NSDragOperation.Link;
                }
                else
                {
                    MainFrame.WindowObject.CallWebScriptMethod("showOverlay", new NSObject[] { new NSString("#msg-unsupported-file-type") });

                    return NSDragOperation.None;
                }
            }
            else
            {
                return NSDragOperation.None;
            }
        }

        public override void DraggingExited(NSDraggingInfo sender)
        {
            if (!Frame.Contains(sender.DraggingLocation))
            {
                MainFrame.WindowObject.CallWebScriptMethod("hideOverlays", new NSObject[] { });
            }
        }

        public override void DraggingEnded(NSDraggingInfo sender)
        {
            if (Frame.Contains(sender.DraggingLocation))
            {
                NSPasteboard pasteboard = sender.DraggingPasteboard;

                bool typeExists = (Array.IndexOf(pasteboard.Types, "NSFilenamesPboardType") >= 0);

                if (typeExists)
                {
                    NSPasteboardItem[] items = pasteboard.PasteboardItems;

                    if (GetAppBundles(items).Any())
                    {
                        MainFrame.WindowObject.CallWebScriptMethod("showOverlay", new NSObject[] { new NSString("#msg-add-app-success") });

                        foreach (NSUrl url in GetAppBundles(items))
                        {
                            RegisterSoftwareAgent(new Uri("file://" + url.Path));
                        }

                        Thread.Sleep(1000);

                        MainFrame.WindowObject.CallWebScriptMethod("hideOverlays", new NSObject[] { });
                    }
                    else if (GetArchiveFiles(items).Any())
                    {
                        MainFrame.WindowObject.CallWebScriptMethod("showOverlay", new NSObject[] { new NSString("#msg-import-success") });

                        foreach (NSUrl url in GetArchiveFiles(items))
                        {
                            ImportArchiveFile(new Uri("file://" + url.Path));
                        }

                        Thread.Sleep(1000);

                        MainFrame.WindowObject.CallWebScriptMethod("hideOverlays", new NSObject[] { });

                        // Navigate to the dashboard or reload it to show the now file..
                        NavigateHome();
                    }
                }
            }
        }

        private IEnumerable<NSUrl> GetAppBundles(NSPasteboardItem[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                NSUrl url = new NSUrl(items[i].GetStringForType("public.file-url"));

                if (url.Path.EndsWith(".app", StringComparison.InvariantCulture))
                {
                    yield return url;
                }
            }
        }

        private IEnumerable<NSUrl> GetArchiveFiles(NSPasteboardItem[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                NSUrl url = new NSUrl(items[i].GetStringForType("public.file-url"));

                if (url.Path.EndsWith(".artx", StringComparison.InvariantCulture))
                {
                    yield return url;
                }
                else if (url.Path.EndsWith(".arty", StringComparison.InvariantCulture))
                {
                    yield return url;
                }
            }
        }

        private void RegisterSoftwareAgent(Uri url)
        {
            string endpoint = string.Format("http://localhost:{0}/artivity/api/1.0/agents/software/paths/add?url={1}", ViewController.Port, url.AbsoluteUri);

            WebRequest request = WebRequest.Create(endpoint);
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            response.Close();
        }

        private void ImportArchiveFile(Uri url)
        {
            string endpoint = string.Format("http://localhost:{0}/artivity/api/1.0/import?fileUrl={1}", ViewController.Port, url.AbsoluteUri);

            WebRequest request = WebRequest.Create(endpoint);
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            response.Close();
        }

        #endregion
    }
}
