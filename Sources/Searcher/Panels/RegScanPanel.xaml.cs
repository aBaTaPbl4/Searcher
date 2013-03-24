using System.Windows;
using System.Windows.Controls;

namespace ScanX.Panels
{
    /// <summary>
    /// Interaction logic for RegScanPanel.xaml
    /// </summary>
    public partial class RegScanPanel : UserControl
    {
        public RegScanPanel()
        {
            InitializeComponent();
        }

        private void Start_Clicked(object sender, RoutedEventArgs e)
        {
            //
        }

        public bool IsNeedToScanControls
        {
            get
            {
                return chkControlScan.IsChecked ?? false;
            }
        }

        public bool IsNeedHardClean
        {
            get
            {
                return chkHardControlClean.IsChecked ?? false;
            }
        }

        private void chkControlScan_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsNeedToScanControls)
            {
                chkHardControlClean.IsChecked = false;
                chkOnlyCleanComFromConfig.IsChecked = false;
            }
            chkHardControlClean.IsEnabled = IsNeedToScanControls;
            chkOnlyCleanComFromConfig.IsEnabled = IsNeedHardClean;
        }

        private void chkHardControlClean_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsNeedHardClean)
            {
                chkOnlyCleanComFromConfig.IsChecked = false;
            }
            chkOnlyCleanComFromConfig.IsEnabled = IsNeedHardClean;
        }

        private void chkOnlyComFromConfig_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Checkbox_Checked(object sender, RoutedEventArgs e)
        {
           // CheckBox c = (CheckBox)sender;
           // c.IsChecked = !c.IsChecked;
        }
    }
}
