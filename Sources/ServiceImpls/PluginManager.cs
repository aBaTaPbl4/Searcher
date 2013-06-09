using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;
using Common.Interfaces;
using log4net;

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
        private List<ISearchPlugin> _corePlugins;
        private AppDomain _privateDomain;

        public PluginManager()
        {
            _externalPlugins = new List<ISearchPlugin>();
            _corePlugins = new List<ISearchPlugin>();
            _corePlugins.Add(new SearchByFileNamePlugin());
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

        public ISearchPlugin[] CorePlugins
        {
            get { return _corePlugins.ToArray(); }
        }

        public ISearchPlugin[] AllPlugins
        {
            get
            {
                var list = new List<ISearchPlugin>();
                list.AddRange(_corePlugins);
                list.AddRange(_externalPlugins);
                return list.ToArray();
            }
        }

        public IFileSystem FileSystem { get; set; }

        Assembly LoadAssemblyToDomainIfNeeded(AppDomain domain, AssemblyName asmInfo)
        {
            var loadedAssemblyNames = (from a in domain.GetAssemblies()
                                       select a.FullName).ToArray();
            Assembly asm = null;
            if (!loadedAssemblyNames.Contains(asmInfo.FullName))
            {
                asm = FileSystem.LoadAssemblyToDomain(domain, asmInfo);
            }
            return asm;
        }

        public void ScanPluginsFolder()
        {
            var pluginFiles = FileSystem.GetFiles("Plugins");
            
            foreach (var pluginFileName in pluginFiles)
            {
                try
                {
                    if (!Path.GetExtension(pluginFileName).Equals(".dll", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    var asmInfo = FileSystem.GetAssemblyInfo(pluginFileName);
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
                        var plug = (ISearchPlugin) Activator.CreateInstance(pluginType);
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
