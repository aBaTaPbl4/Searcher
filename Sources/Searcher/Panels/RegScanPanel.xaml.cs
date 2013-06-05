using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Common;
using Searcher.VM;

namespace Searcher.Panels
{
    /// <summary>
    /// Interaction logic for RegScanPanel.xaml
    /// </summary>
    public partial class RegScanPanel : UserControl
    {
        private readonly ScanOptionVM _data;

        public RegScanPanel()
        {
            InitializeComponent();
            _data = new ScanOptionVM();
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                _data.InitPlugins();
            }
            

            DataContext = _data;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rbnOR.IsChecked = true;
        }

        private void btnRegScanStart_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
