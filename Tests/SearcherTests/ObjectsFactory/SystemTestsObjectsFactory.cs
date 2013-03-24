using Common;
using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using Rhino.Mocks;
using Searcher.VM;
using ServiceImpls;

namespace SearcherTests
{
    public class SystemTestsObjectsFactory : IObjectsFactory
    {

        public ISearchSettings CreateSettings(string fileNameSearchPattern = "note.*", string fileContentSearchPattern = "note", bool isMultithreaded = false, ISearchPlugin[] activePlugins = null)
        {

            var settings = new SearchSettings();
            settings.FolderToScan = TestHelper.DeepestFolder;
            settings.IsMultithreadRequired = isMultithreaded;
            settings.FileNameSearchPattern = fileNameSearchPattern;
            settings.FileContentSearchPattern = fileContentSearchPattern;
            settings.ActivePlugins = activePlugins ?? AppContext.PluginManager.AllPlugins;
            return settings;            
        }

        
        public IFileSystem CreateFileSystem()
        {
            return new FileSystem();
        }

        public IPluginManager CreatePluginManager()
        {
            return new PluginManager();
        }

        //todo: Оставил до тех пор пока не допишу UnitTestsObjectsFactory
        public ScanStrategyBase CreateStrategy()
        {
            var strategy = MockRepository.GenerateStub<ScanStrategyBase>();
            strategy.Stub(x => x.StartScan(null)).IgnoreArguments().Return(true);
            return strategy;
        }

        public SearchEngine CreateEngine()
        {
            var engine = new SearchEngine();
            engine.ScanStrategy = CreateStrategy();
            return engine;
        }

    }
}
