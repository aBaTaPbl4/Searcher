using System;
using System.Collections.Generic;
using System.IO;
using Common;


namespace SearcherTests
{
    public static class TestHelper
    {
        
        public static bool ContainsSimilarElement(this ICollection<string> collection, string element)
        {
            foreach (var el in collection)
            {
                if (el.ContainsIgnoreCase(element))
                {
                    return true;
                }
            }
            return false;
        }

        public static string XmlFileName
        {
            get { return Path.GetFullPath(@"1\5\note.xml"); }
        }

        public static string AssemblyFileName
        {
            get { return Path.GetFullPath(@"1\2\3\log4net.dll"); }
        }

        public static string NativeFileName
        {
            get { return Path.GetFullPath(@"1\2\3\7z.exe"); }
        }

        public static string DeepestFolder
        {
            get
            {
                return Path.GetFullPath("1");
            }
        }
    }
}
