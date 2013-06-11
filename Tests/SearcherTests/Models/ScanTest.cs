using System.Collections.Generic;
using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using NUnit.Framework;
using Rhino.Mocks;

namespace SearcherTests.Models
{
    /// <summary>
    /// Абстрактная фикстура
    /// Тесты сканирования запускаются по одному разу на каждую стратегию сканирования.
    /// При этом само собой резульаты должны быть одинаковыми вне зависимости от стратегии,
    /// поэтому все тесты определены в этом классе. В потомках только переопределен метод создания стратегии
    /// </summary>
    [TestFixture]
    public abstract class ScanTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _process = TestsConfiguration.ObjectsFactory.CreateScan();
            _process.Stub(x => x.IsNeedCancelation).Return(false);
            _process.Stub(x => x.CancelationOccured());
            _process.Folder = TestHelper.DeepestFolder;
            _process.FileWasFound += RegScan_MatchItem;
            _scanStrategy = CreateStrategy();
            _foundFiles = new List<string>();
        }

        #endregion

        private Scan _process;
        protected ScanStrategyBase _scanStrategy;
        private List<string> _foundFiles;

        /// <summary>
        /// Метод переопределен в потомке
        /// </summary>
        /// <returns></returns>
        protected abstract ScanStrategyBase CreateStrategy();

        [TestCase("7z", "", 1)]
        [TestCase("test", "", 3)]
        [TestCase("note", "", 2)]
        [TestCase("", "", TestHelper.FilesInFirstTestDir)]
        [TestCase("", "note", TestHelper.FilesInFirstTestDir)]
        public void ScanByFileAttributesTest(string filePattern, string fileContentPattern, int expectedMatchesCount)
        {
            IScanSettings settings = TestsConfiguration.ObjectsFactory.CreateScanSettings(filePattern,
                                                                                              fileContentPattern);

            _scanStrategy.ScanSettings = settings;
            _scanStrategy.StartScan(_process);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }

        [TestCase("", "note", 1)]
        [TestCase("", "tagotest", 0)]
        public void ScanWithXmlPluginTest(string filePattern, string fileContentPattern, int expectedMatchesCount)
        {
            _scanStrategy.ScanSettings =
                TestsConfiguration.ObjectsFactory.CreateScanSettings(
                    filePattern, fileContentPattern,
                    false, TestHelper.GetCoreAndXmlPlugin());

            _scanStrategy.StartScan(_process);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }

        [TestCase("", "ILog", 2)] //log4net + Common.Logging
        [TestCase("", "IInterceptor", 1), Description("сборка должна загружаться если рефы лежат рядом")]
        public void ScanWithAssemblyPluginTest(string filePattern, string fileContentPattern, int expectedMatchesCount)
        {
            _scanStrategy.ScanSettings =
                TestsConfiguration.ObjectsFactory.CreateScanSettings(
                    filePattern, fileContentPattern,
                    false, TestHelper.GetCoreAndTypePlugin());

            _scanStrategy.StartScan(_process);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }

        [TestCase("", "один", 2), Description("В dos файле не найдет")]
        [TestCase("", "два", 1)]//utf-8
        [TestCase("", "три", 1)]//win
        [TestCase("", "четыре", 0)]//dos
        [TestCase("", "one", 3)]
        [TestCase("", "two", 1)]//utf-8
        [TestCase("", "free", 1)]//win
        [TestCase("", "four", 1)]//dos
        public void ScanWithTextPluginTest(string filePattern, string fileContentPattern, int expectedMatchesCount)
        {
            _scanStrategy.ScanSettings =
                TestsConfiguration.ObjectsFactory.CreateScanSettings(
                    filePattern, fileContentPattern,
                    false, TestHelper.GetCoreAndTextPlugin());
            _scanStrategy.StartScan(_process);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count,"Error for word '{0}'. {1}", fileContentPattern, Log.Content);
        }


        private void RegScan_MatchItem(ScanData file)
        {
            _foundFiles.Add(file.FullName);
        }
    }
}