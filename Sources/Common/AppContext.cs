using System;
using System.Collections.Generic;
using Common.Interfaces;
using log4net;

namespace Common
{
    /// <summary>
    /// Контекст приложения. 
    /// Также реализует реестр сервисов, доступный из любой точки приложения.(aBaTaPbl4)
    /// </summary>
    public static class AppContext
    {
        private static readonly Dictionary<Type, object> _services;

        static AppContext()
        {
            _services = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Настройки поиска, указанные пользователем
        /// </summary>
        public static ISearchSettings SearchSettings
        {
            get
            {
                return _services[typeof (ISearchSettings)] as ISearchSettings;
            }
        }

        ///// <summary>
        ///// Настройки приложения
        ///// </summary>
        //public static ISettings Settings
        //{
        //    get { return _services[typeof (ISettings)] as ISettings; }
        //}

        ///// <summary>
        ///// Прокся для работы с тфс
        ///// </summary>
        //public static ITeamFoundationService Tfs
        //{
        //    get { return _services[typeof (ITeamFoundationService)] as ITeamFoundationService; }
        //}

        ///// <summary>
        ///// Получение у винды информации о tlb
        ///// </summary>
        //public static IComInformationService ComInformationService
        //{
        //    get { return _services[typeof (IComInformationService)] as IComInformationService; }
        //}

        //public static IVersionInfoFixer Fixer
        //{
        //    get { return _services[typeof (IVersionInfoFixer)] as IVersionInfoFixer; }    
        //}

        /// <summary>
        /// Регистрация сервиса приложения
        /// </summary>
        /// <param name="serviceInstance">Экземпляр сервиса</param>
        /// <param name="type">Тип сервиса через который тащить экземпляр</param>
        public static void RegisterService(object serviceInstance, Type type)
        {
            _services[type] = serviceInstance;
        }

        /// <summary>
        /// Получить экземпляр сервиса
        /// </summary>
        /// <param name="type">Тип экземпляра сервиса, который хотим получить</param>
        /// <returns></returns>
        public static Object GetServiceInstance(Type type)
        {
            return _services[type];
        }

        public static ILog Logger
        {
            get { return LogManager.GetLogger(Guid.NewGuid().ToString()); }
        }

    }
}