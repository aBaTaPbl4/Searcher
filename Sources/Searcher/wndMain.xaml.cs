#region Directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using Common;
using Searcher.Implementation;
using Searcher.Panels;
using Searcher.VM;
using log4net;

#endregion

namespace Searcher
{
    public partial class WndMain : Window, IWndMain
    {
        #region Fields

        private readonly ILog _logger;
        private BitmapImage _bmpAbout;
        private BitmapImage _bmpMonitor;
        private BitmapImage _bmpOptions;
        private ActiveScanPanel _pnlActiveScan;
        private HelpPanel _pnlHelp;
        private OptionsPanel _pnlOptions;
        private ScanResultsPanel _pnlScanResults;
        private ScanSettingsPanel _pnlScanSettings;
        private WndMainVM _vm;

        #endregion

        #region Constructor

        public WndMain()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
            _logger = AppContext.Logger;
            InitFields();
            DataContext = _vm;
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
            var btn = e.OriginalSource as Button;
            if (btn == null)
                return;

            string url = null;
            switch (btn.Name)
            {
                case "btnScanStart":
                    if ((string) btn.Content == "Start")
                    {
                        if (_pnlScanSettings.AllDataValid())
                        {
                            LogInfo("Scan Started...");
                            StartScanning();
                        }
                        else
                        {
                            MessageBox.Show("Some data is not valid. Folder path is correct?", "Validation Errors",
                                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                    break;
                case "btnScanCancel":
                    var btnContent = (string) btn.Content;

                    if (btnContent == WndMainVM.Next)
                    {
                        ToggleResultsPanel();
                    }
                    else if (btnContent == WndMainVM.Prev)
                    {
                        ToggleScanSettingsPanel();
                    }
                    else
                    {
                        LogInfo("Scan Aborted by User.");
                        StopScanning();
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
            string logFileName = "log.txt";
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
            var lnk = (Image) sender;

            if (lnk.Name == "imgAbout")
            {
                // load about form
                ShowAboutWindow();
            }
        }

        private static void ShowAboutWindow()
        {
            var w = new WndAbout();
            w.ShowDialog();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var tb = (ToggleButton) sender;
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

        public void ScanningCompleted()
        {
            _pnlActiveScan.btnScanCancel.IsEnabled = true;
            UnLockControls(true);
        }

        public void ScanningCanceled()
        {
            _pnlActiveScan.btnScanCancel.IsEnabled = true;
            UnLockControls(true);
        }

        private void InitFields()
        {
            //панель уже добавлена в разметке
            _pnlScanSettings = (ScanSettingsPanel) grdContainer.Children[0];
            _pnlOptions = new OptionsPanel();
            _pnlHelp = new HelpPanel();
            _pnlActiveScan = new ActiveScanPanel();
            _pnlScanResults = new ScanResultsPanel(_pnlActiveScan.ViewModel);
            _vm = new WndMainVM(this);
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
            stkToolBarPanel.IsEnabled = unlocked;
            grdNonClient.IsEnabled = unlocked;
        }


        private void LogInfo(string entry)
        {
            _logger.Info(entry);
        }

        public void StopScanning()
        {
            //var stoppingProgressWindow = ShowProgress();
            _pnlActiveScan.btnScanCancel.IsEnabled = false;
            _vm.StopScanning();
            UnLockControls(true);
            //stoppingProgressWindow.Close();        
        }

        private ProgressBarWindow ShowProgress()
        {
            var progressWindow = new ProgressBarWindow();
            progressWindow.Owner = this;
            progressWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            progressWindow.Show();
            return progressWindow;
        }

        public void ToggleScanSettingsPanel()
        {
            _vm.Reset();
            ToggleScanSettingsButton();
            TogglePanels("btnRegscan");
        }

        private void StartScanning()
        {
            UnLockControls(false);
            ToggleActiveScanPanel();
            _vm.StartScanning();
        }

        private void ToggleResultsPanel()
        {
            TogglePanels("Results");
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
                    _pnlScanSettings.Visibility = Visibility.Visible;
                    imgStatusBar.Source = _bmpMonitor;
                    txtStatusBar.Text = "Status: Scan Pending..";
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
            foreach (Object o in stkToolBarPanel.Children)
            {
                if (o.GetType() == typeof (ToggleButton))
                {
                    var t = o as ToggleButton;
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