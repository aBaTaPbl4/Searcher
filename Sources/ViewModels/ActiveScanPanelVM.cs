using System;
using System.Collections.ObjectModel;

namespace Searcher.VM
{
    public class ActiveScanPanelVM : ViewModel
    {
        private string _actionButtonText;
        private int _folderCountScanned;
        private string _lastMatch;
        private string _lastScanedFolder;
        private int _matchesCount;
        private int _progress;
        private int _progressMax;
        private double _timeElapsed;

        public ActiveScanPanelVM()
        {
            Results = new ObservableCollection<ScanDataVM>();
            Reset();
        }

        public int MatchesCount
        {
            get { return _matchesCount; }
            private set
            {
                _matchesCount = value;
                OnPropertyChanged("MatchesCount");
            }
        }

        public string LastMatch
        {
            get { return _lastMatch; }
            private set
            {
                _lastMatch = value;
                OnPropertyChanged("LastMatch");
            }
        }


        public int FolderCountScanned
        {
            get { return _folderCountScanned; }
            private set
            {
                _folderCountScanned = value;
                OnPropertyChanged("FolderCountScanned");
            }
        }

        public string LastScanedFolder
        {
            get { return _lastScanedFolder; }
            private set
            {
                _lastScanedFolder = value;
                OnPropertyChanged("LastScanedFolder");
            }
        }

        public string ActionButtonText
        {
            get { return _actionButtonText; }
            set
            {
                _actionButtonText = value;
                OnPropertyChanged("ActionButtonText");
            }
        }

        public double TimeElapsed
        {
            get { return _timeElapsed; }
            set
            {
                _timeElapsed = value;
                OnPropertyChanged("TimeElapsed");
            }
        }

        public int ProgressMax
        {
            get { return _progressMax; }
            set
            {
                _progressMax = value;
                OnPropertyChanged("ProgressMax");
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public ObservableCollection<ScanDataVM> Results { get; private set; }

        public void AddFoundData(ScanDataVM data)
        {
            Action act = () =>
                             {
                                 Results.Add(data);
                                 MatchesCount++;
                                 LastMatch = data.FileName;
                             };
            InvokeInUIThread(act);
        }

        public void FinishedFolderScan(string folderName)
        {
            LastScanedFolder = folderName;
            FolderCountScanned++;
        }

        public void Reset()
        {
            MatchesCount = 0;
            LastMatch = "";
            FolderCountScanned = 0;
            LastScanedFolder = "";
            TimeElapsed = 0;
            ProgressMax = 100;
            Progress = 0;
            Results.Clear();
            ActionButtonText = WndMainVM.Cancel;
        }

        public void CheckResults(bool value)
        {
            foreach (ScanDataVM scanDataVM in Results)
            {
                scanDataVM.Checked = value;
            }
        }
    }
}