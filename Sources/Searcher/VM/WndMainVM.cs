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
            //_regScan.CurrentPath += new SearchProcess.CurrentPathDelegate(RegScan_CurrentPath);
            //_regScan.KeyCount += new SearchProcess.KeyCountDelegate(RegScan_KeyCount);
            //_regScan.LabelChange += new SearchProcess.LabelChangeDelegate(RegScan_LabelChange);
            //_regScan.MatchItem += new SearchProcess.MatchItemDelegate(RegScan_MatchItem);
            //_regScan.ProcessChange += new SearchProcess.ProcessChangeDelegate(RegScan_ProcessChange);
            _regScan.ProcessCompleted += new SearchProcess.ProcessCompletedEventHandler(RegScan_ProcessCompleted);
            _regScan.ScanComplete += new SearchProcess.ScanCompleteDelegate(RegScan_ScanComplete);
            _regScan.SubScanComplete += new SearchProcess.SubScanCompleteDelegate(RegScan_SubScanComplete);
            _regScan.ScanCount += new SearchProcess.ScanCountDelegate(RegScan_ScanCount);
            _regScan.StatusChange += new SearchProcess.StatusChangeDelegate(RegScan_StatusChange);
            _elapsedTimer = new DispatcherTimer();
            _elapsedTimer.Interval = new TimeSpan(1000);
            _elapsedTimer.IsEnabled = false;
            _elapsedTimer.Tick += new EventHandler(ElapsedTimer_Tick);
        }

        public ScanSettingsPanelVM SearchOptions { get; set; }
        public OptionsPanelVM ProgramOptions { get; set; }
        public ActiveScanPanelVM ActivityData { get; set; }

        public bool IsTimerOn
        {
            get { return _elapsedTimer.IsEnabled; }
        }

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

        #region Library Events


        private void RegScan_ProcessCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusBarMessage = "Scan Complete!";
        }

        private void RegScan_ScanComplete()
        {
            StatusBarMessage = "Scan Complete!";            
            _elapsedTimer.IsEnabled = false;
        }

        private void RegScan_ScanCount(int count)
        {
            ActivityData.ProgressMax = count;
        }

        private void RegScan_StatusChange(string label)
        {
            StatusBarMessage = label;
        }
        /// <summary>
        /// Срабатывает при завершении сканирования папки
        /// </summary>
        /// <param name="id"></param>
        private void RegScan_SubScanComplete(string id)
        {
            
        }

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
        #endregion
    }
}
