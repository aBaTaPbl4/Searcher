using System.IO;

namespace Models
{
    public class ScanData
    {
        private string _fileName;
        private string _folderName;

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
    }
}