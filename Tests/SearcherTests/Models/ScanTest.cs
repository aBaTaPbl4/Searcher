using System.Collections.Generic;
using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using NUnit.Framework;
using Rhino.Mocks;

namespace SearcherTests.Models
{
    [TestFixture]
    public abstract class ScanTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _process = TestsConfiguration.ObjectsFactory.CreateSearchProcess();
            _process.Stub(x => x.IsNeedCancelation).Return(false);
            _process.Stub(x => x.CancelationOccured());
            _process.Folder = TestHelper.DeepestFolder;
            _process.FileWasFound += RegScan_MatchItem;
            _scanStrategy = CreateStrategy();
            _foundFiles = new List<string>();
        }

        #endregion

        private SearchProcess _process;
        protected ScanStrategyBase _scanStrategy;
        private List<string> _foundFiles;

        protected abstract ScanStrategyBase CreateStrategy();

        [TestCase("7z", "", 1)]
        [TestCase("test.txt", "", 2)]
        [TestCase("note", "", 2)]
        [TestCase("", "", TestHelper.FilesInFirstTestDir)]
        [TestCase("", "note", TestHelper.FilesInFirstTestDir)]
        public void ScanOnlyByFileNamesTest(string filePattern, string fileContentPattern, int expectedMatchesCount)
        {
            ISearchSettings settings = TestsConfiguration.ObjectsFactory.CreateSearchSettings(filePattern,
                                                                                              fileContentPattern);

            _scanStrategy.SearchSettings = settings;
            _scanStrategy.StartScan(_process);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }

        [TestCase("", "note", 1, true)]
        [TestCase("", "tagotest", 0, true)]
        [TestCase("", "NativeError", 1, false)]
        public void ScanWithPluginsTest(string filePattern, string fileContentPattern, int expectedMatchesCount,
                                        bool xmlPluginNeeded)
        {
            ISearchPlugin[] activePlugins;
            if (xmlPluginNeeded)
            {
                activePlugins = TestHelper.CoreAndXmlPlugin();
            }
            else
            {
                activePlugins = TestHelper.CoreAndTypePlugin();
            }
            _scanStrategy.SearchSettings =
                TestsConfiguration.ObjectsFactory.CreateSearchSettings(
                    filePattern, fileContentPattern,
                    false, activePlugins);

            _scanStrategy.StartScan(_process);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }

        private void RegScan_MatchItem(ScanData file)
        {
            _foundFiles.Add(file.FullName);
        }
    }
}