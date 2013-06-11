using Common.Interfaces;

namespace Searcher.VM
{
    /// <summary>
    /// Класс для отображения в UI
    /// </summary>
    public class PluginDecoratorVM : ViewModel
    {
        private bool _isActive;
        public IScanPlugin Plugin { get; set; }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        public string Name
        {
            get { return Plugin.Name; }
        }
    }
}