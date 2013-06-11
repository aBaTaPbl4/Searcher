using Common.Interfaces;
using NUnit.Framework;

namespace SearcherTests.ServiceImpls
{
    [TestFixture]
    public class SearchByTypePluginTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _plugin = TestsConfiguration.ObjectsFactory.CreateTypePlugin();
        }

        #endregion

        private ISearchPlugin _plugin;

        [TestCase("ILog", false, true)] //interface
        [TestCase("FilterDecision", false, false)] 
        [TestCase("NativeError", false, false)] // no such interface containing class
        [TestCase("Dicision", false, false)] 
        [TestCase("abc", false, false)] 
        [TestCase("SomeType", true, false)]
        public void CheckTest(string typeToSearch, bool isNative, bool expectedResult)
        {
            string fileName = isNative ? TestHelper.NativeFileName : TestHelper.AssemblyFileName;

            ISearchSettings settings = TestsConfiguration.ObjectsFactory.CreateSearchSettings("", typeToSearch);
            bool actualResult = _plugin.Check(fileName, settings);
            Assert.AreEqual(expectedResult, actualResult, Log.Content);
        }
    }
}