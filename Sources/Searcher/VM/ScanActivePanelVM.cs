using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Searcher.VM
{
    public class ScanActivePanelVM
    {

        public ScanActivePanelVM()
        {
            MatchesCount = 0;
            LastMatch = "";
            FolderCountScanned = 0;
            CurrentFolder = "";
            TimeElapsed = TimeSpan.Zero;
            Results = new ObservableCollection<ScanDataVM>();
        }

        public int MatchesCount { get; private set; }
        public string LastMatch { get; private set; }
        public int FolderCountScanned { get; private set; }
        public string CurrentFolder { get; private set; }
        public TimeSpan TimeElapsed { get; private set; }

        public ObservableCollection<ScanDataVM> Results{ get; private set; }

        public void AddFoundData(ScanDataVM data)
        {
            Results.Add(data);
            MatchesCount++;
            LastMatch = data.FileName;
        }

        public void SetCurrentFolder(string folderName)
        {
            CurrentFolder = folderName;
            FolderCountScanned++;
        }

    }
}
