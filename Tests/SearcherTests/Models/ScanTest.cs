using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
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
        private SearchProcess _process;
        protected ScanStrategyBase _scanStrategy;
        private List<string> _foundFiles;

        protected abstract ScanStrategyBase CreateStrategy();

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

        [TestCase("7z", "", 1)]
        [TestCase("test.txt", "", 2)]
        [TestCase("note", "", 2)]
        [TestCase("", "", 0)]
        [TestCase("", "note", 0)]
        public void ScanOnlyByFileNamesTest(string filePattern, string fileContentPattern, int expectedMatchesCount)
        {
            var settings = TestsConfiguration.ObjectsFactory.CreateSearchSettings(filePattern, fileContentPattern);
            
            _scanStrategy.SearchSettings = settings;
            _scanStrategy.StartScan(_process);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }
        
        [TestCase("", "note", 1)]
        [TestCase("", "tagotest", 0)]
        [TestCase("", "NativeError", 1)]
        public void ScanWithPluginsTest(string filePattern, string fileContentPattern, int expectedMatchesCount)
        {

            _scanStrategy.SearchSettings = CreateSettings(filePattern, fileContentPattern);
            _scanStrategy.StartScan(_process);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }

        private ISearchSettings CreateSettings(string filePattern, string fileContentPattern)
        {
            var settings = TestsConfiguration.ObjectsFactory.CreateSearchSettings(
                filePattern,
                fileContentPattern, false, AppContext.PluginManager.AllPlugins);
            return settings;
        }

        private void RegScan_MatchItem(ScanData file)
        {
            _foundFiles.Add(file.FullName);
        }
    }
}
