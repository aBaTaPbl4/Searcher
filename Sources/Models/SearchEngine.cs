using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Common;
using Models.ScanStrategies;

namespace Models
{
    public class SearchEngine
    {
        private BackgroundWorker _oProcessAsyncBackgroundWorker;
        private List<string> _foundFiles = new List<string>();

        #region Delegates
        public delegate void LabelChangeDelegate(string phase, string label);
        public delegate void CurrentPathDelegate(string hive, string path);
        public delegate void KeyCountDelegate();
        public delegate void MatchItemDelegate(string file);
        public delegate void StatusChangeDelegate(string label);
        public delegate void ProcessChangeDelegate();
        public delegate void ScanCountDelegate(int count);
        public delegate void ScanCompleteDelegate();
        public delegate void SubScanCompleteDelegate(string id);
        public delegate void ProcessCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e);
        #endregion

        #region Events
        [Description("Status update")]
        public event LabelChangeDelegate LabelChange;
        [Description("Current processing path")]
        public event CurrentPathDelegate CurrentPath;
        [Description("Key processed count")]
        public event KeyCountDelegate KeyCount;
        [Description("Match item was found")]
        public event MatchItemDelegate MatchItem;
        [Description("Processing status has changed")]
        public event StatusChangeDelegate StatusChange;
        [Description("Processing shifted to new task")]
        public event ProcessChangeDelegate ProcessChange;
        [Description("Task counter")]
        public event ScanCountDelegate ScanCount;
        [Description("Scan Completed")]
        public event ScanCompleteDelegate ScanComplete;
        [Description("Scan Completed")]
        public event SubScanCompleteDelegate SubScanComplete;
        public event ProcessCompletedEventHandler ProcessCompleted;
        #endregion

        public string Folder{ get; set; }

        public string[] FoundFiles
        {
            get { return _foundFiles.ToArray(); }
        }

        internal void AddFoundFile(string fileName)
        {
            lock(_foundFiles)
            {
                if (!_foundFiles.Contains(fileName))
                {
                    _foundFiles.Add(fileName);
                }
                if (MatchItem != null)
                {
                    MatchItem(fileName);
                }
            }            
        }

        public bool StartScan()
        {
            
            if (LabelChange == null || CurrentPath == null || KeyCount == null || MatchItem == null ||
                StatusChange == null || ProcessChange == null || ScanCount == null || ScanComplete == null || 
                SubScanComplete == null)
            {
                return false;
            }
            StatusChange("Starting scanning...");
            SingleThreadScan scan;
            if (AppContext.SearchSettings.IsMultithreadRequired)
            {
                throw new NotImplementedException("");
            }
            else
            {
                scan = new SingleThreadScan();
            }
            var result = scan.StartScan(this);
            StatusChange("Scan finished...");
            ScanComplete();
            return result;
        }

        public void AsyncScan()
        {
            if (_oProcessAsyncBackgroundWorker != null)
            {
                ResetAsync();
            }

            _oProcessAsyncBackgroundWorker = new BackgroundWorker();
            _oProcessAsyncBackgroundWorker.WorkerSupportsCancellation = true;
            _oProcessAsyncBackgroundWorker.DoWork += ProcessAsyncBackgroundWorker_DoWork;
            _oProcessAsyncBackgroundWorker.RunWorkerCompleted += ProcessAsyncBackgroundWorker_RunWorkerCompleted;
            _oProcessAsyncBackgroundWorker.Disposed += new EventHandler(_oProcessAsyncBackgroundWorker_Disposed);
            _oProcessAsyncBackgroundWorker.RunWorkerAsync();
        }


        public void CancelProcessAsync()
        {
            if (_oProcessAsyncBackgroundWorker != null)
            {
                _oProcessAsyncBackgroundWorker.CancelAsync();
                ResetAsync();
            }
        }

        private void ProcessAsyncBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
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

        private void ProcessAsyncBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ProcessCompleted != null)
            {
                ProcessCompleted(this, e);
            }
        }

        private void ResetAsync()
        {
            if (_oProcessAsyncBackgroundWorker != null)
            {
                _oProcessAsyncBackgroundWorker.Dispose();
            }
        }

        private void _oProcessAsyncBackgroundWorker_Disposed(object sender, EventArgs e)
        {
            _oProcessAsyncBackgroundWorker.DoWork -= ProcessAsyncBackgroundWorker_DoWork;
            _oProcessAsyncBackgroundWorker.RunWorkerCompleted -= ProcessAsyncBackgroundWorker_RunWorkerCompleted;
            _oProcessAsyncBackgroundWorker = null;
        }

        private class StopProcessingException : Exception
        {
        }

    }
}
