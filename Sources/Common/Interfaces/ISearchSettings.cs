using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface ISearchSettings
    {
        string FileNameSearchPattern { get; }
        string FileContentSearchPattern { get; }
        ISearchPlugin[] ActivePlugins { get; }
        bool IsMultithreadRequired { get; }
        string FolderToScan { get; }
    }
}