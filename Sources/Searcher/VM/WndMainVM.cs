﻿using System;
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

    public enum CurrentAppState
    {
        PendingScanning,
        CountingFiles,
        Scanning,
        ScanCompleted
    }

    public class WndMainVM : ViewModel
    {
        SearchProcess _scan;
        private DispatcherTimer _elapsedTimer;
        static DateTime _startScanningTime;
        private string _statusBarMessage;
        private IWndMain _mainWindow;
        private CurrentAppState _currentState;

        public WndMainVM(IWndMain wnd)
        {
            _mainWindow = wnd;
            Reset();
            _elapsedTimer = new DispatcherTimer();
            _elapsedTimer.Interval = new TimeSpan(1000);
            _elapsedTimer.IsEnabled = false;
            _elapsedTimer.Tick += new EventHandler(ElapsedTimer_Tick);
        }

        public void Reset()
        {
            _scan = AppContext.GetObject<SearchProcess>();
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

        public ScanSettingsPanelVM SearchOptions
        {
            get { return AppContext.SearchSettings as ScanSettingsPanelVM; }
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

        public bool IsTimerOn
        {
            get { return _elapsedTimer.IsEnabled; }
        }

        #region Events Processing
        private void CountingFiles()
        {
            CurrentState = CurrentAppState.CountingFiles;
            StatusBarMessage = "Counting files in directory..";
        }

        private void ScanStarted()
        {
            CurrentState = CurrentAppState.Scanning;
            StatusBarMessage = "Scanning directory..";
            ActivityData.FinishedFolderScan(SearchOptions.FolderToScan);
        }

        private void ScanCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusBarMessage = "Scan Complete!";
            CurrentState = CurrentAppState.ScanCompleted;
            ActivityData.Progress=100;
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

        public void StartScanning()
        {
            _startScanningTime = DateTime.Now;
            _elapsedTimer.IsEnabled = true;
            ActivityData.Reset();
            _scan.StartScanAsync();
        }

        public void StopScanning()
        {
            _elapsedTimer.IsEnabled = false;
            _scan.CancelProcessAsync();
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
        public const string Cancel = "Cancel";
        public const string Next = "Next";
        public const string Prev = "Prev";

        private void RefreshActiveScanPanel()
        {
            if (ActivityData == null)
            {
                return;
            }
            switch (CurrentState)
            {
                case CurrentAppState.CountingFiles:
                case CurrentAppState.PendingScanning:
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
            var _timeElapsed = DateTime.Now.Subtract(_startScanningTime);
            ActivityData.TimeElapsed = _timeElapsed.TotalSeconds;
        }
    }
}
