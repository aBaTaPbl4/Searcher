using System.Diagnostics;
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
        private OptionsPanelVM _data;

        public OptionsPanel()
        {
            InitializeComponent();
            _data = new OptionsPanelVM();
            DataContext = _data;
        }

        private void Chk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox chk = e.OriginalSource as CheckBox;
            if (chk == null)
                return;
            if (chk.Name == "chkAsync")
            {
                txtThreadsNum.IsEnabled = chk.IsChecked ?? false;
                
            }
        }

        private void btnShowLog_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            var logFileName = "log.txt";
            if (AppContext.FileSystem.FileExtists(logFileName))
            {
                Process.Start("notepad.exe", logFileName);
            }
        }

    }
}
