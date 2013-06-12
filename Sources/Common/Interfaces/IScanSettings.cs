using System;

namespace Common.Interfaces
{
    /// <summary>
    /// Настройки поиска
    /// Данные открыты только на чтение
    /// </summary>
    public interface IScanSettings
    {
        string FileNameSearchPattern { get; }
        string FileContentSearchPattern { get; }
        IScanPlugin[] ActivePlugins { get; }
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