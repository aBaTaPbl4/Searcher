using System;
using System.Collections.Generic;
using System.Threading;
using Common;

namespace Models.ScanStrategies
{
    public class MultiThreadScan : ScanStrategyBase
    {
        private class BuildThreadArgs
        {
            public ManualResetEvent Handler;
            public string FolderName;
        }

        protected override bool StartScanInner(SearchProcess search)
        {
            var waitHandles = new List<WaitHandle>();
            foreach(var folderName in FoldersToScan)
            {
                if (search.IsNeedCancelation)
                {
                    search.CancelationOccured();
                    return false;
                }
                var args = new BuildThreadArgs()
                               {
                                   Handler = new ManualResetEvent(false),
                                   FolderName = folderName
                               };
                waitHandles.Add(args.Handler);
                ThreadPool.QueueUserWorkItem(new WaitCallback(ScanFolderInSeperateThread), args);
            }
            if (waitHandles.Count > 0)
            {
                WaitHandle.WaitAll(waitHandles.ToArray());
            }
            return true;
        }

        private void ScanFolderInSeperateThread(object buildArgs)
        {
            BuildThreadArgs args = null;
            try
            {
                args = buildArgs as BuildThreadArgs;
                ScanFolder(args.FolderName);
            }
            finally 
            {                
                if (args != null)
                {
                    args.Handler.Set();
                }
            }
        }
    }
}
