using System.Windows.Controls;
using CircularProgressBar;
using Searcher.VM;

namespace Searcher.Panels
{
    /// <summary>
    /// Interaction logic for TopRightPanel.xaml
    /// </summary>
    public partial class RegScanActivePanel : UserControl
    {
        private ScanActivePanelVM _data;

        public RegScanActivePanel()
        {
            InitializeComponent();
            _data = new ScanActivePanelVM();
            DataContext = _data;
        }

        public ScanActivePanelVM ViewModel
        {
            get { return _data; }
        }
    }
}
