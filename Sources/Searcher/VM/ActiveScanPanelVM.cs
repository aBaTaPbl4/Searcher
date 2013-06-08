using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Searcher.VM
{
    public class ActiveScanPanelVM
    {

        public ActiveScanPanelVM()
        {
            Results = new ObservableCollection<ScanDataVM>();
            Reset();
        }

        public int MatchesCount { get; private set; }
        public string LastMatch { get; private set; }
        public int FolderCountScanned { get; private set; }
        public string LastScanedFolder { get; private set; }
        public double TimeElapsed { get; set; }
        public int ProgressMax { get; set; }
        public int Progress { get; set; }

        public ObservableCollection<ScanDataVM> Results{ get; private set; }

        public void AddFoundData(ScanDataVM data)
        {
            Results.Add(data);
            MatchesCount++;
            LastMatch = data.FileName;
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
        }

        public void CheckResults(bool value)
        {
            foreach (var scanDataVM in Results)
            {
                scanDataVM.Check = value;
            }
        }

    }
}
