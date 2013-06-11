using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;

namespace ServiceImpls
{
    public class SearchByTextPlugin
    {
        public const string PluginName = "Search by text in txt files";

        #region ISearchPlugin Members

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
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
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
