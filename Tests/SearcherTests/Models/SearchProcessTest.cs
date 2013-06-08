using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using NUnit.Framework;

namespace SearcherTests.Models
{
    [TestFixture]
    public class SearchProcessTest
    {
        private SearchProcess _search;

        [SetUp]
        public void Setup()
        {
            _search = TestsConfiguration.ObjectsFactory.CreateEngine();
        }

        [Test, Description("Если не подписались на события поиска, то все равно поиск должен успешно проходить")]
        public void StartScan_WhenSearchEvents_Unhandled_Test()
        {
            Assert.IsTrue(_search.StartScan(), Log.Content);
        }

    }
}
