using System;
using System.IO;

namespace Models
{
    public class ScanData
    {
        public string FileName { get; set; }

        public string FolderName { get; set; }

        public bool IsReadOnly { get; set; }
        public bool IsArch { get; set; }
        public bool IsHidden { get; set; }
        public long Size { get; set; }
        public DateTime ModificationDate { get; set; }

        public string FullName
        {
            get { return Path.Combine(FolderName, FileName); }
        }
    }
}