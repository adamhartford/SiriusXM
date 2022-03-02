// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SiriusXM
{
	[Register ("PreferencesViewController")]
	partial class PreferencesViewController
	{
		[Outlet]
		AppKit.NSButton EnableNotifications { get; set; }

		[Action ("EnableNotificationsChanged:")]
		partial void EnableNotificationsChanged (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (EnableNotifications != null) {
				EnableNotifications.Dispose ();
				EnableNotifications = null;
			}
		}
	}
}
