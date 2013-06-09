using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;

namespace Searcher.VM
{

    public class OptionsPanelVM : IProgramSettings
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

        public WorkType WorkType
        {
            get
            {
                if (!IsNeedAsyncProcessing)
                {
                    return WorkType.SingleThread;
                }
                if (ThreadsNumber > 0)
                {
                    return WorkType.LimitedThreadsCount;
                }
                return WorkType.UnlimitedThreadsCount;
            }
        }
    }
}
