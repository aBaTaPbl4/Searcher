using System.Windows;
using System.Windows.Controls;
using Common;
using Searcher.VM;

namespace Searcher.Panels
{
    /// <summary>
    /// Interaction logic for TopLeftPanel.xaml
    /// </summary>
    public partial class OptionsPanel : UserControl
    {
        private readonly OptionsPanelVM _data;

        public OptionsPanel()
        {
            InitializeComponent();
            _data = AppContext.GetObject<OptionsPanelVM>();
            DataContext = _data;
        }


        public OptionsPanelVM ViewModel
        {
            get { return _data; }
        }

        private void Chk_Click(object sender, RoutedEventArgs e)
        {
            var chk = e.OriginalSource as CheckBox;
            if (chk == null)
                return;
            if (chk.Name == "chkAsync")
            {
                txtThreadsNum.IsEnabled = chk.IsChecked ?? false;
            }
        }
    }
}