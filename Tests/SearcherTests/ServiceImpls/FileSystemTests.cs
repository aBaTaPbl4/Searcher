﻿using System;
using System.Collections.Generic;
using Common.Interfaces;
using NUnit.Framework;

namespace SearcherTests.ServiceImpls
{
    [TestFixture]
    public class FileSystemTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _fs = TestsConfiguration.ObjectsFactory.CreateFileSystem();
        }

        [TearDown]
        public void Teardown()
        {
            Log.Clear();
        }

        #endregion

        private IFileSystem _fs;


        [TestCase("1", 4)]
        [TestCase("6", 0)]
        [TestCase("7", 0)]
        [TestCase(@"1\2", 2)]
        public void GetAllSubfolders_ReturnsCorrentFoldersCount_Test(string rootFolderName, int expectedSubfoldersCount)
        {
            List<string> subfolders = _fs.GetAllSubfolders(rootFolderName);
            Assert.AreEqual(expectedSubfoldersCount, subfolders.Count, Log.Content);
        }

        [Test]
        public void GetAllSubfolders_ReturnsCorrectFolders_Test()
        {
            List<string> subfolders = _fs.GetAllSubfolders("1");
            string fmt = string.Format(
                "{0}Expected contains folder but it does not. Log content: {0}{{0}}", Environment.NewLine);

            Assert.IsTrue(subfolders.ContainsSimilarElement(@"\2"), fmt, Log.Content);
            Assert.IsTrue(subfolders.ContainsSimilarElement(@"\3"), fmt, Log.Content);
            Assert.IsTrue(subfolders.ContainsSimilarElement(@"\4"), fmt, Log.Content);
            Assert.IsTrue(subfolders.ContainsSimilarElement(@"\5"), fmt, Log.Content);
        }

        [Test]
        public void GetFilesTest()
        {
            List<string> fileNames = _fs.GetFiles("7");
            Assert.AreEqual(2, fileNames.Count, Log.Content);
            Assert.IsTrue(fileNames.ContainsSimilarElement("gitscc.log"));
            Assert.IsTrue(fileNames.ContainsSimilarElement("portfolio.png"));
        }
    }
}