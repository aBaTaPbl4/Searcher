using System.Windows;

namespace Searcher
{
    /// <summary>
    /// Interaction logic for wndAbout.xaml
    /// </summary>
    public partial class WndAbout : Window
    {
        public WndAbout()
        {
            InitializeComponent();
            // System.Windows.Controls.Primitives.Thumb.
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}