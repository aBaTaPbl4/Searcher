namespace Common.Interfaces
{
    public interface IPluginManager
    {
        ISearchPlugin[] AllPlugins { get; }
        ISearchPlugin[] ExternalPlugins { get; }
        ISearchPlugin[] CorePlugins { get; }
    }
}