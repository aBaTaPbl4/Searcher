using System.ComponentModel;
using Models;

namespace Searcher.VM
{
    /// <summary>
    /// Обертка над ScanData. Добавляет свойство Checked и уведомления интерфейса.
    /// </summary>
    public class ScanDataVM : ViewModel, INotifyPropertyChanged
    {
        private bool _checked;
        private ScanData _data;

        public ScanDataVM()
        {
            _data = new ScanData();
            _checked = false;
        }

        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                OnPropertyChanged("Checked");
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

        public int SizeRating
        {
            get
            {
                var sz = Data.Size;
                if (sz < 1000)
                {
                    return 1;
                }
                else if (sz < 2000)
                {
                    return 2;
                }
                else if (sz < 3000)
                {
                    return 3;
                }
                else if (sz < 4000)
                {
                    return 4;
                }
                else if (sz < 5000)
                {
                    return 5;
                }
                else if (sz < 6000)
                {
                    return 6;
                }
                else if (sz < 7000)
                {
                    return 7;
                }
                else if (sz < 8000)
                {
                    return 8;
                }
                else if (sz < 9000)
                {
                    return 9;
                }
                else 
                {
                    return 10;
                }                
            }
        }

        public void Init(ScanData data)
        {
            Data = data;
        }
    }
}