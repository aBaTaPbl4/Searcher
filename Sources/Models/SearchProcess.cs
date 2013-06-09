using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using Common.Interfaces;
using Models.ScanStrategies;

namespace Models
{
    public class SearchProcess
    {
        private BackgroundWorker _worker;
        private readonly List<ScanData> _foundFiles = new List<ScanData>();
        private ManualResetEvent _completionSignal = new ManualResetEvent(false);
        private bool _lastScanResult;
        #region Delegates
        public delegate void ReportProgressDelegate(int percentsComplete);
        public delegate void FileFoundDelegate(ScanData foundInfo);        
        public delegate void SubScanCompleteDelegate(string folderName);
        public delegate void ProcessCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e);

        public delegate void ScanStartedHandler();

        public delegate void CountingFilesHandler();
        #endregion

        #region Events

        [Description("Occures when progress is changed")]
        public event ReportProgressDelegate ProgressChanged;
        
        [Description("Occurs when file matched to condition has found")]
        public event FileFoundDelegate FileWasFound;
        
        [Description("Occurs when subfolder scan completed")]
        public event SubScanCompleteDelegate SubScanComplete;

        [Description("Occurs when scan Completed")]
        public event ProcessCompletedEventHandler ScanComplete;

        [Description("Occurs on pre scan stage")]
        public event CountingFilesHandler CountingFiles;

        [Description("Occurs when scan fisicaly begins")]
        public event ScanStartedHandler ScanStarted;

#region call by strategy to notify ui

        internal void RaiseProgressChanged(int currentProgress)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(currentProgress);
            }
        }

        internal void RaiseSubScanCompleted(string folderName)
        {
            if (SubScanComplete != null)
            {
                SubScanComplete(folderName);
            }
        }

        internal void RaiseCountingFiles()
        {
            if (CountingFiles != null)
            {
                CountingFiles();
            }
        }

        internal void RaiseScanStarted()
        {
            if (ScanStarted != null)
            {
                ScanStarted();
            }
        }

#endregion

        private void RaiseFileWasFound(ScanData fileInfo)
        {
            if (FileWasFound != null)
            {
                FileWasFound(fileInfo);
            }
        }


        private void RaiseScanCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ScanComplete != null)
            {
                ScanComplete(this, e);
            }
            //IsNeedCancelation = false;
            _completionSignal.Set();
        }
        #endregion

        public string Folder{ get; set; }

        public ScanData[] FoundFiles
        {
            get { return _foundFiles.ToArray(); }
        }

        internal void AddFoundFile(ScanData fileInfo)
        {
            lock(_foundFiles)
            {
                _foundFiles.Add(fileInfo);
                RaiseFileWasFound(fileInfo);
            }
        }


        public ScanStrategyBase ScanStrategy { get; set; }

        public void StartScanAsync()
        {
            //IsNeedCancelation = false;
            if (_worker != null)
            {
                ResetAsync();
            }
            _completionSignal.Reset();
            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += DoWork;
            _worker.RunWorkerCompleted += RaiseScanCompleted;
            _worker.Disposed += new EventHandler(WorkerDisposed);
            _worker.RunWorkerAsync();
        }

        public int FilesProcessed
        {
            get { return ScanStrategy.FileProcessed; }
        }

        public bool IsNeedCancelation
        {
            get
            {
                return _worker.CancellationPending;
            }
        }

        public void CancelProcessAsync()
        {
            if (_worker != null)
            {
                //IsNeedCancelation = true;
                _worker.CancelAsync();
                //RaiseScanCompleted(null, null);
                //ResetAsync();
            }
        }

        /// <summary>
        /// Метод нужен тольк для удобства тестирования
        /// </summary>
        /// <returns></returns>
        public bool StartScan()
        {
            StartScanAsync();
            _completionSignal.WaitOne();
            return LastScanResult;
        }

        public bool LastScanResult
        {
            get { return _lastScanResult; }
            private set { _lastScanResult = value; }
        }

        private DoWorkEventArgs _currentArgs;

        internal bool Cancel
        {
            set { _currentArgs.Cancel = value; }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _currentArgs = e;
                LastScanResult = false;
                LastScanResult = ScanStrategy.StartScan(this);
                if (!LastScanResult)
                {
                    CancelProcessAsync();
                }
            }
            catch (StopProcessingException)
            {
                AppContext.Logger.InfoFormat("Scan process was stoped.");
            }
            catch (Exception ex)
            {
                AppContext.Logger.ErrorFormat("In scan thread error occured. {0}", ex);
            }
        }

        private void ResetAsync()
        {
            if (_worker != null)
            {
                _worker.Dispose();
            }
            
        }

        private void WorkerDisposed(object sender, EventArgs e)
        {
            _worker.DoWork -= DoWork;
            _worker.RunWorkerCompleted -= RaiseScanCompleted;
            _worker = null;
        }

        public ISearchSettings Settings
        {
            get { return ScanStrategy.SearchSettings; }
            set { ScanStrategy.SearchSettings = value; }
        }

        private class StopProcessingException : Exception
        {
        }

    }
}
