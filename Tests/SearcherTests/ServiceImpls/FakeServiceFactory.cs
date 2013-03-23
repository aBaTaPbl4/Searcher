using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;
using Rhino.Mocks;

namespace SearcherTests.ServiceImpls
{
    public static class FakeObjectsFactory
    {

        public static ISearchSettings CreateSettings(string fileNameSearchPattern = "note.*", string fileContentSearchPattern = "note", bool isMultithreaded = false)
        {
            var settings = MockRepository.GenerateStub<ISearchSettings>();
            settings.Stub(x => x.IsMultithreadRequired).Return(isMultithreaded);
            settings.Stub(x => x.FileNameSearchPattern).Return(fileNameSearchPattern);
            settings.Stub(x => x.FileContentSearchPattern).Return(fileContentSearchPattern);
            return settings;            
        }

    }
}
