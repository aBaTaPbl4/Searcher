using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using ServiceImpls;

namespace SearcherTests
{
    public interface IObjectsFactory
    {
        /// <summary>
        /// ������������� ������� �������� ����� ����������� ���� ���, ��� ������ ����������.
        /// </summary>
        void Initialize();
        void RestoreObjects();
        ISearchSettings CreateSettings(string fileNameSearchPattern = "note.*", string fileContentSearchPattern = "note", bool isMultithreaded = false, ISearchPlugin[] activePlugins = null);
        IFileSystem CreateFileSystem();
        IPluginManager CreatePluginManager();
        PluginManager CreatePluginManagerConcrete();
        ScanStrategyBase CreateStrategy();
        SearchProcess CreateEngine();
        ScanStrategyBase CreateMultiThreadStrategy();
        ScanStrategyBase CreateSingleThreadStrategy();
        ISearchPlugin CreateFileNamePlugin();
        ISearchPlugin CreateTagPlugin();
        ISearchPlugin CreateTypePlugin();
    }
}