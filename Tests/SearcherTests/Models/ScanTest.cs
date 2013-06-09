using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using NUnit.Framework;

namespace SearcherTests.Models
{
    [TestFixture]
    public abstract class ScanTest
    {
        private SearchProcess Process;
        protected ScanStrategyBase _scan;
        private List<string> _foundFiles;

        protected abstract ScanStrategyBase CreateStrategy();

        [SetUp]
        public void Setup()
        {
            Process = new SearchProcess();
            Process.Folder = TestHelper.DeepestFolder;
            Process.FileWasFound += RegScan_MatchItem;
            _scan = CreateStrategy();
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
            
            _scan.SearchSettings = settings;
            _scan.StartScan(Process);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }
        
        [TestCase("", "note", 1)]
        [TestCase("", "tagotest", 0)]
        [TestCase("", "NativeError", 1)]
        public void ScanWithPluginsTest(string filePattern, string fileContentPattern, int expectedMatchesCount)
        {

            _scan.SearchSettings = CreateSettings(filePattern, fileContentPattern);
            _scan.StartScan(Process);
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
