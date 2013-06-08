using System.Diagnostics;
using System.Windows.Controls;
using Common;

namespace Searcher.Panels
{
    /// <summary>
    /// Interaction logic for DefaultPanel.xaml
    /// </summary>
    public partial class HelpPanel : UserControl
    {
        public HelpPanel()
        {
            InitializeComponent();
        }

        private void GoToPage_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
                return;
            string url;
            if (btn.Name == "btnHelpMain")
            {
                url = "http://yandex.ru";
            }
            else
            {
                url = "http://google.ru";
            }
            Process.Start("iexplore.exe", url);
        }

        private void btnAbout_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = new WndAbout();
            wnd.Show();
        }
    }
}
