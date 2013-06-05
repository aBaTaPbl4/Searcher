using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Searcher.VM
{
    public class OptionsPanelVM
    {
        public OptionsPanelVM()
        {
            IsNeedAsyncProcessing = false;
            ThreadsNumber = 0;
            EnableLogging = false;
            VerboseLogging = false;
        }
        public bool IsNeedAsyncProcessing { get; set; }
        public int ThreadsNumber { get; set; }
        public bool EnableLogging { get; set; }
        public bool VerboseLogging { get; set; }
    }
}
