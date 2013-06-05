using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;

namespace Searcher.VM
{
    /// <summary>
    /// Класс для отображения в UI
    /// </summary>
    public class PluginDecoratorVM
    {
        public ISearchPlugin Plugin { get; set; }
        public bool IsActive { get; set; }
        public string Name
        {
            get { return Plugin.Name; }
        }
        
    }
}
