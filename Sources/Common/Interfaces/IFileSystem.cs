using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Common.Interfaces
{
    /// <summary>
    /// ���� ��������� � �������� dto, ����� ����, 
    /// ��� ����������� FileInfo ����������� ������� �����
    /// </summary>
    public struct FileInfoShort
    {
        public bool IsHidden;
        public bool IsArch;
        public bool IsReadOnly;
        public DateTime ModificationDate;
        public long FileSize;
    }

    /// <summary>
    /// �������� ���������� �� �������� �����
    /// ������ ������ ��� �������� ����������� � ��������� ��������� ���� ������
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// �������� ���-�� ������ � ����������� �����, � ������ �������� ������������� ��������
        /// </summary>
        /// <returns></returns>
        int GetFilesCountToScan();

        /// <summary>
        /// �������� ������ ������ ������� � ��������(�� ����������)
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        List<string> GetFiles(string folderName);

        /// <summary>
        /// ���������� �������� ������ ���� ������������
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        List<string> GetAllSubfolders(string folderName);

        /// <summary>
        /// ��������� �������� �� ���������� ���� .net �������, �� �������� ��� ����.
        /// (� �������� ����� ������������� ������ ��������� �����).
        /// ����� ������� ��������� �������� �������� ���������� �� ������
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        bool IsAssembly(string fileName);

        string FixFolderName(string folderName);
        bool FileExtists(string fileName);
        bool DirectoryExists(string dir);
        void FileDelete(string fileName);
        FileInfoShort GetFileInfo(string fileName);
        Stream GetFileStream(string fileName);
        byte[] ReadFile(string fileName);
        IScanSettings ScanSettings { get; }
    }
}