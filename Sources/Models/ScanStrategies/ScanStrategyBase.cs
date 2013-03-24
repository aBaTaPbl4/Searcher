using System.Collections.Generic;
using Common;

namespace Models.ScanStrategies
{
    public abstract class ScanStrategyBase
    {
        protected SearchEngine _engine;

        public abstract bool StartScan(SearchEngine engine);

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
                        _engine.AddFoundFile(fileName);
                        break;
                    }
                }
            }
        }
    }
}