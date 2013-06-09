using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Interfaces
{
    public enum WorkType
    {
        SingleThread,
        UnlimitedThreadsCount,
        LimitedThreadsCount
    }

    public interface IProgramSettings
    {
        int ThreadsNumber { get; }
        bool EnableLogging { get; }
        bool VerboseLogging { get; }
        WorkType WorkType { get; }
    }
}
