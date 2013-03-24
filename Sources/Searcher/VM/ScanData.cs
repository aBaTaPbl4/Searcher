using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Searcher.VM
{
    public class ScanData : INotifyPropertyChanged
    {
        private string _fileName;
        private bool _check;

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                OnPropertyChanged("FileName");
                _fileName = value;
            }
        }

        public bool Check
        {
            get { return _check; }
            set
            {
                OnPropertyChanged("Check");
                _check = value;
            }
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
