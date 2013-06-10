using System.IO;

namespace Models
{
    public class ScanData
    {
        public string FileName { get; set; }

        public string FolderName { get; set; }

        public string FullName
        {
            get { return Path.Combine(FolderName, FileName); }
        }
    }
}