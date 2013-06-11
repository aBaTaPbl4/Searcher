using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Common;
using Common.Interfaces;

namespace ServiceImpls
{
    public class PluginManager : IPluginManager, IDisposable
    {
        private readonly List<IScanPlugin> _corePlugins;        
        private readonly List<IScanPlugin> _externalPlugins;
        private AppDomain _privateDomain;

        public PluginManager()
        {
            _externalPlugins = new List<IScanPlugin>();
            _externalPlugins.Add(new ScanByTextPlugin());
            _corePlugins = new List<IScanPlugin>();
            _corePlugins.Add(new ScanByFileAttributesPlugin());
        }

        public AppDomain PrivateDomain
        {
            get
            {
                if (_privateDomain == null)
                {
                    _privateDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null);
                }
                return _privateDomain;
            }
            set
            {
                if (_privateDomain == value)
                {
                    return;
                }
                if (_privateDomain != null)
                {
                    AppDomain.Unload(_privateDomain);
                }
                _privateDomain = value;
            }
        }

        private IFileSystem _fileSystem;
        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
            set
            {
                _fileSystem = value;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_privateDomain != null)
            {
                AppDomain.Unload(_privateDomain);
                _privateDomain = null;
            }
        }

        #endregion

        #region IPluginManager Members

        public IScanPlugin[] ExternalPlugins
        {
            get { return _externalPlugins.ToArray(); }
        }

        public IScanPlugin[] CorePlugins
        {
            get { return _corePlugins.ToArray(); }
        }

        public IScanPlugin[] AllPlugins
        {
            get
            {
                var list = new List<IScanPlugin>();
                list.AddRange(_corePlugins);
                list.AddRange(_externalPlugins);
                return list.ToArray();
            }
        }

        #endregion

        private Assembly LoadPluginIfNeeded(string pluginLocation)
        {

            string[] loadedAssemblyFileNames = (from a in PrivateDomain.GetAssemblies()
                                            select Path.GetFileName(a.Location)).ToArray();
            Assembly asm = null;
            string pluginFileName = Path.GetFileName(pluginLocation);
            bool isAssemblyLoaded = loadedAssemblyFileNames.ContainsIgnoreCase(pluginFileName);

            if (!isAssemblyLoaded)
            {
                //обязатель this.FileSystem, потому что экземпляр FileSystem еще не зарега в контейнере!
                asm = PrivateDomain.LoadAssembly(pluginLocation, this.FileSystem);
            }
            return asm;
        }

        public string PluginFolder
        {
            get { return Path.Combine(Environment.CurrentDirectory, "Plugins"); }
        }

        public void ScanPluginsFolder()
        {
            List<string> pluginFiles = FileSystem.GetFiles("Plugins");

            foreach (string pluginLocation in pluginFiles)
            {
                try
                {
                    if (!Path.GetExtension(pluginLocation).Equals(".dll", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    if (!FileSystem.IsAssembly(pluginLocation))
                    {
                        continue;
                    }
                    Assembly asm = LoadPluginIfNeeded(pluginLocation);
                    bool pluginAlredyLoaded = asm == null;
                    if (pluginAlredyLoaded)
                    {
                        continue;
                    }

                    Type[] pluginTypes = (from t in asm.GetTypes()
                                          where t.IsClass && t.GetInterfaces().Contains(typeof (IScanPlugin))
                                          select t).ToArray();
                    foreach (Type pluginType in pluginTypes)
                    {
                        var plug = (IScanPlugin) Activator.CreateInstance(pluginType);
                        if (plug.IsCorePlugin)
                        {
                            _corePlugins.Add(plug);
                        }
                        else
                        {
                            _externalPlugins.Add(plug);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppContext.Logger.ErrorFormat(
                        "ScanPluginsFolder:During load plugin from assembly '{0}', error occured: {1}",
                        pluginLocation, ex);
                }
            }
        }
    }
}