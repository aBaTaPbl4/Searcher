namespace Searcher.VM
{
    public interface IWndMain
    {
        /// <summary>
        /// Сканирование закончено. Дергается со стороны vm
        /// </summary>
        void ScanningCompleted();

        void ScanningCanceled();
    }
}