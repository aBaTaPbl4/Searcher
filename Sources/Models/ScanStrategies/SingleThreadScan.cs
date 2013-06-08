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
                ScanFolder(folderName);
            }
            return true;
        }
    }
}
