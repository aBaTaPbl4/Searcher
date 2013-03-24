using System.Collections.Generic;
using System.Threading;
using Common;

namespace Models.ScanStrategies
{
    public class SingleThreadScan : ScanStrategyBase
    {
        public override bool StartScan(SearchEngine engine)
        {
            _engine = engine;
            foreach(var folderName in FoldersToScan)
            {
                ScanFolder(folderName);
            }
            return true;
        }
    }
}
