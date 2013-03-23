using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Common.Interfaces
{
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
    }
}