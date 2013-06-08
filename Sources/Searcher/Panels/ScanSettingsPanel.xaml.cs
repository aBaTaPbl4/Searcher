using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Searcher.VM;
using Common;
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
            _data = AppContext.SearchSettings as ScanSettingsPanelVM;
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                _data.InitPlugins();
            }
            DataContext = _data;
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

        public ScanSettingsPanelVM ViewModel
        {
            get { return _data; }
        }

        public bool AllDataValid()
        {
            return this.IsValid() && !_data.FolderToScan.IsNullOrEmpty();
        }

    }
}
