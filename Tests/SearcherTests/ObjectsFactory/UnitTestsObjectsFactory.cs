using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using Rhino.Mocks;
using ServiceImpls;

namespace SearcherTests.ObjectsFactory
{
    public class UnitTestsObjectsFactory : IObjectsFactory
    {
        /// <summary>
        /// Инициализация константных тестовых данных на основе жесткого диска
        /// В дальнейшем эти тестовые данные будут использоваться для конфигурирования моков, возращаемых фабрикой
        /// </summary>
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void RestoreObjects()
        {
            //Так как тестовые данные для конфигурации моков не меняются, метод останется пустым
        }

        public ISearchSettings CreateSettings(string fileNameSearchPattern = "note.*", string fileContentSearchPattern = "note", bool isMultithreaded = false, ISearchPlugin[] activePlugins = null)
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

        public SearchProcess CreateEngine()
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

        public ISearchPlugin CreateFileNamePlugin()
        {
            throw new NotImplementedException();
        }

        public ISearchPlugin CreateTagPlugin()
        {
            throw new NotImplementedException();
        }

        public ISearchPlugin CreateTypePlugin()
        {
            throw new NotImplementedException();
        }
    }
}
