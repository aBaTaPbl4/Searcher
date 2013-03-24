using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SearchByTag;
using SearchByType;
using ServiceImpls;

namespace SearcherTests.ServiceImpls
{
    class PluginManagerTest
    {
        private PluginManager _manager;

        [SetUp]
        public void Setup()
        {
            _manager = new PluginManager();    
        }

        [Test]
        public void ScanPluginsFolderCountTest()
        {
            _manager.ScanPluginsFolder();
            Assert.AreEqual(3, _manager.AllPlugins.Count(), Log.Content);
            Assert.IsNotNull(_manager.MainPlugin);

        }

        [Test]
        public void ScanPluginsFolderContentTest()
        {
            _manager.ScanPluginsFolder();
            var pluginNames = (from p in _manager.AllPlugins
                               select p.Name).ToArray();
            Assert.Contains(SearchByTagPlugin.PluginName, pluginNames, Log.Content);
            Assert.Contains(SearchByTypePlugin.PluginName, pluginNames, Log.Content);
        }
    }
}
