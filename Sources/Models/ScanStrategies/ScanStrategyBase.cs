using System.Collections.Generic;
using System.IO;
using Common;

namespace Models.ScanStrategies
{
    public abstract class ScanStrategyBase
    {
        protected SearchProcess _search;

        public abstract bool StartScan(SearchProcess search);

        protected List<string> FoldersToScan
        {
            get
            {
                var foldersToScan = AppContext.FileSystem.GetAllSubfolders(AppContext.SearchSettings.FolderToScan);
                foldersToScan.Add(AppContext.SearchSettings.FolderToScan);
                return foldersToScan;
            }
        }

        protected void ScanFolder(string folderName)
        {
            var files = AppContext.FileSystem.GetFiles(folderName);
            foreach (var fullFileName in files)
            {
                foreach (var plugin in AppContext.SearchSettings.ActivePlugins)
                {
                    if (plugin.Check(fullFileName, AppContext.SearchSettings))
                    {
                        var fileInfo = new ScanData();
                        fileInfo.FileName = Path.GetFileName(fullFileName);
                        fileInfo.FolderName = Path.GetDirectoryName(fullFileName);
                        _search.AddFoundFile(fileInfo);
                        break;
                    }
                }
            }
        }

        public static ScanStrategyBase CreateInstance()
        {
            if (AppContext.SearchSettings.IsMultithreadRequired)
            {
                return new MultiThreadScan();
            }
            else
            {
                return new SingleThreadScan();
            }
        }
    }
}