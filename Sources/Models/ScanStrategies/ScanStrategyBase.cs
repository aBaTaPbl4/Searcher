using System;
using System.Collections.Generic;
using System.IO;
using Common;

namespace Models.ScanStrategies
{
    public abstract class ScanStrategyBase
    {
        private double _filesPerOnePercent;
        private int _totalFilesCount;
        private volatile int _filesProcessed;
        protected SearchProcess _search;

        public bool StartScan(SearchProcess search)
        {
            _filesProcessed = 0;
            _search = search;
            _totalFilesCount = AppContext.FileSystem.GetFilesCountToScan();
            _filesPerOnePercent = _totalFilesCount/100.0000;
            return StartScanInner(search);            
        }

        protected abstract bool StartScanInner(SearchProcess search);

        protected List<string> FoldersToScan
        {
            get
            {
                var foldersToScan = new List<string>();
                foldersToScan.Add(AppContext.SearchSettings.FolderToScan);
                if (AppContext.SearchSettings.RecursiveScan)
                {
                    foldersToScan.AddRange(AppContext.FileSystem.GetAllSubfolders(AppContext.SearchSettings.FolderToScan));
                }                    
                return foldersToScan;
            }
        }

        private bool IsProgressChanged
        {
            get
            {
                if (_totalFilesCount <= 100)
                {
                    return true;
                }
                return (int)(_filesProcessed % _filesPerOnePercent) == 0;
            }
        }

        protected void ScanFolder(string folderName)
        {
            var files = AppContext.FileSystem.GetFiles(folderName);
            foreach (var fullFileName in files)
            {
                try
                {
                    _filesProcessed++;
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
                    if (IsProgressChanged)
                    {
                        _search.RaiseProgressChanged((int)(_filesProcessed / _filesPerOnePercent));
                    }

                }
                catch (Exception ex)
                {
                    AppContext.Logger.ErrorFormat("During proccessing file {0} error occured! {1}", fullFileName, ex);
                }

            }
            _search.RaiseSubScanCompleted(folderName);

        }

        public static ScanStrategyBase CreateInstance()
        {
            if (AppContext.SearchSettings.IsMultithreadRequired)
            {
                return new MultiThreadScan();
            }
            return new SingleThreadScan();
        }
    }
}