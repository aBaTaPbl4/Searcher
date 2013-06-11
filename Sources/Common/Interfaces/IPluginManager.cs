namespace Common.Interfaces
{
    /// <summary>
    /// ������������ ����� � ���������
    /// ������� ������ �������
    /// ������� ��������� �������
    /// ������ � ���� ����� ���� ��������
    /// </summary>
    public interface IPluginManager
    {
        ISearchPlugin[] AllPlugins { get; }

        /// <summary>
        ///����������� �������. ���������� ���������� �� ������ ������������ �� UI
        /// </summary>
        ISearchPlugin[] ExternalPlugins { get; }
        
        /// <summary>
        /// ������� ����, ������������ ��� ������ ������
        /// </summary>
        ISearchPlugin[] CorePlugins { get; }

        void ScanPluginsFolder();
    }
}