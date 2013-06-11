using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using AssemblyResolver;
using Common;
using Common.Interfaces;

namespace SearchByType
{
    /// <summary>
    /// Плагин поиска по типу
    /// </summary>
    [Serializable]
    public class ScanByTypePlugin : IScanPlugin
    {
        public const string PluginName = "Search by type in .net assemblies";
        private readonly string _currentAssemblyPath;

        public ScanByTypePlugin()
        {
             _currentAssemblyPath = new Uri(this.GetType().Assembly.CodeBase).LocalPath;
        }

        #region IScanPlugin Members

        public SearchType Type
        {
            get { return SearchType.ByFileContent; }
        }


        public bool IsCorePlugin
        {
            get { return false; }
        }

        public bool Check(string fileName, IScanSettings settings)
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

                return AssemblyContainsInterfaceImplemetation(fileName, settings.FileContentSearchPattern);
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



        public bool AssemblyContainsInterfaceImplemetation(string fileName, string interfaceName)
        {
            var domain = CreateTempDomain(Path.GetDirectoryName(fileName));

            var resolver = (AssemblyResolver.AssemblyResolver)domain.CreateInstanceFromAndUnwrap(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AssemblyResolver.dll"),
                "AssemblyResolver.AssemblyResolver");

            try
            {
                resolver.LoadAssembly(AppContext.FileSystem.ReadFile(fileName));
                resolver.FileName = Path.GetFileName(fileName);
                var result = resolver.ContainsInterfaceImplementation(interfaceName);
                if (resolver.ErrorMessage != null)
                {
                    AppContext.Logger.ErrorFormat(resolver.ErrorMessage);
                }
                return result;
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }

        public static void FileExists()
        {
            AppContext.FileSystem.FileExtists("aaaa");
        }

        public AppDomain CreateTempDomain(string baseDirectory)
        {
            var info = new AppDomainSetup();
            string name = Guid.NewGuid().ToString();
            info.ApplicationName = name;
            info.ApplicationBase = baseDirectory;
            info.PrivateBinPath = baseDirectory;
            info.PrivateBinPathProbe = baseDirectory;
            info.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            var evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
            AppDomain domain = AppDomain.CreateDomain(name, evidence, info);
            return domain;
        }

    }
}