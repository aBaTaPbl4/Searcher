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

        public void Init(ScanData data)
        {
            Data = data;
        }
    }
}