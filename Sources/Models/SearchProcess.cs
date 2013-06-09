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
        #region Delegates
        public delegate void ReportProgressDelegate(int percentsComplete);
        public delegate void FileFoundDelegate(ScanData foundInfo);        
        public delegate void SubScanCompleteDelegate(string folderName);
        public delegate void ProcessCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e);
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

        public void AsyncScan()
        {
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

        public void CancelProcessAsync()
        {
            if (_worker != null)
            {
                _worker.CancelAsync();
                ResetAsync();
            }
        }

        /// <summary>
        /// Метод нужен тольк для удобства тестирования
        /// </summary>
        /// <returns></returns>
        public bool StartScan()
        {
            AsyncScan();
            _completionSignal.WaitOne();
            return LastScanResult;
        }

        public bool LastScanResult { get; private set; }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                LastScanResult = false;
                LastScanResult = ScanStrategy.StartScan(this);
                if (!LastScanResult)
                {
                    CancelProcessAsync();
                }
            }
            catch (StopProcessingException)
            {
                // end
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
        }

        private class StopProcessingException : Exception
        {
        }

    }
}
