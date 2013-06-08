using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Common;
using Models;

namespace Searcher.VM
{
    public class WndMainVM
    {
        SearchProcess _regScan;
        private DispatcherTimer _elapsedTimer;
        static DateTime _startScanningTime;

        public WndMainVM()
        {
            _regScan = new SearchProcess();
            _regScan.ScanComplete += ScanCompleted;
            _regScan.ProgressChanged += ProgressChanged;
            _regScan.SubScanComplete += SubScanCompleted;
            _regScan.FileWasFound += FileWasFound;
            _elapsedTimer = new DispatcherTimer();
            _elapsedTimer.Interval = new TimeSpan(1000);
            _elapsedTimer.IsEnabled = false;
            _elapsedTimer.Tick += new EventHandler(ElapsedTimer_Tick);
        }

        public ScanSettingsPanelVM SearchOptions
        {
            get { return AppContext.SearchSettings as ScanSettingsPanelVM; }
        }
        public OptionsPanelVM ProgramOptions { get; set; }
        public ActiveScanPanelVM ActivityData { get; set; }

        public bool IsTimerOn
        {
            get { return _elapsedTimer.IsEnabled; }
        }

        #region Events Processing
        private void ScanCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusBarMessage = "Scan Complete!";
            _elapsedTimer.IsEnabled = false;
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

        public void StartScan()
        {
            _startScanningTime = DateTime.Now;
            _elapsedTimer.IsEnabled = true;
        }

        public void StopScan()
        {
            _elapsedTimer.IsEnabled = false;
        }

        public ObservableCollection<ScanDataVM> Results
        {
            get { return ActivityData.Results; }
        }

        public string StatusBarMessage { get; set; }

        public void RemoveResults()
        {
            foreach (var scanData in Results)
            {
                try
                {
                    AppContext.FileSystem.FileDelete(scanData.FullName);
                }
                catch (Exception ex)
                {
                    AppContext.Logger.ErrorFormat("During file deleting error occured!{1}{0}", 
                        ex, Environment.NewLine);
                }
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
            var _timeElapsed = DateTime.Now.Subtract(_startScanningTime);
            ActivityData.TimeElapsed = _timeElapsed.TotalSeconds;
        }
    }
}
