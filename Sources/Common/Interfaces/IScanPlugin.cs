using System.Collections.Generic;

namespace Common.Interfaces
{
    public enum SearchType
    {
        ByFileAttibutes,
        ByFileContent
    }

    public interface IScanPlugin
    {
        SearchType Type { get; }
        string Name { get; }
        List<string> AssociatedFileExtensions { get; }
        bool IsForAnyFileExtension { get; }
        bool IsCorePlugin { get; }

        /// <summary>
        /// Проверить удовлетворяет ли файл условию поиска
        /// </summary>
        /// <param name="fileName"> имя файла </param>
        /// <param name="settings"> настройки системные</param>
        /// <returns>true - удовлетворяет, false - нет</returns>
        bool Check(string fileName, IScanSettings settings);
    }
}