
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.WebKit;
using System.Reflection;
using System.IO;

namespace Artivity.Journal.Mac
{
    public partial class MainWindow : MonoMac.AppKit.NSWindow
    {
        #region Members

        public readonly WebView Browser = new WebView();

        #endregion

        #region Constructors

        // Called when created from unmanaged code
        public MainWindow(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }
		
        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MainWindow(NSCoder coder)
            : base(coder)
        {
            Initialize();
        }
		
        // Shared initialization code
        private void Initialize()
        {
            BackgroundColor = NSColor.FromCalibratedRgba(.2f,.2f,.2f,1.0f);

            SetContentSize(new SizeF(800, 700));

            ContentView = Browser;

            Browser.FailedProvisionalLoad += OnBrowserLoadError;
            Browser.MainFrame.LoadRequest(new NSUrlRequest(new NSUrl("http://localhost:8262/artivity/app/journal/1.0/")));
        }

        private void OnBrowserLoadError(object sender, WebFrameErrorEventArgs e)
        {
            // NOTE: This somehow only works the first time. Any subsequent requests to the
            // registered URL fail for an unknown reason.
            string assemblyName = "Artivity.Apid.Modules.Journal";

            // Read the connection error page from the artivity journal module assembly.
            var assembly = Assembly.Load(assemblyName);
            var resourceName = assemblyName + ".app.partials.error-no-apid-connection.html";

            // Read the page into a stream and display it in the browser window.
            using(Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using(StreamReader reader = new StreamReader(stream))
                {
                    string html = reader.ReadToEnd();

                    Browser.MainFrame.LoadHtmlString(html, new NSUrl("http://localhost/"));
                }
            }
        }

        #endregion
    }
}

