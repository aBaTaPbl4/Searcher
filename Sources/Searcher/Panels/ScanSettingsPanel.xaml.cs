using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Common;
using Searcher.VM;
using UserControl = System.Windows.Controls.UserControl;

namespace Searcher.Panels
{
    /// <summary>
    /// Interaction logic for ScanSettingsPanel.xaml
    /// </summary>
    public partial class ScanSettingsPanel : UserControl
    {
        private readonly ScanSettingsPanelVM _data;

        public ScanSettingsPanel()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _data = AppContext.GetObject<ScanSettingsPanelVM>();
                _data.InitPlugins();
            }
            DataContext = _data;
        }

        public ScanSettingsPanelVM ViewModel
        {
            get { return _data; }
        }

        private void btnChoseFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
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

        public bool AllDataValid()
        {
            return this.IsValid() && !_data.FolderToScan.IsNullOrEmpty();
        }
    }
}