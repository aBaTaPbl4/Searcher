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

    public interface IFileSystem
    {
        string FixFolderName(string folderName);

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

        XDocument LoadXmlFile(string fileName);
        AssemblyName GetAssemblyInfo(string fileName);
        Assembly LoadAssemblyToDomain(AppDomain domain, AssemblyName asmInfo);
        bool FileExtists(string fileName);
        bool DirectoryExists(string dir);
        void FileDelete(string fileName);
        int GetFilesCountToScan();
        FileInfoShort GetFileInfo(string fileName);
    }
}