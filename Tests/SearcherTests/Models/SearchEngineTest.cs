using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using NUnit.Framework;

namespace SearcherTests.Models
{
    [TestFixture]
    public class SearchEngineTest
    {
        private SearchEngine _engine;

        [SetUp]
        public void Setup()
        {
            _engine = TestsConfiguration.ObjectsFactory.CreateEngine();
        }

        [Test]
        public void StartScanTest()
        {
            Assert.IsFalse(_engine.StartScan(), Log.Content);
        }
    }
}
