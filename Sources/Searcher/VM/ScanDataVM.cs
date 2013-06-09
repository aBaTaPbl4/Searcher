using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Models;

namespace Searcher.VM
{
    /// <summary>
    /// Обертка над ScanData. Добавляет свойство Check и уведомления интерфейса.
    /// </summary>
    public class ScanDataVM : ViewModel, INotifyPropertyChanged
    {
        private bool _check;
        private ScanData _data;

        public ScanDataVM()
        {
            _data = new ScanData();
            _check = false;
        }

        public bool Check
        {
            get { return _check; }
            set
            {
                _check = value;
                OnPropertyChanged("Check");
            }
        }

        public string FileName
        {
            get { return Data.FileName; }
        }

        public string FolderName
        {
            get { return Data.FolderName; }
        }

        public string FullName
        {
            get { return Data.FullName; }
        }

        public ScanData Data
        {
            get { return _data; }
            private set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        public void Init(ScanData data)
        {
            Data = data;
        }

        public string FoundByPlugin
        {
            get { return Data.FoundByPlugin; }
        }
    }
}
