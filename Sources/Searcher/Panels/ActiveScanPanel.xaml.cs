using System.Windows.Controls;
using Searcher.VM;

namespace Searcher.Panels
{
    /// <summary>
    /// Interaction logic for TopRightPanel.xaml
    /// </summary>
    public partial class ActiveScanPanel : UserControl
    {
        private readonly ActiveScanPanelVM _data;

        public ActiveScanPanel()
        {
            InitializeComponent();
            _data = new ActiveScanPanelVM();
            DataContext = _data;
        }

        public ActiveScanPanelVM ViewModel
        {
            get { return _data; }
        }
    }
}