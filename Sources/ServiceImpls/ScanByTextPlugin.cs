using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common;
using Common.Interfaces;

namespace ServiceImpls
{
    /// <summary>
    /// Плагин поиска по содержимому текстовых файлов
    /// </summary>
    public class ScanByTextPlugin : IScanPlugin
    {
        public const string PluginName = "Search by text in txt files";
        private IFileSystem _fileSystem;

        private IFileSystem FileSystem
        {
            get
            {
                if (_fileSystem == null)
                {
                    return AppContext.FileSystem;
                }
                return _fileSystem;
            }
            set { _fileSystem = value; }
        }

        #region IScanPlugin Members

        public SearchType Type
        {
            get { return SearchType.ByFileContent; }
        }

        public bool IsCorePlugin
        {
            get { return false; }
        }

        public bool Check(string fileName, IScanSettings settings, IFileSystem fileSystem)
        {
            try
            {
                FileSystem = fileSystem;
                bool checkRequired = !settings.FileContentSearchPattern.IsNullOrEmpty();

                if (!checkRequired)
                {
                    return true;
                }

                bool fileHasDiferentExtension = !AssociatedFileExtensions.Contains(Path.GetExtension(fileName));

                if (fileHasDiferentExtension)
                {
                    return false;
                }
                var fileEncoding = GetFileEncoding(fileName);
                //Стандартное определение кодировки в классе StreamReader работает коряво
                using (var reader = new StreamReader(FileSystem.GetFileStream(fileName), fileEncoding))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.ContainsIgnoreCase(settings.FileContentSearchPattern))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// Проблемма с тем чтобы отличить кодировку win от dos.
        /// Ввиду того что сам файл не содержит инфы о кодировке.
        /// Поэтому метод в обоих случаях возращаеть 1251 кодировку
        /// </summary>
        /// <param name="srcFile">Файл кодировку которого нужно определить</param>
        /// <returns>Если кодировку определить не удалось возращается Windows-1251</returns>
        private Encoding GetFileEncoding(string srcFile)
        {
            // *** Use Default of Encoding.Default (Ansi CodePage)
            Encoding enc = Encoding.Default;

            // *** Detect byte order mark if any - otherwise assume default
            var buffer = new byte[5];
            using (var fs = FileSystem.GetFileStream(srcFile))
            {
                fs.Read(buffer, 0, 5);                
            }

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;
            else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                // 1201 unicodeFFFE Unicode (Big-Endian)
                enc = Encoding.GetEncoding(1201);
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                // 1200 utf-16 Unicode
                enc = Encoding.GetEncoding(1200);
            else
                enc = Encoding.GetEncoding(1251);

            return enc;
        }

        public string Name
        {
            get { return PluginName; }
        }

        public List<string> AssociatedFileExtensions
        {
            get { return new List<string> { ".txt" }; }
        }

        public bool IsForAnyFileExtension
        {
            get { return false; }
        }

        #endregion
    }
}
