// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;

namespace SiriusXM
{
	public partial class PreferencesViewController : ViewControllerBase
	{
		public PreferencesViewController (IntPtr handle) : base (handle)
		{
		}

		partial void EnableNotificationsChanged(NSObject sender)
		{
			var enabled = ((NSButton)sender).State == NSCellStateValue.On;
			if (enabled)
			{
                var notification = new NSUserNotification
                {
                    Title = "Notifications enabled"
                };
                NSUserNotificationCenter.DefaultUserNotificationCenter.DeliverNotification(notification);
			}
		}
	}
}
