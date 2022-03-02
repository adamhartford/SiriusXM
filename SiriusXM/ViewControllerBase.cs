using System;
using AppKit;

namespace SiriusXM
{
    public abstract class ViewControllerBase : NSViewController
    {
        protected AppDelegate AppDelegate => (AppDelegate)NSApplication.SharedApplication.Delegate;

        protected ViewControllerBase(IntPtr handle) : base(handle)
        {
        }

        protected void ShowAlert(string message)
        {
            var alert = new NSAlert { MessageText = message };
            alert.AddButton("OK");
            alert.BeginSheet(View.Window);
        }
    }
}