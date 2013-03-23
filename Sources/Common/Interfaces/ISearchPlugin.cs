using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Interfaces
{
    public enum SearchType
    {
        ByFileName,
        ByFileContent,
        ByFileNameOrContent
    }

    public interface ISearchPlugin
    {
        SearchType Type { get; }
        string Name { get; }
        List<string> AssociatedFileExtensions { get; }
        bool IsForAnyFileExtension { get; }

        /// <summary>
        /// Проверить удовлетворяет ли файл условию поиска
        /// </summary>
        /// <param name="fileName"> имя файла </param>
        /// <param name="settings"> настройки системные</param>
        /// <returns>true - удовлетворяет, false - нет</returns>
        bool Check(string fileName, ISearchSettings settings);
    }
}
