using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;
using Common.Interfaces;

namespace ServiceImpls
{
    /// <summary>
    /// Сканирование папки с плагинами
    /// Шерстит сборку плагина
    /// Создает экземпляр плагина
    /// Хранит в себе набор всех плагинов
    /// </summary>
    public class PluginManager : IPluginManager, IDisposable
    {
        private List<ISearchPlugin> _externalPlugins;
        private ISearchPlugin _mainPlugin;
        private AppDomain _privateDomain;

        public PluginManager()
        {
            _externalPlugins = new List<ISearchPlugin>();
            _mainPlugin = new SearchByFileNamePlugin();
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

        public ISearchPlugin[] ExternalPlugins
        {
            get { return _externalPlugins.ToArray(); }
        }

        public ISearchPlugin[] AllPlugins
        {
            get
            {
                var list = new List<ISearchPlugin>();
                list.Add(_mainPlugin);
                list.AddRange(_externalPlugins);
                return list.ToArray();
            }
        }

        public ISearchPlugin MainPlugin
        {
            get { return _mainPlugin; }
        }

        Assembly LoadAssemblyToDomainIfNeeded(AppDomain domain, AssemblyName asmInfo)
        {
            var loadedAssemblyNames = (from a in domain.GetAssemblies()
                                       select a.FullName).ToArray();
            Assembly asm = null;
            if (!loadedAssemblyNames.Contains(asmInfo.FullName))
            {
                asm = AppContext.FileSystem.LoadAssemblyToDomain(domain, asmInfo);
            }
            return asm;
        }

        public void ScanPluginsFolder()
        {
            var pluginFiles = AppContext.FileSystem.GetFiles("Plugins");
            
            foreach (var pluginFileName in pluginFiles)
            {
                try
                {
                    if (!Path.GetExtension(pluginFileName).Equals(".dll", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    var asmInfo = AppContext.FileSystem.GetAssemblyInfo(pluginFileName);
                    if (asmInfo == null)
                    {
                        continue;
                    }
                    
                    var asm = LoadAssemblyToDomainIfNeeded(PrivateDomain, asmInfo);
                    bool pluginAlredyLoaded = asm == null;
                    if (pluginAlredyLoaded)
                    {
                        continue;
                    }

                    var pluginTypes = (from t in asm.GetTypes()
                                      where t.IsClass && t.GetInterfaces().Contains(typeof (ISearchPlugin))
                                      select t).ToArray();
                    foreach (var pluginType in pluginTypes)
                    {
                        _externalPlugins.Add((ISearchPlugin)Activator.CreateInstance(pluginType));
                    }

                }
                catch (Exception ex)
                {
                    AppContext.Logger.ErrorFormat("ScanPluginsFolder:Duringload plugin from assembly '{0}', error occured: {1}",
                        pluginFileName, ex);
                }
            }
        }

        public void Dispose()
        {            
            if (_privateDomain != null )
            {
                AppDomain.Unload(_privateDomain);
                _privateDomain = null;
            }
        }

    }
}
