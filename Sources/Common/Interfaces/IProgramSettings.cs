namespace Common.Interfaces
{
    public enum WorkType
    {
        SingleThread,
        UnlimitedThreadsCount,
        LimitedThreadsCount
    }

    public struct ThreadInfo
    {
        public int MinWorkerThreads;
        public int MinCompletionThreads;
        public int MaxWorkerThreads;
        public int MaxCompletionThreads;
    }

    public interface IProgramSettings
    {
        int ThreadsNumber { get; }
        bool EnableLogging { get; }
        bool VerboseLogging { get; }
        WorkType WorkType { get; }
    }
}