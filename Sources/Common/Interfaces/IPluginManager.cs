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
        IScanPlugin[] AllPlugins { get; }

        /// <summary>
        ///����������� �������. ���������� ���������� �� ������ ������������ �� UI
        /// </summary>
        IScanPlugin[] ExternalPlugins { get; }
        
        /// <summary>
        /// ������� ����, ������������ ��� ������ ������
        /// </summary>
        IScanPlugin[] CorePlugins { get; }

        string PluginFolder { get; }

        void ScanPluginsFolder();
    }
}