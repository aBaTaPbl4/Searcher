using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using ServiceImpls;

namespace SearcherTests
{
    public interface IObjectsFactory
    {
        void RestoreObjects();

        IScanSettings CreateScanSettings(string fileNameSearchPattern = "note.",
                                             string fileContentSearchPattern = "note",
                                             bool isMultithreaded = false, IScanPlugin[] activePlugins = null);

        IFileSystem CreateFileSystem();
        IPluginManager CreatePluginManager();
        PluginManager CreatePluginManagerConcrete();
        ScanStrategyBase CreateStrategy();
        Scan CreateEngine();
        ScanStrategyBase CreateMultiThreadStrategy();
        ScanStrategyBase CreateSingleThreadStrategy();
        IScanPlugin CreateFileNamePlugin();
        IScanPlugin CreateTagPlugin();
        IScanPlugin CreateTypePlugin();

        IProgramSettings CreateProgramSettings(WorkType tp = WorkType.SingleThread, int threadsCount = 0,
                                               bool logRequired = false, bool verboseLogRequired = false);

        Scan CreateScan();
    }
}