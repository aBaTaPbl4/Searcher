namespace Common.Interfaces
{
    /// <summary>
    /// Сканирование папки с плагинами
    /// Шерстит сборку плагина
    /// Создает экземпляр плагина
    /// Хранит в себе набор всех плагинов
    /// </summary>
    public interface IPluginManager
    {
        IScanPlugin[] AllPlugins { get; }

        /// <summary>
        ///отключаемые плагины. Отключение происходит по выбору пользователя из UI
        /// </summary>
        IScanPlugin[] ExternalPlugins { get; }
        
        /// <summary>
        /// Плагины ядра, отрабатывают при поиске всегда
        /// </summary>
        IScanPlugin[] CorePlugins { get; }

        string PluginFolder { get; }

        void ScanPluginsFolder();
    }
}