using System;
using System.Collections.Generic;
using System.IO;
using Common;
using Common.Interfaces;

namespace ServiceImpls
{
    public class SearchByFileNamePlugin : ISearchPlugin
    {
        #region ISearchPlugin Members

        public SearchType Type
        {
            get { return SearchType.ByFileName; }
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
                if (settings.FileNameSearchPattern.IsNullOrEmpty())
                {
                    return true;
                }
                return Path.GetFileName(fileName).ContainsIgnoreCase(settings.FileNameSearchPattern);
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
            get { return "SearchByFileNamePlugin"; }
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