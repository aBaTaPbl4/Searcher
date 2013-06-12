using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Common.Interfaces
{
    /// <summary>
    /// Ввел структуру в качестве dto, ввиду того, 
    /// что стандартный FileInfo позволяется слишком много
    /// </summary>
    public struct FileInfoShort
    {
        public bool IsHidden;
        public bool IsArch;
        public bool IsReadOnly;
        public DateTime ModificationDate;
        public long FileSize;
    }

    /// <summary>
    /// Изоляция приложения от жесткого диска
    /// Сервис введен для удобства мокиривония в контексте написания юнит тестов
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Получает кол-во файлов в сканируемой папке, с учетом заданных пользователем настроек
        /// </summary>
        /// <returns></returns>
        int GetFilesCountToScan();

        /// <summary>
        /// Получает список файлов лежащих в каталоге(не рекурсивно)
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        List<string> GetFiles(string folderName);

        /// <summary>
        /// Рекурсивно получаем список всех подкаталогов
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        List<string> GetAllSubfolders(string folderName);

        /// <summary>
        /// Проверяет является ли переданный файл .net сборкой, не загружая сам файл.
        /// (С жесткого диска подтягиваются только заголовки файла).
        /// Таким образом позволяет отличить нативные библиотеки от сборок
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        bool IsAssembly(string fileName);

        string FixFolderName(string folderName);
        bool FileExtists(string fileName);
        bool DirectoryExists(string dir);
        void FileDelete(string fileName);
        FileInfoShort GetFileInfo(string fileName);
        Stream GetFileStream(string fileName);
        byte[] ReadFile(string fileName);
        IScanSettings ScanSettings { get; }
    }
}