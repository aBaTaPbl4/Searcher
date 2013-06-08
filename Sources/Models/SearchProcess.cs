using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Common;
using Models.ScanStrategies;

namespace Models
{
    public class SearchProcess
    {
        private BackgroundWorker _worker;
        private readonly List<ScanData> _foundFiles = new List<ScanData>();
        private ScanStrategyBase _scanStrategy;

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
            if (ScanComplete != null)
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


        public ScanStrategyBase ScanStrategy
        {
            get
            {
                if (_scanStrategy == null)
                {
                    _scanStrategy = ScanStrategyBase.CreateInstance();
                }
                return _scanStrategy;
            }
            set
            {
                _scanStrategy = value;
            }
        }

        public void AsyncScan()
        {
            if (_worker != null)
            {
                ResetAsync();
            }

            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += DoWork;
            _worker.RunWorkerCompleted += RaiseScanCompleted;
            _worker.Disposed += new EventHandler(WorkerDisposed);
            _worker.RunWorkerAsync();
        }


        public void CancelProcessAsync()
        {
            if (_worker != null)
            {
                _worker.CancelAsync();
                ResetAsync();
            }
        }

        public bool StartScan()
        {

            var result = ScanStrategy.StartScan(this);
            return result;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!StartScan())
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

        private class StopProcessingException : Exception
        {
        }

    }
}
