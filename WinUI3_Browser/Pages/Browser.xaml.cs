using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using WinUI3_Browser.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_Browser.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Browser : Page
    {
        public ObservableCollection<TabViewItem> tabViewItems;

        public Browser()
        {
            tabViewItems = new ObservableCollection<TabViewItem>();
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddNewTab();
        }

        private void AddNewTab()
        {
            TabViewItem tab = new TabViewItem()
            {
                Header = new BrowserTabHeader("New Tab"),
                IconSource = new SymbolIconSource() { Symbol = Symbol.Document }
            };
            WebViewWithNavigationBar webView = new WebViewWithNavigationBar(tab);
            tab.Content = webView;
            tabViewItems.Add(tab);
            tabView.SelectedItem = tab;
        }

        private void CloseTab(TabViewItem tab)
        {
            tabViewItems.Remove(tab);
            if(tabViewItems.Count == 0)
            {
                Exit();
            }
        }

        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            CloseTab(args.Tab);
        }

        private void TabView_AddTabButtonClick(TabView sender, object args)
        {
            AddNewTab();
        }

        private void Exit()
        {
            Application.Current.Exit();
        }
    }
}
