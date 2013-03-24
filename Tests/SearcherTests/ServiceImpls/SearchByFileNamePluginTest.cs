using Common.Interfaces;
using Rhino.Mocks;
using NUnit.Framework;
using ServiceImpls;

namespace SearcherTests.ServiceImpls
{
    [TestFixture]
    public class SearchByFileNamePluginTest
    {
        private ISearchPlugin _plugin;

        [SetUp]
        public void Setup()
        {
            _plugin = TestsConfiguration.ObjectsFactory.CreateFileNamePlugin();
        }

        [TestCase("note.xml", true)]
        [TestCase("note", true)]
        [TestCase("note.", true)]
        [TestCase("not", true)]
        [TestCase(".xml", true)]
        [TestCase("abc", false)]
        [TestCase("note.xmlns", false)]
        public void CheckTest(string fileNamePattern, bool expectedResult)
        {
            var serv = TestsConfiguration.ObjectsFactory.CreateSettings(fileNamePattern);
            var actualResult = _plugin.Check(TestHelper.XmlFileName, serv);
            Assert.AreEqual(expectedResult, actualResult, Log.Content);
        }
    }
}
