﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Common;
using Common.Interfaces;

namespace Searcher.VM
{
    public class ScanSettingsPanelVM : IScanSettings, IDataErrorInfo
    {
        private ObservableCollection<PluginDecoratorVM> _pluginListsForUser;
        private string _fileNameSearchPattern;
        private IFileSystem _fileSystem;
        private string _folderToScan;
        private IPluginManager _pluginManager;

        public ScanSettingsPanelVM()
        {
            IsHidden = null;
            IsArch = null;
            IsReadOnly = null;
            MinModificationDate = null;
            MinFileSize = 0;
            RecursiveScan = true;
            _pluginListsForUser = new ObservableCollection<PluginDecoratorVM>();
            //FolderToScan = @"H:\docs";
            //FileNameSearchPattern = "Story";
        }


        public ObservableCollection<PluginDecoratorVM> PluginListsForUser
        {
            get
            {
                if (_pluginListsForUser.Count == 0)
                {
                    InitPlugins();
                }
                return _pluginListsForUser;
            }
            private set { _pluginListsForUser = value; }
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

        #region IDataErrorInfo Members

        public string this[string name]
        {
            get
            {
                string result = null;
                if (name == "FileNameSearchPattern")
                {
                    foreach (char wrongChar in Path.GetInvalidPathChars())
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

                    bool folderIsNotExists = !_folderToScan.IsNullOrEmpty() &&
                                             !FileSystem.DirectoryExists(_folderToScan);
                    if (folderIsNotExists)
                    {
                        result = String.Format("Directory '{0}' does not exists!", _folderToScan);
                    }
                }
                return result;
            }
        }

        public string Error
        {
            get { return null; }
        }

        #endregion

        #region IScanSettings Members

        public virtual bool? IsHidden { get; set; }
        public virtual bool? IsArch { get; set; }
        public virtual bool? IsReadOnly { get; set; }
        public virtual bool RecursiveScan { get; set; }

        private DateTime? _minModificationDate;
        public virtual DateTime? MinModificationDate
        {
            get { return _minModificationDate; }
            set
            {
                _minModificationDate = value;
            }
        }

        /// <summary>
        /// Минимальный размер файла в кб
        /// </summary>
        public virtual long MinFileSize { get; set; }

        public virtual string FileNameSearchPattern
        {
            get { return _fileNameSearchPattern; }
            set { _fileNameSearchPattern = value; }
        }

        public virtual string FolderToScan
        {
            get { return _folderToScan; }
            set { _folderToScan = value; }
        }

        public virtual string FileContentSearchPattern { get; set; }

        public virtual IScanPlugin[] ActivePlugins
        {
            get
            {
                return
                    PluginListsForUser.Where(x => x.IsActive).Select(x => x.Plugin).Union(PluginManager.CorePlugins).
                        ToArray();
            }
        }

        #endregion

        public void SetActivePlugin(PluginDecoratorVM activePlugin)
        {
            foreach (PluginDecoratorVM curPlugin in PluginListsForUser)
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

        public void InitPlugins()
        {
            foreach (IScanPlugin loadedPlugin in PluginManager.ExternalPlugins)
            {
                bool isPluginAlredyAdded = _pluginListsForUser.Where(x => x.Name == loadedPlugin.Name).Any();
                if (!isPluginAlredyAdded)
                {
                    _pluginListsForUser.Add(new PluginDecoratorVM
                    {
                        IsActive = false,
                        Plugin = loadedPlugin
                    });                    
                }
            }
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Settings:{0}FileNamePattern='{1}'{0}FileContentPattern='{2}'{0}",
                    Environment.NewLine, FileNameSearchPattern, FileContentSearchPattern);
        }
    }
}