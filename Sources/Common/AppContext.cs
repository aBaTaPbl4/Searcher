using System;
using System.Collections.Generic;
using Common.Interfaces;
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

        /// <summary>
        /// Шлюз к файловой системе
        /// </summary>
        public static IFileSystem FileSystem
        {
            get { return _services[typeof(IFileSystem)] as IFileSystem; }
        }

        /// <summary>
        /// Получаем доступ к информации о подгруженных плагинах
        /// </summary>
        public static IPluginManager PluginManager
        {
            get { return _services[typeof(IPluginManager)] as IPluginManager; }
        }

        /// <summary>
        /// Регистрация сервиса приложения
        /// </summary>
        /// <param name="serviceInstance">Экземпляр сервиса</param>
        /// <param name="type">Тип сервиса через который тащить экземпляр</param>
        public static void RegisterService(object serviceInstance, Type type)
        {
            if (_services.ContainsKey(type) &&
                _services[type] != serviceInstance)
            {
                ReleaseExistingInstance(type);
            }
            _services[type] = serviceInstance;
            var concreteType = serviceInstance.GetType();
            if (type != concreteType)
            {
                _services[concreteType] = serviceInstance;
            }

        }

        /// <summary>
        /// Check if needed to call Dispose
        /// </summary>
        /// <param name="serviceInstance"></param>
        /// <param name="type"></param>
        private static void ReleaseExistingInstance(Type type)
        {
            //release old service instance
            var serv = _services[type];
            _services.Remove(type);
            Type concreteType = serv.GetType();
            if (_services.ContainsKey(concreteType))
            {
                _services.Remove(concreteType);
            }
            if (concreteType.GetInterfaces().Contains(typeof(IDisposable)))
            {
                var servDisposible = serv as IDisposable;
                servDisposible.Dispose();
            }
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