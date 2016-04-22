
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.WebKit;

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
        void Initialize()
        {
            ContentView = Browser;

            Browser.MainFrame.LoadRequest(new NSUrlRequest(new NSUrl("http://localhost:8262/artivity/app/journal/1.0/")));

            SetContentSize(new SizeF(800, 700));
        }

        #endregion
    }
}

