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
        private IPluginManager _pluginManager;
        private IFileSystem _fileSystem;
        private ObservableCollection<PluginDecoratorVM> _plugins;

        public ScanSettingsPanelVM()
        {
            OrLink = true;
            IsHidden = null;
            IsArch = null;
            IsReadOnly = null;
            MinModificationDate = new DateTime(1955, 1, 1);
            MinFileSize = 0;
            RecursiveScan = true;
            FolderToScan = @"H:\docs";
            FileNameSearchPattern = "L4D2";
        }

        public bool OrLink { get; set; }
        public bool? IsHidden { get; set; }
        public bool? IsArch { get; set; }
        public bool? IsReadOnly{ get; set; }
        public bool RecursiveScan { get; set; }

        public virtual DateTime MinModificationDate { get; set; }

        /// <summary>
        /// Минимальный размер файла в кб
        /// </summary>
        public int MinFileSize { get; set; }

        public virtual string FileNameSearchPattern
        {
            get { return _fileNameSearchPattern; }
            set
            {
                _fileNameSearchPattern = value;
            }
        }

        public virtual string FolderToScan
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

        public virtual string FileContentSearchPattern { get; set; }

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
                return Plugins.Where(x => x.IsActive || x.Plugin.IsCorePlugin).Select(x=>x.Plugin).ToArray();
            }
        }

        public ObservableCollection<PluginDecoratorVM> Plugins
        {
            get
            {
                if (_plugins == null)
                {
                    InitPlugins();
                }
                return _plugins;
            }
            private set { _plugins = value; }
        }

        public IPluginManager PluginManager
        {
            get
            {
                if (_pluginManager == null)
                {
                    _pluginManager = AppContext.PluginManager;
                }
                return _pluginManager;
            }
            set { _pluginManager = value; }
        }

        public void InitPlugins()
        {
            _plugins = new ObservableCollection<PluginDecoratorVM>();
            var corePlugins = PluginManager.AllPlugins.Where(x => x.IsCorePlugin).ToArray();
            foreach (var plugin in corePlugins)
            {
                _plugins.Add(new PluginDecoratorVM() { IsActive = true, Plugin = plugin });                
            }

            foreach (var plugin in PluginManager.ExternalPlugins)
            {
                _plugins.Add(new PluginDecoratorVM()
                {
                    IsActive = false,
                    Plugin = plugin
                });
            }    
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Settings:{0}FileNamePattern='{1}'{0}FileContentPattern='{2}'{0}",
                    Environment.NewLine, FileNameSearchPattern, FileContentSearchPattern);
        }

        public IFileSystem FileSystem
        {
            get
            {
                if (_fileSystem == null)
                {
                    _fileSystem = AppContext.FileSystem;
                }
                return _fileSystem;
            }
            set { _fileSystem = value; }
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
                                             !FileSystem.DirectoryExists(this._folderToScan);
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
