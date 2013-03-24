using Common.Interfaces;
using Models;
using Models.ScanStrategies;

namespace SearcherTests
{
    public interface IObjectsFactory
    {
        ISearchSettings CreateSettings(string fileNameSearchPattern = "note.*", string fileContentSearchPattern = "note", bool isMultithreaded = false, ISearchPlugin[] activePlugins = null);
        IFileSystem CreateFileSystem();
        IPluginManager CreatePluginManager();
        ScanStrategyBase CreateStrategy();
        SearchEngine CreateEngine();
    }
}