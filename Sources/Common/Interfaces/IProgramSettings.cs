namespace Common.Interfaces
{
    public enum WorkType
    {
        SingleThread,
        UnlimitedThreadsCount,
        LimitedThreadsCount
    }

    public class ThreadInfo
    {
        public int MinWorkerThreads;
        public int MinCompletionThreads;
        public int MaxWorkerThreads;
        public int MaxCompletionThreads;
    }

    /// <summary>
    /// Настройки уровня программы
    /// Данные открыты только на чтение
    /// </summary>
    public interface IProgramSettings
    {
        int ThreadsNumber { get; }
        bool EnableLogging { get; }
        bool VerboseLogging { get; }
        WorkType WorkType { get; }
    }
}