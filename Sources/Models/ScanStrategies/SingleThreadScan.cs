using System.Collections.Generic;
using System.Threading;
using Common;

namespace Models.ScanStrategies
{
    public class SingleThreadScan : ScanStrategyBase
    {
        protected override bool StartScanInner(SearchProcess search)
        {
            foreach(var folderName in FoldersToScan)
            {
                if (search.IsNeedCancelation)
                {
                    search.CancelationOccured();
                    return false;
                }
                ScanFolder(folderName);
            }
            return true;
        }
    }
}
