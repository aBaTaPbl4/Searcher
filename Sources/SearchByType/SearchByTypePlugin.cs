using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Common;
using Common.Interfaces;

namespace SearchByType
{
    public class SearchByTypePlugin : ISearchPlugin
    {

        public const string PluginName = "SearchByTypePlugin";

        public SearchType Type
        {
            get { return SearchType.ByFileContent; }
        }


        public bool Check(string fileName, ISearchSettings settings)
        {
            try
            {
                AssemblyName asmInfo = null;
                if (settings.FileContentSearchPattern.IsNullOrEmpty() ||
                    !AssociatedFileExtensions.Contains(Path.GetExtension(fileName)) ||
                    settings.FileContentSearchPattern.IsNullOrEmpty() ||
                    !IsAssembly(fileName, out asmInfo))
                {
                    return false;
                }
                
                return AssemblyContainsType(asmInfo, settings.FileContentSearchPattern);
            }
            catch (Exception ex)
            {                
                AppContext.Logger.ErrorFormat("{3}.Check: FileName: '{0}', Settings: '{1}'. Exception occured: {2}",
                    fileName, settings, ex, Name);
                return false;
            }            
        }

        public bool AssemblyContainsType(AssemblyName asmInfo, string typeName)
        {
            AppDomain domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null);
            try
            {
                var asm = AppContext.FileSystem.LoadAssemblyToDomain(domain, asmInfo);
                var q = from t in asm.GetTypes()
                        where t.Name.ContainsIgnoreCase(typeName) && (t.IsClass || t.IsInterface)
                        select t;
                return q.Any();
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }


        public bool IsAssembly(string fileName, out AssemblyName asmInfo)
        {
            asmInfo = AppContext.FileSystem.GetAssemblyInfo(fileName);
            return asmInfo != null;
        }

        public string Name
        {
            get { return PluginName; }
        }

        public List<string> AssociatedFileExtensions
        {
            get 
            { 
                return new List<string>() {".dll", ".exe"} ;
            }
        }

        public bool IsForAnyFileExtension
        {
            get
            {
                return false;
            }
        }
    }
}
