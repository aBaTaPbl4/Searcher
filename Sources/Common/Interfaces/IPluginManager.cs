namespace Common.Interfaces
{
    public interface IPluginManager
    {
        ISearchPlugin[] AllPlugins { get; }
        ISearchPlugin MainPlugin { get; }
    }
}