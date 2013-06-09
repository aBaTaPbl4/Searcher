using Common;
using Common.Interfaces;
using NUnit.Framework;
using SearchByType;

namespace SearcherTests.ServiceImpls
{
    [TestFixture]
    public class SearchByTypePluginTest
    {
        private ISearchPlugin _plugin;

        [SetUp]
        public void Setup()
        {
            _plugin = TestsConfiguration.ObjectsFactory.CreateTypePlugin();
        }

        [TestCase("ILog", false, true)] //interface
        [TestCase("FilterDecision", false, false)] // enum
        [TestCase("NativeError", false, true)] // class
        [TestCase("Dicision", false, false)] // no such class
        [TestCase("abc", false, false)] // no such class
        [TestCase("SomeType", true, false)]
        public void CheckTest(string typeToSearch, bool isNative, bool expectedResult)
        {
            var fileName = isNative ? TestHelper.NativeFileName : TestHelper.AssemblyFileName;

            var settings = TestsConfiguration.ObjectsFactory.CreateSearchSettings("", typeToSearch);
            var actualResult = _plugin.Check(fileName, settings);
            Assert.AreEqual(expectedResult, actualResult, Log.Content);
        }
    }
}
