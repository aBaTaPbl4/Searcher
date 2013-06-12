using System.Collections.Generic;
using System.IO;
using Common;
using Common.Interfaces;
using System.Linq;

namespace SearcherTests
{
    public static class TestHelper
    {
        public const int FilesInFirstTestDir = 12;
        public const int DirsInFirstTestFolder = 5;

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
            get { return Path.GetFullPath("1"); }
        }

        public static bool ContainsSimilarElement(this ICollection<string> collection, string element)
        {
            foreach (string el in collection)
            {
                if (el.ContainsIgnoreCase(element))
                {
                    return true;
                }
            }
            return false;
        }

        public static void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest, true);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }

        private static IScanPlugin[] GetCoreAndOneExternalPlugin(string externalPluginFileType)
        {
            var lst = new List<IScanPlugin>();
            lst.AddRange(AppContext.PluginManager.CorePlugins);
            lst.Add(AppContext.PluginManager.ExternalPlugins.
                Where(x => x.AssociatedFileExtensions.Contains(externalPluginFileType)).Single());
            return lst.ToArray();            
        }

        public static IScanPlugin[] GetCoreAndXmlPlugin()
        {
            return GetCoreAndOneExternalPlugin(".xml");
        }

        public static IScanPlugin[] GetCoreAndTypePlugin()
        {
            return GetCoreAndOneExternalPlugin(".dll");
        }

        public static IScanPlugin[] GetCoreAndTextPlugin()
        {
            return GetCoreAndOneExternalPlugin(".txt");
        }
    }
}