using System.IO;
using Common.Interfaces;

namespace Models
{
    public class ScanData
    {
        private string _fileName;
        private string _folderName;
        private ISearchPlugin _foundBy;

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        public string FolderName
        {
            get
            {
                return _folderName;
            }
            set
            {
                _folderName = value;
            }
        }

        public string FullName
        {
            get { return Path.Combine(FolderName, FileName); }
        }

        public string FoundByPlugin
        {
            get
            {
                if (_foundBy == null)
                {
                    return string.Empty;
                }
                return _foundBy.Name;
            }
        }
    }
}