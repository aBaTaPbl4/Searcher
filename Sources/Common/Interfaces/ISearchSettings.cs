using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface ISearchSettings
    {
        string FileNameSearchPattern { get; }
        string FileContentSearchPattern { get; }
        List<ISearchPlugin> ActivePlugins { get;}
        bool IsMultithreadRequired { get; }
    }
}