using Common;
using Common.Interfaces;
using NUnit.Framework;

namespace SearcherTests.ServiceImpls
{
    [TestFixture]
    public class SearchByTagPluginTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _plugin = TestsConfiguration.ObjectsFactory.CreateTagPlugin();
        }

        #endregion

        private IScanPlugin _plugin;

        [TestCase("not", false)]
        [TestCase("note", true)]
        [TestCase("heading", true)]
        public void CheckTest(string tagName, bool expectedResult)
        {
            IScanSettings settings = TestsConfiguration.ObjectsFactory.CreateScanSettings("", tagName);
            bool actualResult = _plugin.Check(TestHelper.XmlFileName, settings, AppContext.FileSystem);
            Assert.AreEqual(expectedResult, actualResult, Log.Content);
        }
    }
}