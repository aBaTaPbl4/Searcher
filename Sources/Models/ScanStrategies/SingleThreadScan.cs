namespace Models.ScanStrategies
{
    public class SingleThreadScan : ScanStrategyBase
    {
        protected override bool StartScanInner(SearchProcess search)
        {
            foreach (string folderName in FoldersToScan)
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