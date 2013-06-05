using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Common;
using Common.Interfaces;

namespace Searcher.VM
{
    
    public class ScanOptionVM : ISearchSettings
    {
        private string _fileNameSearchPattern;
        private List<ISearchPlugin> _activePlugins;

        public ScanOptionVM()
        {
            _activePlugins = new List<ISearchPlugin>();
            OrLink = true;
        }

        public bool OrLink { get; set; }

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
            get { return Plugins.Where(x => x.IsActive).ToArray() as ISearchPlugin[]; }
            set
            {
                //todo:закрыть публичный доступ
                _activePlugins = new List<ISearchPlugin>(value);
            }
        }
        
        public ObservableCollection<PluginDecoratorVM> Plugins { get; private set; }

        public void InitPlugins()
        {
            Plugins = new ObservableCollection<PluginDecoratorVM>();
            foreach (var plugin in AppContext.PluginManager.ExternalPlugins)
            {
                Plugins.Add(new PluginDecoratorVM()
                {
                    IsActive = false,
                    Plugin = plugin
                });
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
