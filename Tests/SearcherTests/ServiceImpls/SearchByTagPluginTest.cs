using Common.Interfaces;
using Rhino.Mocks;
using NUnit.Framework;
using SearchByTag;
using ServiceImpls;

namespace SearcherTests.ServiceImpls
{
    [TestFixture]
    public class SearchByTagPluginTest
    {
        private ISearchPlugin _plugin;

        [SetUp]
        public void Setup()
        {
            _plugin = new SearchByTagPlugin();
        }

        [TestCase("not", false)]
        [TestCase("note", true)]
        [TestCase("heading", true)]
        public void CheckTest(string tagName, bool expectedResult)
        {
            var settings = FakeObjectsFactory.CreateSettings("",tagName);
            var actualResult = _plugin.Check(TestHelper.XmlFileName, settings);
            Assert.AreEqual(expectedResult, actualResult, Log.Content);
        }
    }
}
