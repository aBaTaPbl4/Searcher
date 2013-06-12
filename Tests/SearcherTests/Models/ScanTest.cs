using System;
using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Models;
using Models.ScanStrategies;
using NUnit.Framework;
using Rhino.Mocks;
using Searcher.VM;

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
        private Scan _process;
        protected ScanStrategyBase _scanStrategy;
        private List<string> _foundFiles;
        private FileInfoShort _fileInfo;
        private IFileSystem _fs;
        const int _fileSizeToReturn = 500;
        private readonly DateTime _modificationDateToReturn;
        #region Setup/Teardown

        protected  ScanTest()
        {
            _modificationDateToReturn = new DateTime(2013, 6, 9);
        }

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

            _fileInfo = new FileInfoShort();
            _fileInfo.FileSize = _fileSizeToReturn;
            _fileInfo.ModificationDate = _modificationDateToReturn;
            _fileInfo.IsHidden = _fileInfo.IsArch = _fileInfo.IsReadOnly = false;

            _fs = TestsConfiguration.ObjectsFactory.CreateFileSystem();
            _fs.Stub(x => x.GetFileInfo(null)).IgnoreArguments().Return(_fileInfo);
        }

        #endregion

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

        [TestCase(false,false,false, 1)]
        [TestCase(false, false, null, 1)]
        [TestCase(true, false, false, 0)]
        [TestCase(false, true, false, 0)]
        [TestCase(false, false, true, 0)]
        public void ScanByFileAttributes_FlagsCheck_Test(bool? isReadonly, bool isHidden, bool isArch, int expectedMatchesCount)
        {
            var settings = TestsConfiguration.ObjectsFactory.CreateScanSettings();
            settings.Stub(x => x.IsReadOnly).Return(isReadonly);
            settings.Stub(x => x.IsHidden).Return(isHidden);
            settings.Stub(x => x.IsArch).Return(isArch);

            _fs.Stub(x => x.ScanSettings).Return(settings);
            _scanStrategy.ScanSettings = settings;
            _scanStrategy.FileSystem = _fs;
            _scanStrategy.StartScan(_process);

            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }

        [TestCase("09.06.2013", 1)]
        [TestCase("08.06.2013", 1)]
        [TestCase("10.06.2013", 0)]
        [TestCase("", 1)]
        [TestCase(null, 1)]
        public void ScanByFileAttributes_ModificationDate_Test(string minModificationDate, int expectedMatchesCount)
        {
            var settings = TestsConfiguration.ObjectsFactory.CreateScanSettings();
            var dt = minModificationDate.IsNullOrEmpty() ? null : (DateTime?)DateTime.Parse(minModificationDate);
            settings.Stub(x => x.MinModificationDate).Return(dt);
            _fs.Stub(x => x.ScanSettings).Return(settings);
            _scanStrategy.ScanSettings = settings;
            _scanStrategy.FileSystem = _fs;
            _scanStrategy.StartScan(_process);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count, Log.Content);
        }

        [TestCase(_fileSizeToReturn - 100, 1)]
        [TestCase(_fileSizeToReturn, 1)]
        [TestCase(_fileSizeToReturn + 100, 0)]
        public void ScanByFileAttributes_MinSize_Test(int minSize, int expectedMatchesCount)
        {
            var settings = TestsConfiguration.ObjectsFactory.CreateScanSettings();
            settings.Stub(x => x.MinFileSize).Return(minSize);
            _fs.Stub(x => x.ScanSettings).Return(settings);
            _scanStrategy.ScanSettings = settings;
            _scanStrategy.FileSystem = _fs;
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
                    TestHelper.GetCoreAndXmlPlugin());


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
                    TestHelper.GetCoreAndTypePlugin());

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
                    TestHelper.GetCoreAndTextPlugin());
            _scanStrategy.StartScan(_process);
            Assert.AreEqual(expectedMatchesCount, _foundFiles.Count,"Error for word '{0}'. {1}", fileContentPattern, Log.Content);
        }


        private void RegScan_MatchItem(ScanData file)
        {
            _foundFiles.Add(file.FullName);
        }
    }
}