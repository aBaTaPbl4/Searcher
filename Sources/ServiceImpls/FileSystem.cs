using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Common;
using Common.Interfaces;

namespace ServiceImpls
{
    public class FileSystem : IFileSystem
    {

        public ISearchSettings SearchSettings { get; set; }

        #region IFileSystem Members

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

        public List<string> GetFiles(string folderName)
        {
            var files = new List<string>();

            try
            {
                folderName = FixFolderName(folderName);
                if (folderName.IsNullOrEmpty())
                {
                    AppContext.Logger.ErrorFormat("GetFiles:Incorrect folder name:'{0}'", folderName);
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


        public List<string> GetAllSubfolders(string folderName)
        {
            var emptyList = new List<string>();
            try
            {
                folderName = FixFolderName(folderName);
                if (folderName.IsNullOrEmpty())
                {
                    return emptyList;
                }
                var walker = new DirectoryWalker(folderName);
                walker.Run();
                return walker.Results;
            }
            catch (Exception ex)
            {
                AppContext.Logger.ErrorFormat(
                    "GetAllSubfolders:During getting subfolders of folder '{0}' error occured:{1}{2}",
                    folderName, Environment.NewLine, ex);
                return emptyList;
            }
        }

        public int GetFilesCountToScan()
        {
            string folderName = SearchSettings.FolderToScan;
            if (SearchSettings.RecursiveScan)
            {
                return Directory.GetFiles(folderName, "*.*", SearchOption.AllDirectories).Length;
            }
            else
            {
                return Directory.GetFiles(folderName, "*.*", SearchOption.TopDirectoryOnly).Length;
            }
        }

        public FileInfoShort GetFileInfo(string fileName)
        {
            var info = new FileInfo(fileName);
            var result = new FileInfoShort();
            result.IsReadOnly = info.IsReadOnly;
            result.ModificationDate = info.LastWriteTime;
            result.IsArch = (FileAttributes.Archive & info.Attributes) == FileAttributes.Archive;
            result.IsHidden = (FileAttributes.Hidden & info.Attributes) == FileAttributes.Hidden;
            result.FileSize = info.Length/1024;
            return result;
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

        #endregion

        public Stream GetFileStream(string fileName)
        {
            var fs = File.OpenRead(fileName);
            return fs;
        }

        public bool IsAssembly(string fileName)
        {
            AssemblyName asmInfo = null;
            try
            {
                asmInfo = AssemblyName.GetAssemblyName(fileName);
                return asmInfo != null;
            }
            catch (FileNotFoundException)
            {
            }
            catch (BadImageFormatException)
            {
            }
            catch (FileLoadException)
            {
            }
            return false;
        }

        public byte[] ReadFile(string fileName)
        {
            byte[] buffer;
            using (var fs = GetFileStream(fileName))
            {
                buffer = new byte[(int)fs.Length];
                fs.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
    }
}