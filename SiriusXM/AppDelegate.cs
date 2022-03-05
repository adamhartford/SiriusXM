using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppKit;
using Foundation;

namespace SiriusXM
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate, INSMenuValidation
    {
        public ViewController ViewController { get; set; }

        private PlayerState _playerState;
        public PlayerState PlayerState
        {
            set
            {
                _playerState = value;
                HandlePlayerStateChanged();
            }
        }
        
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
            ViewController?.RunJs("unofficialClient.playPause();");
        }
        
        [Action("ClickedSkipForward:")]
        public void ClickedSkipForward(NSObject sender)
        {
            ViewController?.RunJs("unofficialClient.skipForward();");
        }
        
        [Action("ClickedSkipBack:")]
        public void ClickedSkipBack(NSObject sender)
        {
            ViewController?.RunJs("unofficialClient.skipBack();");
        }
        
        [Action("ClickedHome:")]
        public void ClickedHome(NSObject sender)
        {
            ViewController.Browser.LoadRequest(new NSUrlRequest(new NSUrl("https://player.siriusxm.com")));
        }
        
        [Action("ClickedReload:")]
        public void ClickedReload(NSObject sender)
        {
            ViewController?.RunJs("location.reload();");
        }
        
        [Action("ClickedBack:")]
        public void ClickedBack(NSObject sender)
        {
            ViewController?.RunJs("history.back();");
        }
        
        [Action("ClickedForward:")]
        public void ClickedForward(NSObject sender)
        {
            ViewController?.RunJs("history.forward();");
        }

        public override NSMenu ApplicationDockMenu(NSApplication sender)
        {
            return PlaybackMenu;
        }

        public bool ValidateMenuItem(NSMenuItem menuItem)
        {
            return true;
        }

        private void HandlePlayerStateChanged()
        {
            if (string.IsNullOrEmpty(_playerState.ArtistName) || string.IsNullOrEmpty(_playerState.TrackName))
                return;
            
            var defaults = NSUserDefaults.StandardUserDefaults;
            if (defaults.BoolForKey("EnableNotifications"))
            {
                var notification = new NSUserNotification
                {
                    Title = _playerState.TrackName,
                    Subtitle = _playerState.ArtistName
                };
                NSUserNotificationCenter.DefaultUserNotificationCenter.DeliverNotification(notification);
            }
        }
    }
}
