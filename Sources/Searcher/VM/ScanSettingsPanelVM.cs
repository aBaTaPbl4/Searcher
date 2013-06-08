using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Common;
using Common.Interfaces;

namespace Searcher.VM
{
    
    public class ScanSettingsPanelVM : ISearchSettings, IDataErrorInfo
    {
        private string _fileNameSearchPattern;
        private string _folderToScan;

        public ScanSettingsPanelVM()
        {
            OrLink = true;
            IsHidden = null;
            IsArch = null;
            IsReadOnly = null;
            MinModificationDate = new DateTime(1955, 1, 1);
            MinFileSize = 0;
        }

        public bool OrLink { get; set; }
        public bool? IsHidden { get; set; }
        public bool? IsArch { get; set; }
        public bool? IsReadOnly{ get; set; }
        public DateTime MinModificationDate { get; set; }
        /// <summary>
        /// Минимальный размер файла в кб
        /// </summary>
        public int MinFileSize { get; set; }
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
        public string FolderToScan
        {
            get
            {
                return _folderToScan;
            }
            set
            {
                _folderToScan = value;
            }
        }

        public string FileContentSearchPattern { get; set; }

        public void SetActivePlugin(PluginDecoratorVM activePlugin)
        {
            foreach (var curPlugin in Plugins)
            {
                if (curPlugin == activePlugin)
                {
                    curPlugin.IsActive = true;
                }
                else
                {
                    curPlugin.IsActive = false;
                }
            }
        }

        public virtual ISearchPlugin[] ActivePlugins
        {
            get
            {
                return Plugins.Where(x => x.IsActive).Select(x=>x.Plugin).ToArray();
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
                    "Settings:{0}FileNamePattern='{1}'{0}FileContentPattern='{2}'{0}IIsMultithreadRequired='{3}'{0}",
                    Environment.NewLine, FileNameSearchPattern, FileContentSearchPattern, IsMultithreadRequired);
        }

        public string this[string name]
        {
            get 
            { 
                string result = null;
                if (name == "FileNameSearchPattern")
                {
                    foreach (var wrongChar in Path.GetInvalidPathChars())
                    {
                        if (_fileNameSearchPattern.IsNullOrEmpty())
                        {
                            return null;
                        }
                        if (_fileNameSearchPattern.Contains(wrongChar))
                        {
                            result = string.Format("File name contains wrong character '{0}'!!!", wrongChar);
                        }
                    }
                }
                else if (name == "FolderToScan")
                {
                    if (_folderToScan.IsNullOrEmpty())
                    {
                        return null;
                    }

                    bool folderIsNotExists = !this._folderToScan.IsNullOrEmpty() &&
                                             !AppContext.FileSystem.DirectoryExists(this._folderToScan);
                    if (folderIsNotExists)
                    {
                        result = String.Format("Directory '{0}' does not exists!", this._folderToScan);
                    }               
                }
                return result;
            }
        }

        public string Error
        {
            get { return null; }
        }
    }
}
