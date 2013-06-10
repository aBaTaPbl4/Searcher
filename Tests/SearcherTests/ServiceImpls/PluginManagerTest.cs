using System.Linq;
using NUnit.Framework;
using SearchByTag;
using SearchByType;
using ServiceImpls;

namespace SearcherTests.ServiceImpls
{
    internal class PluginManagerTest
    {
        private PluginManager _manager;

        [SetUp]
        public void Setup()
        {
            _manager = TestsConfiguration.ObjectsFactory.CreatePluginManagerConcrete();
        }

        [Test]
        public void ScanPluginsFolderCountTest()
        {
            _manager.ScanPluginsFolder();
            Assert.AreEqual(3, _manager.AllPlugins.Count(), Log.Content);
            Assert.IsNotNull(_manager.CorePlugins[0]);
        }

        [Test]
        public void ScanPluginsFolderContentTest()
        {
            _manager.ScanPluginsFolder();
            string[] pluginNames = (from p in _manager.AllPlugins
                                    select p.Name).ToArray();
            Assert.Contains(SearchByTagPlugin.PluginName, pluginNames, Log.Content);
            Assert.Contains(SearchByTypePlugin.PluginName, pluginNames, Log.Content);
        }
    }
}