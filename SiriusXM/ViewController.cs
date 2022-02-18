using System;
using System.IO;
using AppKit;
using Foundation;
using WebKit;

namespace SiriusXM
{
    public partial class ViewController : NSViewController, IWKNavigationDelegate, IWKUIDelegate
    {
        private WKWebView _browser;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var config = new WKWebViewConfiguration();
            config.Preferences.SetValueForKey(FromObject(true), new NSString("developerExtrasEnabled"));

            _browser = new WKWebView(View.Frame, config);
            _browser.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
            _browser.NavigationDelegate = this;
            _browser.UIDelegate = this;

            // Using Edge for Mac user agent because Safari user agent doesn't work here with SiriusXM player.
            _browser.CustomUserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.82 Safari/537.36 Edg/98.0.1108.51";

            _browser.LoadRequest(new NSUrlRequest(new NSUrl("https://player.siriusxm.com")));
            View.AddSubview(_browser);

            var appDelegate = (AppDelegate)NSApplication.SharedApplication.Delegate;
            appDelegate.ViewController = this;
        }
        
        public void RunJS(string js)
        {
            _browser.EvaluateJavaScript(js, null);
        }

        [Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
        public void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            if (navigationAction.TargetFrame == null)
            {
                decisionHandler(WKNavigationActionPolicy.Cancel);
                NSWorkspace.SharedWorkspace.OpenUrl(navigationAction.Request.Url);
                return;
            }
            
            decisionHandler(WKNavigationActionPolicy.Allow);
        }

        [Export("webView:didFinishNavigation:")]
        public void DidFinishNavigation(WebKit.WKWebView webView, WebKit.WKNavigation navigation)
        {
            var path = NSBundle.MainBundle.PathForResource("unofficialClient", "js");
            var js = File.ReadAllText(path);
            RunJS(js);
        }
    }
}
