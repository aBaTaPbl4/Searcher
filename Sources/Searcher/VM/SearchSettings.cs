using System;
using System.Collections.Generic;
using System.IO;
using Common.Interfaces;

namespace Searcher.VM
{
    
    public class SearchSettings : ISearchSettings
    {
        private string _fileNameSearchPattern;
        private List<ISearchPlugin> _activePlugins;

        public SearchSettings()
        {
            _activePlugins = new List<ISearchPlugin>();
        }

        //todo: продумать как бы сделать валидацию покорретней
        public string FileNameSearchPattern
        {
            get { return _fileNameSearchPattern; }
            set
            {
                char emptyChar = '\0';
                foreach (var badChar in Path.GetInvalidFileNameChars())
                {
                    value = value.Replace(badChar, emptyChar);
                }

                _fileNameSearchPattern = value;
            }
        }

        //todo: нужна валидация на допустимые символы в папках
        public string FolderToScan { get; set; }

        public string FileContentSearchPattern { get; set; }
        
        public ISearchPlugin[] ActivePlugins
        {
            get { return _activePlugins.ToArray(); }
            set
            {
                _activePlugins = new List<ISearchPlugin>(value);
            }
        }

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
