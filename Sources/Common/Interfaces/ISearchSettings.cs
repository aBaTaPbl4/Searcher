using System;

namespace Common.Interfaces
{
    /// <summary>
    /// Настройки поиска
    /// (позволяет только читать данные о настройках указанных пользователем)
    /// </summary>
    public interface ISearchSettings
    {
        string FileNameSearchPattern { get; }
        string FileContentSearchPattern { get; }
        ISearchPlugin[] ActivePlugins { get; }
        string FolderToScan { get; }
        bool RecursiveScan { get; }
        bool? IsHidden { get; }
        bool? IsArch { get; }
        bool? IsReadOnly { get; }
        DateTime? MinModificationDate { get; }

        /// <summary>
        /// Минимальный размер файла в кб
        /// </summary>
        long MinFileSize { get; }
    }
}