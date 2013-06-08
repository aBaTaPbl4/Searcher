using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Models;

namespace Searcher.VM
{
    public class ScanDataVM : ScanData, INotifyPropertyChanged
    {
        private bool _check;

        public bool Check
        {
            get { return _check; }
            set
            {
                OnPropertyChanged("Check");
                _check = value;
            }
        }

        public void Init(ScanData data)
        {
            this.FileName = data.FileName;
            this.FolderName = data.FolderName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
