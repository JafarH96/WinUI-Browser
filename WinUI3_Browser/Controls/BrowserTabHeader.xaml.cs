using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_Browser.Controls
{
    public sealed partial class BrowserTabHeader : UserControl
    {
        public BrowserTabHeader(string title)
        {
            this.InitializeComponent();
            HeaderText.Text = title;
        }

        public void SetHeaderTitle(string title, bool indicator)
        {
            HeaderText.Text = title;
            SetIndicatorState(indicator);
        }

        public void SetIndicatorState(bool active)
        {
            LoadingIndicator.IsActive = active;
            LoadingIndicator.Visibility = active ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
