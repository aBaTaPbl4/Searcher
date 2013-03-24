using System;
using System.Collections.Generic;
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
        private List<ISearchPlugin> _plugins;
        private ISearchPlugin _mainPlugin;
        private AppDomain _privateDomain;

        public PluginManager()
        {
            _plugins = new List<ISearchPlugin>();
            _mainPlugin = new SearchByFileNamePlugin();
            _plugins.Add(_mainPlugin);
            _privateDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null);
        }

        public ISearchPlugin[] AllPlugins
        {
            get { return _plugins.ToArray(); }
        }

        public ISearchPlugin MainPlugin
        {
            get { return _mainPlugin; }
        }

        public void ScanPluginsFolder()
        {
            var pluginFiles = AppContext.FileSystem.GetFiles("Plugins");
            foreach (var pluginFileName in pluginFiles)
            {
                try
                {
                    var asmInfo = AppContext.FileSystem.GetAssemblyInfo(pluginFileName);
                    if (asmInfo == null)
                    {
                        continue;
                    }
                    var loadedAssemblyNames = (from a in _privateDomain.GetAssemblies()
                                               select a.FullName).ToArray();
                    Assembly asm;
                    if (!loadedAssemblyNames.Contains(asmInfo.FullName))
                    {
                        asm = AppContext.FileSystem.LoadAssemblyToDomain(_privateDomain, asmInfo);    
                    }
                    else
                    {
                        continue;
                    }
                    var pluginTypes = (from t in asm.GetTypes()
                                      where t.IsClass && t.GetInterfaces().Contains(typeof (ISearchPlugin))
                                      select t).ToArray();
                    foreach (var pluginType in pluginTypes)
                    {
                        _plugins.Add((ISearchPlugin)Activator.CreateInstance(pluginType));
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
