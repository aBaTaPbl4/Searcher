namespace Models.ScanStrategies
{
    public class SingleThreadScan : ScanStrategyBase
    {
        protected override bool StartScanInner(Scan scan)
        {
            foreach (string folderName in FoldersToScan)
            {
                if (scan.IsNeedCancelation)
                {
                    scan.CancelationOccured();
                    return false;
                }
                ScanFolder(folderName);
            }
            return true;
        }
    }
}