﻿using System;
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

        public override bool StartScan(SearchEngine engine)
        {
            _engine = engine;
            var waitHandles = new List<WaitHandle>();
            foreach(var folderName in FoldersToScan)
            {
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