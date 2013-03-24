using System.Collections.Generic;
using Common;

namespace Models.ScanStrategies
{
    public class SingleThreadScan
    {
        private SearchEngine _engine;

        public bool StartScan(SearchEngine engine)
        {
            _engine = engine;
            var foldersToScan = AppContext.FileSystem.GetAllSubfolders(AppContext.SearchSettings.FolderToScan);
            foldersToScan.Add(AppContext.SearchSettings.FolderToScan);
            foreach(var folderName in foldersToScan)
            {
                ScanFolder(folderName);
            }
            return false;
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
