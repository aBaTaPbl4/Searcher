using System;

namespace Common.Interfaces
{
    /// <summary>
    /// ��������� ������
    /// (��������� ������ ������ ������ � ���������� ��������� �������������)
    /// </summary>
    public interface ISearchSettings
    {
        string FileNameSearchPattern { get; }
        string FileContentSearchPattern { get; }
        ISearchPlugin[] ActivePlugins { get; }
        string FolderToScan { get; }
        bool RecursiveScan { get; }
        bool? IsHidden { get; }
        bool? IsArch { get; }
        bool? IsReadOnly { get; }
        DateTime? MinModificationDate { get; }

        /// <summary>
        /// ����������� ������ ����� � ��
        /// </summary>
        long MinFileSize { get; }
    }
}