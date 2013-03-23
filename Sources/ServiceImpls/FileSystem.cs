using System;
using System.Collections.Generic;
using System.IO;
using Common;

namespace ServiceImpls
{
    public class FileSystem
    {

        private string FixFolderName(string folderName)
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

    }
}
