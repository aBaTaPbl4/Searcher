using System;
using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using Rhino.Mocks;
using ServiceImpls;

namespace SearcherTests.ObjectsFactory
{
    /// <summary>
    /// Фабрика тестовых объектов для юнит тестов.
    /// Контекст юнит тестов конфигурируются данным видом фабрики.
    /// </summary>
    public class UnitTestsObjectsFactory : IObjectsFactory
    {
        #region IObjectsFactory Members

        public void RestoreObjects()
        {
            //Так как тестовые данные для конфигурации моков не меняются, метод останется пустым
        }

        public IScanSettings CreateScanSettings(string fileNameSearchPattern, string fileContentSearchPattern,
                                                    IScanPlugin[] activePlugins)
        {
            throw new NotImplementedException();
        }

        public IFileSystem CreateFileSystem()
        {
            throw new NotImplementedException();
        }

        public IPluginManager CreatePluginManager()
        {
            throw new NotImplementedException();
        }

        public PluginManager CreatePluginManagerConcrete()
        {
            throw new NotImplementedException();
        }

        public ScanStrategyBase CreateStrategy()
        {
            var strategy = MockRepository.GenerateStub<ScanStrategyBase>();
            strategy.Stub(x => x.StartScan(null)).IgnoreArguments().Return(true);
            return strategy;
        }

        public Scan CreateEngine()
        {
            throw new NotImplementedException();
        }

        public ScanStrategyBase CreateMultiThreadStrategy()
        {
            throw new NotImplementedException();
        }

        public ScanStrategyBase CreateSingleThreadStrategy()
        {
            throw new NotImplementedException();
        }

        public IScanPlugin CreateFileNamePlugin()
        {
            throw new NotImplementedException();
        }

        public IScanPlugin CreateTagPlugin()
        {
            throw new NotImplementedException();
        }

        public IScanPlugin CreateTypePlugin()
        {
            throw new NotImplementedException();
        }

        public IProgramSettings CreateProgramSettings(WorkType tp, int threadsCount, bool logRequired,
                                                      bool verboseLogRequired)
        {
            throw new NotImplementedException();
        }

        public Scan CreateScan()
        {
            throw new NotImplementedException();
        }

        #endregion

        public IProgramSettings CreateProgramSettings()
        {
            throw new NotImplementedException();
        }
    }
}