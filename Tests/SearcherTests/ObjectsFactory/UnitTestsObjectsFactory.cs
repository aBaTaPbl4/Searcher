using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using ServiceImpls;

namespace SearcherTests.ObjectsFactory
{
    public class UnitTestsObjectsFactory : IObjectsFactory
    {
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
            throw new NotImplementedException();
        }

        public SearchEngine CreateEngine()
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
