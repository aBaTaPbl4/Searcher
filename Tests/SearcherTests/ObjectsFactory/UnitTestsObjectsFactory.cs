using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;
using Models.ScanStrategies;

namespace SearcherTests.ObjectsFactory
{
    public class UnitTestsObjectsFactory //: IObjectsFactory
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

        public ScanStrategyBase CreateStrategy()
        {
            throw new NotImplementedException();
        }
    }
}
