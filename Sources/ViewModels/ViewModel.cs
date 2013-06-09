using System;
using System.ComponentModel;
using System.Windows;


namespace Searcher.VM
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(String info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        protected void InvokeInUIThread(Action act)
        {
            Application.Current.Dispatcher.Invoke(act);
        }
    }
}
