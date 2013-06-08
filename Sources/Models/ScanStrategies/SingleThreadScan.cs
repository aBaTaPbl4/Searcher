using System.Collections.Generic;
using System.Threading;
using Common;

namespace Models.ScanStrategies
{
    public class SingleThreadScan : ScanStrategyBase
    {
        public override bool StartScan(SearchProcess search)
        {
            _search = search;
            foreach(var folderName in FoldersToScan)
            {
                ScanFolder(folderName);
            }
            return true;
        }
    }
}
