using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using ServiceImpls;

namespace SearcherTests
{
    public interface IObjectsFactory
    {
        void RestoreObjects();

        ISearchSettings CreateSearchSettings(string fileNameSearchPattern = "note.",
                                                    string fileContentSearchPattern = "note",
                                                    bool isMultithreaded = false, ISearchPlugin[] activePlugins = null);
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
        IProgramSettings CreateProgramSettings(WorkType tp = WorkType.SingleThread, int threadsCount = 0,
                                                      bool logRequired = false, bool verboseLogRequired = false);
    }
}