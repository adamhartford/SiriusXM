using System;
using System.IO;
using AppKit;
using Foundation;
using Newtonsoft.Json;
using WebKit;

namespace SiriusXM
{
    public partial class ViewController : ViewControllerBase, IWKNavigationDelegate, IWKUIDelegate, IWKScriptMessageHandler
    {
        public WKWebView Browser { get; private set; }

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var config = new WKWebViewConfiguration();
            config.Preferences.SetValueForKey(FromObject(true), new NSString("developerExtrasEnabled"));

            var contentController = new WKUserContentController();
            contentController.AddScriptMessageHandler(this, "playerState");
            config.UserContentController = contentController;

            Browser = new WKWebView(View.Frame, config);
            Browser.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
            Browser.NavigationDelegate = this;
            Browser.UIDelegate = this;

            // Using Edge for Mac user agent because Safari user agent doesn't work here with SiriusXM player.
            Browser.CustomUserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.82 Safari/537.36 Edg/98.0.1108.51";
            
            Browser.LoadRequest(new NSUrlRequest(new NSUrl("https://player.siriusxm.com")));
            View.AddSubview(Browser);

            AppDelegate.ViewController = this;
        }
        
        public void RunJs(string js, WKJavascriptEvaluationResult completionHandler = null)
        {
            Browser.EvaluateJavaScript(js, (res, err) =>
            {
                completionHandler?.Invoke(res, err);
            });
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
            RunJs(js);
        }

        [Export("webView:runJavaScriptAlertPanelWithMessage:initiatedByFrame:completionHandler:")]
        public virtual void RunJavaScriptAlertPanel(WKWebView webView, string message, WKFrameInfo frame, Action completionHandler)
        {
            ShowAlert(message);
            completionHandler();
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            if (message.Name == "playerState")
            {
                var state = JsonConvert.DeserializeObject<PlayerState>(message.Body.ToString());
                AppDelegate.PlayerState = state;
            }
        }
    }
}
