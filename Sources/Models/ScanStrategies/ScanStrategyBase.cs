using System.Collections.Generic;
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
            foreach (var fileName in files)
            {
                foreach (var plugin in AppContext.SearchSettings.ActivePlugins)
                {
                    if (plugin.Check(fileName, AppContext.SearchSettings))
                    {
                        _search.AddFoundFile(fileName);
                        break;
                    }
                }
            }
        }
    }
}