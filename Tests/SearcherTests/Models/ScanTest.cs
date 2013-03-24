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
        private SearchEngine _engine;
        protected ScanStrategyBase _scan;
        private List<string> _foundFiles;

        protected abstract ScanStrategyBase CreateStrategy();

        [SetUp]
        public void Setup()
        {
            _engine = new SearchEngine();
            _engine.Folder = TestHelper.DeepestFolder;
            _engine.MatchItem += RegScan_MatchItem;
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
            var settings = TestsConfiguration.ObjectsFactory.CreateSettings(
                filePattern,
                fileContentPattern,
                false,
                new ISearchPlugin[] {AppContext.PluginManager.MainPlugin});

            AppContext.RegisterService(settings, typeof (ISearchSettings));
            _scan.StartScan(_engine);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }
        
        [TestCase("", "note", 1)]
        [TestCase("", "tagotest", 0)]
        [TestCase("", "NativeError", 1)]
        public void ScanWithPluginsTest(string filePattern, string fileContentPattern, int expectedMatchesCount)
        {
            var settings = TestsConfiguration.ObjectsFactory.CreateSettings(
                filePattern,
                fileContentPattern);
            AppContext.RegisterService(settings, typeof(ISearchSettings));
            _scan.StartScan(_engine);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }

        private void RegScan_MatchItem(string file)
        {
            _foundFiles.Add(file);
        }
    }
}
