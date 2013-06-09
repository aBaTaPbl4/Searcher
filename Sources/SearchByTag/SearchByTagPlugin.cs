using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Common;
using Common.Interfaces;

namespace SearchByTag
{
    public class SearchByTagPlugin : ISearchPlugin
    {
        public const string PluginName = "SearchByTagPlugin";

        public SearchType Type
        {
            get { return SearchType.ByFileContent; }
        }


        public bool IsCorePlugin
        {
            get { return false; }
        }

        public bool Check(string fileName, ISearchSettings settings)
        {
            try
            {
                if (settings.FileContentSearchPattern.IsNullOrEmpty() ||
                    !AssociatedFileExtensions.Contains(Path.GetExtension(fileName)))
                {
                    return false;
                }
                var doc = XDocument.Load(fileName);
                if (doc.Root.Name == settings.FileContentSearchPattern)
                {
                    return true;
                }
                var element = doc.Root.Element(settings.FileContentSearchPattern);
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
            get { return new List<string>() {".xml"} ;
            }
        }

        public bool IsForAnyFileExtension
        {
            get
            {
                return false;
            }
        }
    }
}
