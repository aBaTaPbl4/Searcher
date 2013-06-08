#region Directives
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Common;
using Models;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using Searcher.Implementation;
using Searcher.Panels;
using Searcher.VM;
using log4net;

#endregion

namespace Searcher
{
    public partial class WndMain : Window
    {

        #region Fields
        ScanSettingsPanel _pnlScanSettings;
        OptionsPanel _pnlOptions;
        HelpPanel _pnlHelp;
        ActiveScanPanel _pnlActiveScan;
        ScanResultsPanel _pnlScanResults;
        WndMainVM _vm;
        BitmapImage _bmpMonitor;
        BitmapImage _bmpOptions;
        BitmapImage _bmpAbout;        
        private ILog _cLog;
        
        #endregion
        
        #region Constructor
        public WndMain()
        {
            InitializeComponent();
            _cLog = AppContext.Logger;
            InitFields();            
        }
        #endregion

         
        #region Overrides

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if (GlassHelper.ExtendGlass(this, 1, 1, 26, 1))
            {
                grdWindowGrid.Margin = new Thickness(1, 26, 1, 1);
                grdNonClient.Visibility = Visibility.Visible;
            }
            else
            {
                grdWindowGrid.Margin = new Thickness(0, 0, 0, 0);
                grdNonClient.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Control Events
        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
            // panel button actions
            Button btn = e.OriginalSource as Button;
            if (btn == null)
                return;

            string url = null;
            switch (btn.Name)
            {
                case "btnScanStart":
                    if ((string)btn.Content == "Start")
                    {
                        if (_pnlScanSettings.AllDataValid())
                        {
                            LogEntry("Scan Started...");
                            ScanStart();
                        }
                        else
                        {
                            MessageBox.Show("Some data is not valid. Folder path is correct?", "Validation Errors",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                    break;
                case "btnScanCancel":
                    if ((string)btn.Content == "Cancel")
                    {
                        LogEntry("Scan Aborted by User.");
                        if (_vm.IsTimerOn)
                        {
                            // cancel out
                            ScanCancel();
                        }
                    }
                    else
                    {
                        ToggleResultsPanel();
                    }
                    break;
                case "btnSelectAll":
                    _vm.CheckAllResults();
                    break;
                case "btnDeselectAll":
                    _vm.UncheckAllResults();
                    break;
                case "btnRemove":
                    _vm.RemoveResults();
                    break;
                case "btnShowLog":
                    OpenLogInNotepad();
                    break;
                case "btnHelpMain":
                    OpenUrlInBrowser("http://yandex.ru");
                    break;
                case "btnHelpHome":
                    OpenUrlInBrowser("http://google.ru");
                    break;
                case "btnAbout":
                    ShowAboutWindow();
                    break;                    
            }
            
        }

        private static void OpenLogInNotepad()
        {
            var logFileName = "log.txt";
            if (AppContext.FileSystem.FileExtists(logFileName))
            {
                Process.Start("notepad.exe", logFileName);
            }
        }

        private void OpenUrlInBrowser(string url)
        {
            Process.Start("iexplore.exe", url);
        }

        private void Link_Click(object sender, MouseButtonEventArgs e)
        {
            Image lnk = (Image)sender;

            if (lnk.Name == "imgAbout" )
            {
                // load about form
                ShowAboutWindow();
            }
        }

        private static void ShowAboutWindow()
        {
            WndAbout w = new WndAbout();
            w.ShowDialog();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = (ToggleButton)sender;
            if (!tb.IsChecked == true)
            {
                tb.IsChecked = true;
                return;
            }
            ToggleToolBarButtons(tb.Name);
            TogglePanels(tb.Name);
        }
        #endregion

        #region Helpers

        private void InitFields()
        {
            _pnlScanSettings = new ScanSettingsPanel();
            _pnlOptions = new OptionsPanel();
            _pnlHelp = new HelpPanel();
            _pnlActiveScan = new ActiveScanPanel();
            _pnlScanResults = new ScanResultsPanel(_pnlActiveScan.ViewModel);
            _vm = new WndMainVM();
            _vm.ActivityData = _pnlActiveScan.ViewModel;
            _vm.ProgramOptions = _pnlOptions.ViewModel;
            _bmpMonitor = new BitmapImage(new Uri("/Images/monitor.png", UriKind.Relative));
            _bmpOptions = new BitmapImage(new Uri("/Images/options.png", UriKind.Relative));
            _bmpAbout = new BitmapImage(new Uri("/Images/about.png", UriKind.Relative));
            

            btnRegscan.IsChecked = true;
            // add the panels
            grdContainer.Children.Add(_pnlActiveScan);
            grdContainer.Children.Add(_pnlScanResults);
            grdContainer.Children.Add(_pnlOptions);
            grdContainer.Children.Add(_pnlHelp);
        }

        private void UnLockControls(bool unlocked)
        {
            this.stkToolBarPanel.IsEnabled = unlocked;
            this.grdNonClient.IsEnabled = unlocked;
        }


        private void LogEntry(string entry)
        {
            _cLog.Info(entry);
        }

        private void Reset()
        {
            _vm.StopScan();
            _vm.ActivityData.Reset();
        }


        private void ScanCancel()
        {
            Reset();
            UnLockControls(true);
            ToggleScanSettingsButton();
            ToggleScanSettingsPanel();
        }

        private void ScanComplete(int items)
        {
            UnLockControls(true);
//            _pnlActiveScan.btnRegScanCancel.Content = "Status: Registry Scan Completed.. " + items.ToString() + " removed..";
            ToggleScanSettingsButton();
            ToggleScanSettingsPanel();
        }

        private void ScanStart()
        {
            
            UnLockControls(false);            
            ToggleActiveScanPanel();
            _vm.StartScan();
        }

        private void ScanStop()
        {
            //ResetData();
            _vm.StopScan();
            UnLockControls(true);
            _pnlActiveScan.btnScanCancel.Content = "Next";
        }

        private void ToggleResultsPanel()
        {
            TogglePanels("Results");
        }

        private void ToggleScanSettingsPanel()
        {
            TogglePanels("btnRegscan");
        }

        private void ToggleActiveScanPanel()
        {
            TogglePanels("Active");
        }

        private void TogglePanels(string name)
        {
            // reset panel visibility
            foreach (UIElement pnl in grdContainer.Children)
            {
                pnl.Visibility = Visibility.Collapsed;
            }
            // toggle visible panel
            switch (name)
            {
                case "btnRegscan":
                    if (_vm.Results.Count > 0)
                    {
                        _pnlScanResults.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _pnlScanSettings.Visibility = Visibility.Visible;
                        imgStatusBar.Source = _bmpMonitor;
                        txtStatusBar.Text = "Status: Scan Pending..";
                    }
                    break;
                case "Active":
                    _pnlActiveScan.Visibility = Visibility.Visible;
                    imgStatusBar.Source = _bmpMonitor;
                    txtStatusBar.Text = "Status: Scanning..";
                    break;
                case "Results":
                    _pnlScanResults.Visibility = Visibility.Visible;
                    imgStatusBar.Source = _bmpMonitor;
                    txtStatusBar.Text = "Status: Scan Completed!";
                    break;
                case "btnOptions":
                    _pnlOptions.Visibility = Visibility.Visible;
                    imgStatusBar.Source = _bmpOptions;
                    txtStatusBar.Text = "Review Scanning Options..";
                    break;
                case "btnHelp":
                    _pnlHelp.Visibility = Visibility.Visible;
                    imgStatusBar.Source = _bmpAbout;
                    txtStatusBar.Text = "Review Help Information..";
                    break;
            }
        }

        private void ToggleScanSettingsButton()
        {
            ToggleToolBarButtons("btnRegscan");
        }

        private void ToggleToolBarButtons(string button)
        {
            foreach (Object o in this.stkToolBarPanel.Children)
            {
                if (o.GetType() == typeof(ToggleButton))
                {
                    ToggleButton t = o as ToggleButton;
                    if (t.Name != button)
                    {
                        t.IsChecked = false;
                    }
                    else
                    {
                        t.IsChecked = true;
                    }
                }
            }
        }
        #endregion
    }
}
