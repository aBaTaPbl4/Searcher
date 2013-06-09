using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Models;
using NUnit.Framework;
using Rhino.Mocks;

namespace SearcherTests.Models
{
    [TestFixture]
    public class SearchProcessTest
    {
        public const int FilesInFirstTestDir = 6;
        public const int DirsInFirstTestFolder = 5;
        private SearchProcess _search;
        private bool _isEventRaised;
        private int _subFolderProcessed;
        private int _lastProgressValue;
        [SetUp]
        public void Setup()
        {
            _search = TestsConfiguration.ObjectsFactory.CreateEngine();
            _isEventRaised = false;
            _subFolderProcessed = 0;
            _lastProgressValue = 0;
        }

        [Test, NUnit.Framework.Description("Если не подписались на события поиска, то все равно поиск должен успешно проходить")]
        public void StartScan_WhenSearchEvents_Unhandled_Test()
        {
            Assert.IsTrue(_search.StartScan(), Log.Content);
        }

        [Test]
        public void RaisingScanCompletionEventTest()
        {
            _search.ScanComplete += ScanCompleted;
            _search.StartScan();
            CheckEventRaising();
        }

        [Test] 
        public void RaisingProgressEventTest()
        {
            _search.ProgressChanged += ProgressChanged;
            _search.StartScan();
            CheckEventRaising();
            Assert.AreEqual(100, _lastProgressValue, Log.Content);
        }

        [Test]
        public void RaisingSubFolderScanCompleteEventTest()
        {
            _search.SubScanComplete += SubScanCompleted;
            _search.StartScan();
            CheckEventRaising();
            Assert.AreEqual(DirsInFirstTestFolder, _subFolderProcessed, Log.Content);
        }

        [Test]
        public void RaisingFoundFileEventTest()
        {
            _search.Settings.Stub(x => x.FileNameSearchPattern).Return("7z");
            _search.FileWasFound += FileWasFound;
            _search.StartScan();
            CheckEventRaising();
        }

        private void CheckEventRaising()
        {
            Assert.AreEqual(FilesInFirstTestDir, _search.FilesProcessed,"Wrong processed files number. {0}", Log.Content);
            Assert.IsTrue(_isEventRaised, Log.Content);
        }

        private void ScanCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isEventRaised = true;
        }

        private void ProgressChanged(int progress)
        {
            _lastProgressValue = progress;
            _isEventRaised = true;
        }

        private void FileWasFound(ScanData fileInfo)
        {
            _isEventRaised = true;
        }

        private void SubScanCompleted(string subFolderName)
        {
            _isEventRaised = true;
            _subFolderProcessed++;
        }

    }
}
