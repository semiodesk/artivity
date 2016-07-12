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

namespace Artivity.Journal.Mac
{
    [Register("BrowserView")]
    public class BrowserView : WebView
    {
        public BrowserView(IntPtr handle) : base(handle)
        {
            RegisterForDraggedTypes(new string[] { "NSFilenamesPboardType" });
        }

        public override NSDragOperation DraggingEntered(NSDraggingInfo sender)
        {
            NSPasteboard pasteboard = sender.DraggingPasteboard;

            bool typeExists = (Array.IndexOf(pasteboard.Types, "NSFilenamesPboardType") >= 0);

            if (typeExists)
            {
                return NSDragOperation.Link;
            }
            else
            {
                return NSDragOperation.None;
            }
        }

        public override void DraggingEnded(NSDraggingInfo sender)
        {
            NSPasteboard pasteboard = sender.DraggingPasteboard;

            bool typeExists = (Array.IndexOf(pasteboard.Types, "NSFilenamesPboardType") >= 0);

            if (typeExists)
            {
                NSPasteboardItem[] pasteboardItems = pasteboard.PasteboardItems;

                for (int i = 0; i < pasteboardItems.Length; i++)
                {
                    NSUrl url = new NSUrl(pasteboardItems[i].GetStringForType("public.file-url"));

                    if(url.Path.EndsWith(".app"))
                    {
                        RegisterSoftwareAgent(new Uri("file://" + url.Path));
                    }
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

            MainFrame.LoadRequest(new NSUrlRequest(new NSUrl(string.Format("http://localhost:{0}/artivity/app/journal/1.0/#/settings", ViewController.Port))));
        }
    }
}
