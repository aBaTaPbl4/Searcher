using Common.Interfaces;
using NUnit.Framework;

namespace SearcherTests.ServiceImpls
{
    [TestFixture]
    public class SearchByFileNamePluginTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _plugin = TestsConfiguration.ObjectsFactory.CreateFileNamePlugin();
        }

        #endregion

        private ISearchPlugin _plugin;

        [TestCase("note.xml", true)]
        [TestCase("note", true)]
        [TestCase("note.", true)]
        [TestCase("not", true)]
        [TestCase(".xml", true)]
        [TestCase("abc", false)]
        [TestCase("note.xmlns", false)]
        public void CheckTest(string fileNamePattern, bool expectedResult)
        {
            ISearchSettings serv = TestsConfiguration.ObjectsFactory.CreateSearchSettings(fileNamePattern);
            bool actualResult = _plugin.Check(TestHelper.XmlFileName, serv);
            Assert.AreEqual(expectedResult, actualResult, Log.Content);
        }
    }
}