using System;
using System.Collections.Generic;
using Common.Interfaces;
using Spring.Context;
using Spring.Context.Support;
using log4net;
using System.Linq;

namespace Common
{
    /// <summary>
    /// Контекст приложения. 
    /// Является оберткой над Spring.Net IoC
    /// </summary>
    public static class AppContext
    {
        private static IApplicationContext _springContext;

        static AppContext()
        {
            string contextName;
#if UNIT_TESTS
            contextName = "Isolation";
#else
            contextName = "Integration";
#endif
            _springContext = ContextRegistry.GetContext(contextName);
        }

        /// <summary>
        /// Настройки поиска, указанные пользователем
        /// </summary>
        public static ISearchSettings SearchSettings
        {
            get
            {
                return _springContext.GetObject<ISearchSettings>();
            }
        }

        /// <summary>
        /// Настройки программы
        /// </summary>
        public static IProgramSettings ProgramSettings
        {
            get { return _springContext.GetObject<IProgramSettings>(); }
        }

        /// <summary>
        /// Шлюз к файловой системе
        /// </summary>
        public static IFileSystem FileSystem
        {
            get { return _springContext.GetObject<IFileSystem>(); }
        }

        /// <summary>
        /// Получаем доступ к информации о подгруженных плагинах
        /// </summary>
        public static IPluginManager PluginManager
        {
            get { return _springContext.GetObject<IPluginManager>(); }
        }


        /// <summary>
        /// Получить экземпляр сервиса
        /// </summary>
        /// <param name="type">Тип экземпляра сервиса, который хотим получить</param>
        /// <returns></returns>
        public static T GetObject<T>()
        {
            return _springContext.GetObject<T>();
        }

        public static ILog Logger
        {
            get { return LogManager.GetLogger(Guid.NewGuid().ToString()); }
        }

    }
}