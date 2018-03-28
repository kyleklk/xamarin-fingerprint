// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SMS.Fingerprint.Sample.Mac
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton btnAdd { get; set; }

		[Outlet]
		AppKit.NSButton btnAuthenticate { get; set; }

		[Outlet]
		AppKit.NSButton btnRemove { get; set; }

		[Outlet]
		AppKit.NSTextField lblStatus { get; set; }

		[Outlet]
		AppKit.NSButton swAutoCancel { get; set; }

		[Action ("AddClicked:")]
		partial void AddClicked (AppKit.NSButton sender);

		[Action ("AuthenticateClicked:")]
		partial void AuthenticateClicked (AppKit.NSButton sender);

		[Action ("AuthenticateLocalizedClicked:")]
		partial void AuthenticateLocalizedClicked (AppKit.NSButton sender);

		[Action ("RemoveClicked:")]
		partial void RemoveClicked (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnAuthenticate != null) {
				btnAuthenticate.Dispose ();
				btnAuthenticate = null;
			}

			if (btnRemove != null) {
				btnRemove.Dispose ();
				btnRemove = null;
			}

			if (btnAdd != null) {
				btnAdd.Dispose ();
				btnAdd = null;
			}

			if (lblStatus != null) {
				lblStatus.Dispose ();
				lblStatus = null;
			}

			if (swAutoCancel != null) {
				swAutoCancel.Dispose ();
				swAutoCancel = null;
			}
		}
	}
}
