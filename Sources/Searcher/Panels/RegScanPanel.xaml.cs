using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Searcher.VM;
using UserControl = System.Windows.Controls.UserControl;

namespace Searcher.Panels
{
    /// <summary>
    /// Interaction logic for RegScanPanel.xaml
    /// </summary>
    public partial class RegScanPanel : UserControl
    {
        private readonly ScanPanelVM _data;

        public RegScanPanel()
        {
            InitializeComponent();
            _data = new ScanPanelVM();
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                _data.InitPlugins();
            }
            

            DataContext = _data;
        }

        private void btnScanStart_Click(object sender, RoutedEventArgs e)
        {
            int i = 1;
            i++;
        }

        private void btnChoseFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = dialog.SelectedPath;
            }
        }

        private void lstPlugins_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itm = lstPlugins.SelectedItem as PluginDecoratorVM;
            _data.SetActivePlugin(itm);
        }

    }
}
