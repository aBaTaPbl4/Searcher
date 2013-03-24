using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Common;
using Searcher.VM;

namespace ScanX.Panels
{
    /// <summary>
    /// Interaction logic for RegScanPanel.xaml
    /// </summary>
    public partial class RegScanPanel : UserControl
    {
        private readonly ObservableCollection<PluginDecorator> _plugins;

        public RegScanPanel()
        {
            InitializeComponent();
            _plugins = new ObservableCollection<PluginDecorator>();
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                foreach (var plugin in AppContext.PluginManager.ExternalPlugins)
                {
                    _plugins.Add(new PluginDecorator()
                    {
                        IsActive = false,
                        Plugin = plugin
                    });
                }                
            }

        }

        public ObservableCollection<PluginDecorator> Plugins
        {
            get { return _plugins; }
        }

        private void Start_Clicked(object sender, RoutedEventArgs e)
        {
            //
        }

        public bool IsNeedToScanControls
        {
            get
            {
                return false;
            }
        }

        public bool IsNeedHardClean
        {
            get
            {
                return  false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rbnOR.IsChecked = true;
            //txtFileContent.IsEnabled = true;
            //txtFileName.IsEnabled = true;
            DataContext = this;
        }

    }
}
