using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Common;
using Common.Interfaces;

namespace SearchByTag
{
    /// <summary>
    /// Плагин поиска по тэгу
    /// </summary>
    public class ScanByTagPlugin : IScanPlugin
    {
        public const string PluginName = "Search by tag in xml files";

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
                bool checkRequired = !settings.FileContentSearchPattern.IsNullOrEmpty();

                if (!checkRequired)
                {
                    return true;
                }

                XDocument doc = null;
                using (var fs = fileSystem.GetFileStream(fileName))
                {
                    doc = XDocument.Load(fs);
                }
                if (doc == null)
                {
                    return false;
                }
                if (doc.Root.Name == settings.FileContentSearchPattern)
                {
                    return true;
                }
                XElement element = doc.Root.Element(settings.FileContentSearchPattern);
                return element != null;
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
            get { return PluginName; }
        }

        public List<string> AssociatedFileExtensions
        {
            get { return new List<string> {".xml"}; }
        }

        public bool IsForAnyFileExtension
        {
            get { return false; }
        }

        #endregion
    }
}