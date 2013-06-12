using System.Threading;
using Common.Interfaces;

namespace Searcher.VM
{
    public class OptionsPanelVM : IProgramSettings
    {
        private readonly ThreadInfo _defaultThreadInfo;
        private int _threadsNumber;
        private bool _isNeedAsyncProcessing;

        public OptionsPanelVM()
        {
            IsNeedAsyncProcessing = false;
            ThreadsNumber = 0;
            EnableLogging = false;
            VerboseLogging = false;
            _defaultThreadInfo = CurrentThreadInfo;
        }

        public bool IsNeedAsyncProcessing
        {
            get { return _isNeedAsyncProcessing; }
            set
            {
                _isNeedAsyncProcessing = value;
                InitThreadPool();
            }
        }

        #region IProgramSettings Members

        
        public int ThreadsNumber
        {
            get { return _threadsNumber; }
            set
            {
                _threadsNumber = value;
                InitThreadPool();
            }
        }

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

        private ThreadInfo CurrentThreadInfo
        {
            get
            {
                var info = new ThreadInfo();
                ThreadPool.GetMinThreads(out info.MinWorkerThreads, out info.MinCompletionThreads);
                ThreadPool.GetMaxThreads(out info.MaxWorkerThreads, out info.MaxCompletionThreads);
                return info;
            }
            set
            {
                ThreadPool.SetMinThreads(value.MinWorkerThreads, value.MinCompletionThreads);
                ThreadPool.SetMaxThreads(value.MaxWorkerThreads, value.MaxCompletionThreads);
            }
        }

        private ThreadInfo RequiredThreadInfo
        {
            get
            {
                if (WorkType == WorkType.LimitedThreadsCount)
                {
                    var info = new ThreadInfo();
                    info.MinCompletionThreads =
                        info.MinWorkerThreads = 
                        info.MaxCompletionThreads = 
                        info.MaxWorkerThreads = ThreadsNumber;
                    return info;
                }
                return _defaultThreadInfo;

            }
        }

        private int _i = 0;
        private void InitThreadPool()
        {
            //var info = RequiredThreadInfo;
            //CurrentThreadInfo = RequiredThreadInfo;
        }
        #endregion
    }
}