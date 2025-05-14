using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Web.WebView2.Core;
using WinUI3_Browser.Utilities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_Browser.Controls
{
    public sealed partial class WebViewWithNavigationBar : UserControl
    {
        enum RefreshButtonState
        {
            Refresh,
            Stop
        }

        private string url;
        private readonly TabViewItem tab;

        private RefreshButtonState refreshButtonState = RefreshButtonState.Refresh;

        public WebView2 WebView
        {
            get { return webView; }
        }

        public TextBox URLBox
        {
            get { return urlBox; }
        }

        public string InitURL
        {
            get { return url; }
        }

        public WebViewWithNavigationBar(TabViewItem tab, string initURL = "about:blank")
        {
            this.url = initURL;
            this.tab = tab;
            this.InitializeComponent();
            SetupWebView();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private async void SetupWebView()
        {
            webView.CoreWebView2Initialized += OnCoreWebView2Initialized;
            webView.NavigationStarting += OnNavigationStarted;
            webView.NavigationCompleted += OnNavigationCompleted;
            await webView.EnsureCoreWebView2Async();
            webView.CoreWebView2.Navigate(url);
        }

        private void UpdateTabTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                if (URLHelpers.IsBlankUrl(title))
                {
                    ((BrowserTabHeader)tab.Header).SetHeaderTitle("New Tab", false);
                }
                else
                {
                    ((BrowserTabHeader)tab.Header).SetHeaderTitle(title, false);
                }
            }
        }

        private async void UpdateFavIcon(CoreWebView2 sender)
        {
            var favicon = await sender.GetFaviconAsync(CoreWebView2FaviconImageFormat.Png);
            if (favicon == null) return;

            using (favicon)
            {
                var bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(favicon);
                var imageIcon = new ImageIconSource { ImageSource = bitmapImage };
                tab.IconSource = imageIcon;
            }
        }

        private void NavigateTo(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) { return; }

            string urlWithHttps = url;
            if (!URLHelpers.IsUrl(url))
            {
                urlWithHttps = URLHelpers.CreateGoogleSearchURL(url);
            }
            else if (!url.StartsWith("https://") || !url.StartsWith("http://") || !url.StartsWith("www."))
            {
                urlWithHttps = "https://" + url;
            }
            webView.CoreWebView2.Navigate(urlWithHttps);
        }

        private void SetRefreshButtonState(RefreshButtonState state)
        {
            refreshButtonState = state;
            if(state == RefreshButtonState.Refresh)
            {
                RefreshButtonSymbol.Symbol = Symbol.Refresh;
            }
            else
            {
                RefreshButtonSymbol.Symbol = Symbol.Clear;
            }
        }

        private void HandleRefreshButtonState()
        {
            if (refreshButtonState == RefreshButtonState.Refresh)
            {
                MakeWebViewFocused();
                webView.Reload();
            }
            else
            {
                webView.CoreWebView2.Stop();
            }
        }

        private void OnNavigationStarted(WebView2 sender, CoreWebView2NavigationStartingEventArgs args)
        {
            ((BrowserTabHeader)tab.Header).SetIndicatorState(true);
            SetRefreshButtonState(RefreshButtonState.Stop);
        }

        private void OnNavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
        {
            ((BrowserTabHeader)tab.Header).SetIndicatorState(false);
            SetRefreshButtonState(RefreshButtonState.Refresh);
        }

        private void OnCoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            sender.CoreWebView2.DocumentTitleChanged += OnPageTitleChanged;
            sender.CoreWebView2.FaviconChanged += OnFavIconChanged;
        }

        private void OnPageTitleChanged(CoreWebView2 sender, object args)
        {
            UpdateTabTitle(sender.DocumentTitle);
        }

        private void OnFavIconChanged(CoreWebView2 sender, object args)
        {
            UpdateFavIcon(sender);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (webView.CanGoBack)
            {
                MakeWebViewFocused();
                webView.GoBack();
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (webView.CanGoForward)
            {
                MakeWebViewFocused();
                webView.GoForward();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            HandleRefreshButtonState();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (webView.CoreWebView2 != null)
            {
                MakeWebViewFocused();
                webView.CoreWebView2.Navigate(url);
            }
        }

        private void MakeWebViewFocused()
        {
            if (webView.FocusState == FocusState.Unfocused)
            {
                webView.Focus(FocusState.Programmatic);
            }
        }

        private void urlBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if(sender is TextBox urlbox)
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    NavigateTo(urlbox.Text);
                }
            }
        }

        private void urlBox_GotFocus(object sender, RoutedEventArgs e)
        {
            urlBox.SelectAll();
        }
    }
}
