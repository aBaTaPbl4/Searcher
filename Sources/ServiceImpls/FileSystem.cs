using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Common;
using Common.Interfaces;

namespace ServiceImpls
{
    /// <summary>
    /// Изоляция приложения от жесткого диска
    /// </summary>
    [Serializable]
    public class FileSystem : IFileSystem
    {

        public FileSystem()
        {
            AppDomain.CurrentDomain.AssemblyResolve += DependenciesResolver;
        }
        public string FixFolderName(string folderName)
        {
            if (folderName.IsNullOrEmpty())
            {
                return null;
            }            
            if (!Path.IsPathRooted(folderName))
            {
                folderName = Path.GetFullPath(folderName);
            }
            return folderName;
        }
        /// <summary>
        /// Получает список файлов лежащих в каталоге(не рекурсивно)
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public List<string> GetFiles(string folderName)
        {
            var files = new List<string>();

            try
            {
                folderName = FixFolderName(folderName);
                if (folderName.IsNullOrEmpty())
                {
                    AppContext.Logger.ErrorFormat("GetFiles:Incorrect folder name:'{0}'",folderName);
                    return files;
                }
                return new List<string>(Directory.GetFiles(folderName));
            }
            catch (Exception ex)
            {
                AppContext.Logger.ErrorFormat(
                    "GetFiles:During getting files from folder '{0}' error occured:{1}{2}",
                    folderName, Environment.NewLine, ex);
                return files;
            }
        }

        /// <summary>
        /// Рекурсивно получаем список всех подкаталогов
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public List<string> GetAllSubfolders(string folderName)
        {
            var folders = new List<string>();
            
            try
            {
                folderName = FixFolderName(folderName);
                if (folderName.IsNullOrEmpty())
                {
                    return folders;
                }
                InitSubfolders(folderName, folders);
                return folders;
            }
            catch (Exception ex)
            {
                AppContext.Logger.ErrorFormat(
                    "GetAllSubfolders:During getting subfolders of folder '{0}' error occured:{1}{2}",
                    folderName, Environment.NewLine, ex);
                return folders;
            }

        }

        public int GetFilesCountToScan()
        {
            string folderName = AppContext.SearchSettings.FolderToScan;
            if (AppContext.SearchSettings.RecursiveScan)
            {
                return Directory.GetFiles(folderName, "*.*", SearchOption.AllDirectories).Length;
            }
            else
            {
                return Directory.GetFiles(folderName, "*.*", SearchOption.TopDirectoryOnly).Length;
            }
        }

        public XDocument LoadXmlFile(string fileName)
        {
            var doc = XDocument.Load(fileName);
            return doc;
        }

        public AssemblyName GetAssemblyInfo(string fileName)
        {
            AssemblyName asmInfo = null;
            try
            {
                asmInfo = AssemblyName.GetAssemblyName(fileName);
                return asmInfo;
            }
            catch (System.IO.FileNotFoundException)
            {}
            catch (System.BadImageFormatException)
            {}
            catch (System.IO.FileLoadException)
            {}
            return asmInfo;
        }

        public Assembly LoadAssemblyToDomain(AppDomain domain, AssemblyName asmInfo)
        {
             return domain.Load(asmInfo);            
        }

        public Assembly DependenciesResolver(object sender, ResolveEventArgs args)
        {
            //allow to resolve conflicts here
            return null;
        }

        private void InitSubfolders(string folderName, List<string> subfolders)
        {
            foreach (string dir in Directory.GetDirectories(folderName))
            {
                subfolders.Add(dir);
                InitSubfolders(dir, subfolders);
            }
        }

        /// <summary>
        /// Получаем содержимое файла
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileContent(string fileName)
        {
            throw new NotImplementedException("");
        }

        public bool FileExtists(string fileName)
        {
            return File.Exists(fileName);
        }

        public void FileDelete(string fileName)
        {
            File.Delete(fileName);
        }

        public bool DirectoryExists(string dir)
        {
            return Directory.Exists(dir);
        }

    }
}
