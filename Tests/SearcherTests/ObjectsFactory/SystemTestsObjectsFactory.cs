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
using Spring.Objects.Factory.Attributes;

namespace SearcherTests.ObjectsFactory
{
    /// <summary>
    /// Фабрика объектов для системных тестов = присутвсую зависимости от сетевых сервисов, жесткого диска, базы данных и т.д.
    /// Нужны для того, чтобы тестировать совместимость ПО с внешними системами на данный момент.
    /// Как правило запускаются в контексте ночного билда
    /// </summary>
    public class SystemTestsObjectsFactory : IObjectsFactory
    {
        [Required]
        public IPluginManager PluginManager { get; set; }

        public IFileSystem FileSystem { get; set; }
        #region IObjectsFactory Members

        public void RestoreObjects()
        {
            string sourcePath = Path.Combine(Environment.CurrentDirectory, "TestData");
            //todo:сделать предварительное удаление всех файлов и каталогов скопированных ранее
            string destPath = Environment.CurrentDirectory;
            TestHelper.CopyFolder(sourcePath, destPath);
        }

        public IProgramSettings CreateProgramSettings(WorkType tp = WorkType.SingleThread, int threadsCount = 0,
                                                      bool logRequired = false, bool verboseLogRequired = false)
        {
            var settings = MockRepository.GenerateMock<IProgramSettings>();
            settings.Stub(x => x.WorkType).Return(tp);
            settings.Stub(x => x.ThreadsNumber).Return(threadsCount);
            settings.Stub(x => x.EnableLogging).Return(logRequired);
            settings.Stub(x => x.VerboseLogging).Return(verboseLogRequired);
            return settings;
        }

        public Scan CreateScan()
        {
            return MockRepository.GeneratePartialMock<Scan>();
        }

        public IScanSettings CreateScanSettings(string fileNameSearchPattern = "note.",
                                                    string fileContentSearchPattern = "note",
                                                    bool isMultithreaded = false, IScanPlugin[] activePlugins = null)
        {
            var settings = MockRepository.GeneratePartialMock<ScanSettingsPanelVM>();
            settings.FolderToScan = TestHelper.DeepestFolder;
            settings.FileNameSearchPattern = fileNameSearchPattern;
            settings.FileContentSearchPattern = fileContentSearchPattern;
            if (activePlugins != null)
            {
                settings.Stub(x => x.ActivePlugins).Return(activePlugins);
            }

            return settings;
        }

        public IFileSystem CreateFileSystem()
        {
            return AppContext.FileSystem;
        }

        public IPluginManager CreatePluginManager()
        {
            return AppContext.PluginManager;
        }

        public PluginManager CreatePluginManagerConcrete()
        {
            return AppContext.PluginManager as PluginManager;
        }

        public ScanStrategyBase CreateStrategy()
        {
            return CreateSingleThreadStrategy();
        }

        public ScanStrategyBase CreateMultiThreadStrategy()
        {
            return AppContext.GetObject<MultiThreadScan>();
        }

        public ScanStrategyBase CreateSingleThreadStrategy()
        {
            return AppContext.GetObject<SingleThreadScan>();
        }

        public Scan CreateEngine()
        {
            return AppContext.GetObject<Scan>();
        }

        public IScanPlugin CreateFileNamePlugin()
        {
            return new ScanByFileAttributesPlugin();
        }

        public IScanPlugin CreateTagPlugin()
        {
            return new ScanByTagPlugin();
        }

        public IScanPlugin CreateTypePlugin()
        {
            return new ScanByTypePlugin();
        }

        #endregion

        public IProgramSettings CreateProgramSettings()
        {
            return CreateProgramSettings(WorkType.SingleThread);
        }

        public IScanSettings CreateScanSettings()
        {
            return CreateScanSettings("note.");
        }
    }
}