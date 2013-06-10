using System;
using System.Collections.Generic;
using System.IO;
using Common;
using Common.Interfaces;

namespace ServiceImpls
{
    public class SearchByFileAttributesPlugin : ISearchPlugin
    {
        #region ISearchPlugin Members

        public SearchType Type
        {
            get { return SearchType.ByFileAttibutes; }
        }

        /// <summary>
        /// Проверить удовлетворяет ли файл условию поиска
        /// </summary>
        /// <param name="fileName"> имя файла </param>
        /// <param name="settings"> настройки системные</param>
        /// <returns>true - удовлетворяет, false - нет</returns>
        public bool Check(string fileName, ISearchSettings settings)
        {
            try
            {
                bool fileNameCheckRequired = !settings.FileNameSearchPattern.IsNullOrEmpty();
                bool fileNameMatchToPattern = Path.GetFileName(fileName).ContainsIgnoreCase(settings.FileNameSearchPattern);

                if (fileNameCheckRequired && !fileNameMatchToPattern)
                {
                    return false;
                }
                var fileInfo = AppContext.FileSystem.GetFileInfo(fileName);
                if (settings.MinFileSize > 0 && fileInfo.FileSize < settings.MinFileSize)
                {
                    return false;
                }

                if (settings.MinModificationDate != null && fileInfo.ModificationDate < settings.MinModificationDate)
                {
                    return false;
                }

                if (settings.IsReadOnly != null && settings.IsReadOnly != fileInfo.IsReadOnly)
                {
                    return false;
                }

                if (settings.IsHidden != null && settings.IsHidden!= fileInfo.IsHidden)
                {
                    return false;
                }

                if (settings.IsArch != null && settings.IsArch != fileInfo.IsArch)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                AppContext.Logger.ErrorFormat("{3}.Check: FileName: '{0}', Settings: '{1}'. Exception occured: {2}",
                                              fileName, settings, ex, Name);
                return false;
            }
        }

        public string Name
        {
            get { return "Search by file attributes"; }
        }

        public List<string> AssociatedFileExtensions
        {
            get { return new List<string>(); }
        }

        public bool IsCorePlugin
        {
            get { return true; }
        }

        public bool IsForAnyFileExtension
        {
            get { return true; }
        }

        #endregion
    }
}