using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Common;
using Common.Interfaces;

namespace SearchByType
{

    public class SearchByTypePlugin : ISearchPlugin
    {
        public const string PluginName = "Search by type in .net assemblies";
        private readonly string _currentAssemblyPath;

        public SearchByTypePlugin()
        {
             _currentAssemblyPath = new Uri(this.GetType().Assembly.CodeBase).LocalPath;
        }

        #region ISearchPlugin Members

        public SearchType Type
        {
            get { return SearchType.ByFileContent; }
        }


        public bool IsCorePlugin
        {
            get { return false; }
        }

        public bool Check(string fileName, ISearchSettings settings)
        {
            try
            {
                bool checkRequired = !settings.FileContentSearchPattern.IsNullOrEmpty();

                if ( !checkRequired )
                {
                    return true;
                }

                bool fileHasDiferentExtension = !AssociatedFileExtensions.Contains(Path.GetExtension(fileName));

                if (fileHasDiferentExtension)
                {
                    return false;
                }

                if (!AppContext.FileSystem.IsAssembly(fileName))
                {
                    return false;
                }

                return AssemblyContainsType(fileName, settings.FileContentSearchPattern);
            }
            catch (Exception ex)
            {
                AppContext.Logger.ErrorFormat("{3}.Check: FileName: '{0}', Settings: '{1}'. Exception occured: {2}",
                                              fileName, settings, ex, Name);
                return false;
            }
        }

        public string Name
        {
            get { return PluginName; }
        }

        public List<string> AssociatedFileExtensions
        {
            get { return new List<string> {".dll", ".exe"}; }
        }

        public bool IsForAnyFileExtension
        {
            get { return false; }
        }

        #endregion

        public bool AssemblyContainsType(string fileName, string typeName)
        {

            var domain = CreateTempDomain(Path.GetDirectoryName(fileName));
            var resolver = (AssemblyResolver) domain.CreateInstanceFromAndUnwrap(_currentAssemblyPath, 
                "SearchByType.AssemblyResolver");

            try
            {
                Assembly asm = domain.LoadAssembly(fileName);
                
                if (resolver.ResolvedProblemOccured && resolver.ResolvedAssembly == null)
                {
                    return false;
                }
                IEnumerable<Type> q = from t in asm.GetTypes()
                                      where t.Name.ContainsIgnoreCase(typeName) && (t.IsClass || t.IsInterface)
                                      select t;
                return q.Any();
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }

        public AppDomain CreateTempDomain(string baseDirectory)
        {
            var info = new AppDomainSetup();
            info.ApplicationBase = baseDirectory;
            AppDomain domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), AppDomain.CurrentDomain.Evidence, info);
            return domain;
        }

    }
}