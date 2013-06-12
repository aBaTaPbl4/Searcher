using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Common;
using Common.Interfaces;

namespace Models.ScanStrategies
{
    public abstract class ScanStrategyBase
    {
        private double _filesPerOnePercent;
        private volatile int _filesProcessed;
        protected Scan _scan;
        private int _totalFilesCount;

        public IFileSystem FileSystem { get; set; }
        public IScanSettings ScanSettings { get; set; }
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
                foldersToScan.Add(ScanSettings.FolderToScan);
                if (ScanSettings.RecursiveScan)
                {
                    foldersToScan.AddRange(FileSystem.GetAllSubfolders(ScanSettings.FolderToScan));
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

        public bool StartScan(Scan scan)
        {
            _filesProcessed = 0;
            _scan = scan;
            scan.RaiseCountingFiles();
            _totalFilesCount = FileSystem.GetFilesCountToScan();
            _filesPerOnePercent = _totalFilesCount/100.0000;
            scan.RaiseScanStarted();
            return StartScanInner(scan);
        }

        protected abstract bool StartScanInner(Scan scan);

        protected void ScanFolder(string folderName)
        {
            List<string> files = FileSystem.GetFiles(folderName);
            foreach (string fullFileName in files)
            {
                try
                {
                    _filesProcessed++;
                    bool isMath = true;
                    foreach (IScanPlugin plugin in ScanSettings.ActivePlugins)
                    {
                        if (!plugin.Check(fullFileName, ScanSettings, FileSystem))
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
                        var info = FileSystem.GetFileInfo(fullFileName);
                        fileInfo.Size = info.FileSize;
                        fileInfo.ModificationDate = info.ModificationDate;
                        fileInfo.IsArch = info.IsArch;
                        fileInfo.IsHidden = info.IsHidden;
                        fileInfo.IsReadOnly = info.IsReadOnly;
                        _scan.AddFoundFile(fileInfo);
                    }
                    if (IsProgressChanged)
                    {
                        _scan.RaiseProgressChanged((int) (_filesProcessed/_filesPerOnePercent));
                    }
                }
                catch (Exception ex)
                {
                    AppContext.Logger.ErrorFormat("During proccessing file {0} error occured! {1}", fullFileName, ex);
                }
            }
            _scan.RaiseSubScanCompleted(folderName);
        }

        public static ScanStrategyBase CreateInstance()
        {
            switch (AppContext.ProgramSettings.WorkType)
            {
                case WorkType.SingleThread:
                        return new SingleThreadScan();
                    //case WorkType.LimitedThreadsCount:
                    //case WorkType.UnlimitedThreadsCount:
                default:
                        return new MultiThreadScan();                    
            }

        }
    }
}