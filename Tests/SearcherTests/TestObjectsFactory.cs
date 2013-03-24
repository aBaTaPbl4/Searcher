using Common;
using Common.Interfaces;
using Searcher.VM;
using ServiceImpls;

namespace SearcherTests
{
    public static class TestObjectsFactory
    {

        public static ISearchSettings CreateSettings(string fileNameSearchPattern = "note.*", string fileContentSearchPattern = "note", bool isMultithreaded = false, ISearchPlugin[] activePlugins = null)
        {

            var settings = new SearchSettings();
            settings.FolderToScan = TestHelper.DeepestFolder;
            settings.IsMultithreadRequired = isMultithreaded;
            settings.FileNameSearchPattern = fileNameSearchPattern;
            settings.FileContentSearchPattern = fileContentSearchPattern;
            settings.ActivePlugins = activePlugins ?? AppContext.PluginManager.AllPlugins;
            return settings;            
        }

        //todo:не забыть реализовать возрат замокированного сервиса
        public static IFileSystem CreateFileSystemStub()
        {
            return null;
        }

        public static IPluginManager CreatePluginManagerStub()
        {
            return null;
        }

    }
}
