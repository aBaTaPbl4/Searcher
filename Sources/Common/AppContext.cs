using System;
using Common.Interfaces;
using Spring.Context;
using Spring.Context.Support;
using log4net;

namespace Common
{
    /// <summary>
    /// Контекст приложения. 
    /// Является оберткой над Spring.Net IoC.
    /// Существует три вида контекстов:
    /// 1) Реальное приложение - контекст не изолирован.
    /// 2) Интеграционные тесты - контекст не изолирован.
    /// 3) Модульные тесты, - контекст изолирует приложения от жесткого диска, сети, бд...
    /// </summary>
    public static class AppContext
    {
        private static readonly IApplicationContext _springContext;

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
        public static IScanSettings ScanSettings
        {
            get { return _springContext.GetObject<IScanSettings>(); }
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


        public static ILog Logger
        {
            get { return LogManager.GetLogger(Guid.NewGuid().ToString()); }
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
    }
}