using System;
using System.Collections.Generic;
using System.IO;
using Common;
using Common.Interfaces;

namespace Models.ScanStrategies
{
    public abstract class ScanStrategyBase
    {
        private double _filesPerOnePercent;
        private volatile int _filesProcessed;
        protected SearchProcess _search;
        private int _totalFilesCount;

        public IFileSystem FileSystem { get; set; }
        public ISearchSettings SearchSettings { get; set; }
        public IProgramSettings ProgramSettings { get; set; }

        public int FileProcessed
        {
            get { return _filesProcessed; }
        }

        protected List<string> FoldersToScan
        {
            get
            {
                var foldersToScan = new List<string>();
                foldersToScan.Add(SearchSettings.FolderToScan);
                if (SearchSettings.RecursiveScan)
                {
                    foldersToScan.AddRange(FileSystem.GetAllSubfolders(SearchSettings.FolderToScan));
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
                return (int) (_filesProcessed%_filesPerOnePercent) == 0;
            }
        }

        public bool StartScan(SearchProcess search)
        {
            _filesProcessed = 0;
            _search = search;
            search.RaiseCountingFiles();
            _totalFilesCount = FileSystem.GetFilesCountToScan();
            _filesPerOnePercent = _totalFilesCount/100.0000;
            search.RaiseScanStarted();
            return StartScanInner(search);
        }

        protected abstract bool StartScanInner(SearchProcess search);

        protected void ScanFolder(string folderName)
        {
            List<string> files = FileSystem.GetFiles(folderName);
            foreach (string fullFileName in files)
            {
                try
                {
                    _filesProcessed++;
                    bool isMath = true;
                    foreach (ISearchPlugin plugin in SearchSettings.ActivePlugins)
                    {
                        if (!plugin.Check(fullFileName, SearchSettings))
                        {
                            isMath = false;
                            break;
                        }
                    }
                    if (isMath)
                    {
                        var fileInfo = new ScanData();
                        fileInfo.FileName = Path.GetFileName(fullFileName);
                        fileInfo.FolderName = Path.GetDirectoryName(fullFileName);
                        _search.AddFoundFile(fileInfo);
                    }
                    if (IsProgressChanged)
                    {
                        _search.RaiseProgressChanged((int) (_filesProcessed/_filesPerOnePercent));
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
            if (AppContext.ProgramSettings.WorkType == WorkType.SingleThread)
            {
                return new SingleThreadScan();
            }
            return new MultiThreadScan();
        }
    }
}