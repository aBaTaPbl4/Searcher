namespace Searcher.VM
{
    public interface IWndMain
    {
        /// <summary>
        /// ������������ ���������. ��������� �� ������� vm
        /// </summary>
        void ScanningCompleted();

        void ScanningCanceled();
    }
}