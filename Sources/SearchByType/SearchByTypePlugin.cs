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
        public SearchType Type
        {
            get { return SearchType.ByFileContent; }
        }


        public bool Check(string fileName, ISearchSettings settings)
        {
            try
            {
                AssemblyName asmInfo = null;
                if (!AssociatedFileExtensions.Contains(Path.GetExtension(fileName)) 
                    || settings.FileContentSearchPattern.IsNullOrEmpty()
                    || !IsFileAssembly(fileName, out asmInfo))
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

        private bool AssemblyContainsType(AssemblyName asmInfo, string typeName)
        {
            AppDomain domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null);
            try
            {
                var asm = domain.Load(asmInfo);
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


        private bool IsFileAssembly(string fileName, out AssemblyName asmInfo)
        {
            asmInfo = null;
            try
            {
                asmInfo = AssemblyName.GetAssemblyName(fileName);
                return true;
            }

            catch (System.IO.FileNotFoundException)
            {

                return false;
            }

            catch (System.BadImageFormatException)
            {
                return false;
            }

            catch (System.IO.FileLoadException)
            {
                return false;
            }
        }

        public string Name
        {
            get { return "SearchByTypePlugin"; }
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
