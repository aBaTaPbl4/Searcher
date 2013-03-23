using System;
using System.Collections.Generic;
using Common.Interfaces;

namespace Searcher.VM
{
    public class SearchSettings : ISearchSettings
    {
        private string _fileNameSearchPattern;
        public string FileNameSearchPattern
        {
            get { return _fileNameSearchPattern; }
            set
            {
                if (value.Contains("*"))
                {
                    value = value.Replace("*", "");
                }
                _fileNameSearchPattern = value;
            }
        }

        public string FileContentSearchPattern { get; set; }
        public List<ISearchPlugin> ActivePlugins { get; set; }
        public bool IsMultithreadRequired { get; set; }

        public override string ToString()
        {
            return
                string.Format(
                    "Settings:{0}FileNamePattern='{1}'{0}FileContentPattern='{2}'{0}IsMultithreadRequired='{3}'{0}",
                    Environment.NewLine, FileNameSearchPattern, FileContentSearchPattern, IsMultithreadRequired);
        }

    }
}
