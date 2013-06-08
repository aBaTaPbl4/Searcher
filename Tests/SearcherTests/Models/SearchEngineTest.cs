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
        private SearchProcess _search;

        [SetUp]
        public void Setup()
        {
            _search = TestsConfiguration.ObjectsFactory.CreateEngine();
        }

        [Test]
        public void StartScanWithUnhandledEventsTest()
        {
            Assert.IsFalse(_search.StartScan(), Log.Content);
        }

        [Test, Ignore("Заставить работать тест каогда буду делать UI")]
        public void StartScanWithHandledEventsTest()
        {
            Assert.IsTrue(_search.StartScan(), Log.Content);
        }
    }
}
