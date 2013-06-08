using System;
using System.IO;
using Common;
using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using Rhino.Mocks;
using SearchByTag;
using SearchByType;
using Searcher.VM;
using ServiceImpls;

namespace SearcherTests
{
    /// <summary>
    /// Фабрика объектов для системных тестов = присутвсую зависимости от сетевых сервисов, жесткого диска, базы данных и т.д.
    /// Нужны для того, чтобы тестировать совместимость ПО с внешними системами на данный момент.
    /// Как правило запускаются в контексте ночного билда
    /// aBaTaPbl4
    /// </summary>
    public class SystemTestsObjectsFactory : IObjectsFactory
    {
        public void Initialize()
        {
            
        }

        public void RestoreObjects()
        {
            var sourcePath = Path.Combine(Environment.CurrentDirectory, "TestData");
            var destPath = Environment.CurrentDirectory;
            TestHelper.CopyFolder(sourcePath, destPath);
        }

        public ISearchSettings CreateSettings(string fileNameSearchPattern = "note.*", string fileContentSearchPattern = "note", bool isMultithreaded = false, ISearchPlugin[] activePlugins = null)
        {

            var settings = MockRepository.GeneratePartialMock<ScanSettingsPanelVM>();
            settings.FolderToScan = TestHelper.DeepestFolder;
            settings.IsMultithreadRequired = isMultithreaded;
            settings.FileNameSearchPattern = fileNameSearchPattern;
            settings.FileContentSearchPattern = fileContentSearchPattern;
            settings.InitPlugins();
            activePlugins = activePlugins ?? AppContext.PluginManager.AllPlugins;
            settings.Stub(x => x.ActivePlugins).Return(activePlugins);                
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

        public PluginManager CreatePluginManagerConcrete()
        {
            return new PluginManager();
        }

        public ScanStrategyBase CreateStrategy()
        {
            return CreateSingleThreadStrategy();
        }

        public ScanStrategyBase CreateMultiThreadStrategy()
        {
            return new MultiThreadScan();
        }

        public ScanStrategyBase CreateSingleThreadStrategy()
        {
            return new SingleThreadScan();
        }

        public SearchProcess CreateEngine()
        {
            var engine = new SearchProcess();
            engine.ScanStrategy = CreateStrategy();
            return engine;
        }

        public ISearchPlugin CreateFileNamePlugin()
        {
            return new SearchByFileNamePlugin();
        }

        public ISearchPlugin CreateTagPlugin()
        {
            return new SearchByTagPlugin();
        }

        public ISearchPlugin CreateTypePlugin()
        {
            return new SearchByTypePlugin();
        }

    }
}
