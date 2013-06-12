using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;
using Common;
using Models;

namespace Searcher.VM
{
    public enum CurrentAppState
    {
        PendingScanning,
        CountingFiles,
        Scanning,
        ScanCompleted
    }

    public class WndMainVM : ViewModel
    {
        public const string Cancel = "Cancel";
        public const string Next = "Next";
        public const string Prev = "Prev";
        private static DateTime _startScanningTime;
        /// <summary>
        /// Таймер по которому подсчитаевается время сканирования.
        /// Работает только пока панель процесса сканирования активна
        /// </summary>
        private readonly DispatcherTimer _elapsedTimer;
        /// <summary>
        /// Таймер для обновления плагинов в режиме realtime. Работает пока 
        /// активна форма задания настроек поиска.
        /// </summary>
        private readonly DispatcherTimer _pluginCheckTimer;
        private readonly IWndMain _mainWindow;
        private CurrentAppState _currentState;
        private Scan _scan;
        private string _statusBarMessage;

        public WndMainVM(IWndMain wnd)
        {
            _mainWindow = wnd;
            Reset();
            _elapsedTimer = new DispatcherTimer();
            _elapsedTimer.Interval = TimeSpan.FromMilliseconds(100);
            _elapsedTimer.IsEnabled = false;
            _elapsedTimer.Tick += ElapsedTimer_Tick;

            _pluginCheckTimer = new DispatcherTimer();
            _pluginCheckTimer.Interval = TimeSpan.FromSeconds(10);
            _pluginCheckTimer.IsEnabled = true;
            _pluginCheckTimer.Tick += PluginCheckTimer_Tick;
        }

        public ScanSettingsPanelVM SearchOptions
        {
            get { return AppContext.ScanSettings as ScanSettingsPanelVM; }
        }

        public OptionsPanelVM ProgramOptions { get; set; }
        public ActiveScanPanelVM ActivityData { get; set; }

        public CurrentAppState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                RefreshActiveScanPanel();
            }
        }

        public bool IsElapsedTimerOn
        {
            get { return _elapsedTimer.IsEnabled; }
        }

        public ObservableCollection<ScanDataVM> Results
        {
            get { return ActivityData.Results; }
        }

        public string StatusBarMessage
        {
            get { return _statusBarMessage; }
            set
            {
                _statusBarMessage = value;
                OnPropertyChanged("StatusBarMessage");
            }
        }

        #region Events Processing

        private void CountingFiles()
        {
            CurrentState = CurrentAppState.CountingFiles;
            StatusBarMessage = "Counting files in directory..";
        }

        private void ScanStarted()
        {
            _pluginCheckTimer.IsEnabled = false;
            if (!_scan.IsNeedCancelation)
            {
                CurrentState = CurrentAppState.Scanning;
                StatusBarMessage = "Scanning directory..";                
            }
            //ActivityData.FinishedFolderScan(SearchOptions.FolderToScan);
        }

        private void ScanCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusBarMessage = "Scan Complete!";
            CurrentState = CurrentAppState.ScanCompleted;
            //ActivityData.Progress = 100;
            _elapsedTimer.IsEnabled = false;
            if (e.Cancelled)
            {
                _mainWindow.ScanningCanceled();
            }
            else
            {
                _mainWindow.ScanningCompleted();
            }
        }

        private void ProgressChanged(int progress)
        {
            ActivityData.Progress = progress;
        }

        private void FileWasFound(ScanData fileInfo)
        {
            var vmData = new ScanDataVM();
            vmData.Init(fileInfo);
            ActivityData.AddFoundData(vmData);
        }

        private void SubScanCompleted(string subFolderName)
        {
            ActivityData.FinishedFolderScan(subFolderName);
        }

        #endregion

        public void Reset()
        {
            _scan = AppContext.GetObject<Scan>();
            _scan.ScanComplete += ScanCompleted;
            _scan.ProgressChanged += ProgressChanged;
            _scan.SubScanComplete += SubScanCompleted;
            _scan.FileWasFound += FileWasFound;
            _scan.CountingFiles += CountingFiles;
            _scan.ScanStarted += ScanStarted;
            CurrentState = CurrentAppState.PendingScanning;
            StatusBarMessage = "Status: File Scan Pending..";
            if (ActivityData != null)
            {
                ActivityData.Reset();
            }
        }

        public void StartScanning()
        {
            _startScanningTime = DateTime.Now;
            _elapsedTimer.IsEnabled = true;
            ActivityData.Reset();
            _scan.StartScanAsync();
        }

        public void StopScanning()
        {
            StatusBarMessage = "Status: File scan is stopping...";
            _elapsedTimer.IsEnabled = false;
            _scan.CancelProcessAsync();
        }

        public void RemoveResults()
        {
            ScanDataVM[] resultArray = Results.ToArray();
            foreach (ScanDataVM data in resultArray)
            {
                try
                {
                    if (data.Checked)
                    {
                        AppContext.FileSystem.FileDelete(data.FullName);
                        ActivityData.Results.Remove(data);
                    }
                }
                catch (Exception ex)
                {
                    AppContext.Logger.ErrorFormat("During file deleting error occured!{1}{0}",
                                                  ex, Environment.NewLine);
                }
            }
        }

        private void RefreshActiveScanPanel()
        {
            if (ActivityData == null)
            {
                return;
            }
            switch (CurrentState)
            {
                case CurrentAppState.PendingScanning:
                    _pluginCheckTimer.IsEnabled = true;
                    ActivityData.ActionButtonText = Cancel;
                    break;
                case CurrentAppState.CountingFiles:
                case CurrentAppState.Scanning:
                    ActivityData.ActionButtonText = Cancel;
                    break;
                case CurrentAppState.ScanCompleted:
                    if (Results.Count > 0)
                    {
                        ActivityData.ActionButtonText = Next;
                    }
                    else
                    {
                        ActivityData.ActionButtonText = Prev;
                    }
                    break;
            }
        }

        public void CheckAllResults()
        {
            ActivityData.CheckResults(true);
        }

        public void UncheckAllResults()
        {
            ActivityData.CheckResults(false);
        }

        private void ElapsedTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeElapsed = DateTime.Now.Subtract(_startScanningTime);
            ActivityData.TimeElapsed = timeElapsed.TotalSeconds;
        }

        /// <summary>
        /// Событие срабатывает только если активна форма настроек поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PluginCheckTimer_Tick(object sender, EventArgs e)
        {
            AppContext.PluginManager.ScanPluginsFolder();
            SearchOptions.InitPlugins();//reinit plugins in ListBox in UI
        }
    }
}