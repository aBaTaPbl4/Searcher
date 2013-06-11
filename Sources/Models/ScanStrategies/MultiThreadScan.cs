using System.Collections.Generic;
using System.Threading;

namespace Models.ScanStrategies
{
    public class MultiThreadScan : ScanStrategyBase
    {
        protected override bool StartScanInner(Scan scan)
        {
            var waitHandles = new List<WaitHandle>();
            foreach (string folderName in FoldersToScan)
            {
                if (scan.IsNeedCancelation)
                {
                    scan.CancelationOccured();
                    return false;
                }
                var args = new BuildThreadArgs
                               {
                                   Handler = new ManualResetEvent(false),
                                   FolderName = folderName
                               };
                waitHandles.Add(args.Handler);
                ThreadPool.QueueUserWorkItem(ScanFolderInSeperateThread, args);
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

        #region Nested type: BuildThreadArgs

        private class BuildThreadArgs
        {
            public string FolderName;
            public ManualResetEvent Handler;
        }

        #endregion
    }
}