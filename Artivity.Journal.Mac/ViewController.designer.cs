// WARNING
//
// This file has been generated automatically by Xamarin Studio Community to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Artivity.Journal.Mac
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
        BrowserView Browser { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Browser != null) {
				Browser.Dispose ();
				Browser = null;
			}
		}
	}
}
