using System;
using AppKit;
using Foundation;

namespace SiriusXM
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        public ViewController ViewController { get; set; }
        
        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        [Export("applicationShouldHandleReopen:hasVisibleWindows:")]
        public override bool ApplicationShouldHandleReopen(NSApplication sender, bool hasVisibleWindows)
        {
            if (!hasVisibleWindows)
            {
                foreach (var window in sender.DangerousWindows)
                {
                    window.MakeKeyAndOrderFront(this);
                }
            }

            return true;
        }

        [Action("ClickedPlayPause:")]
        public void ClickedPlayPause(NSObject sender)
        {
            ViewController?.RunJS("unofficialClient.playPause();");
        }
        
        [Action("ClickedSkipForward:")]
        public void ClickedSkipForward(NSObject sender)
        {
            ViewController?.RunJS("unofficialClient.skipForward();");
        }
        
        [Action("ClickedSkipBack:")]
        public void ClickedSkipBack(NSObject sender)
        {
            ViewController?.RunJS("unofficialClient.skipBack();");
        }
    }
}
