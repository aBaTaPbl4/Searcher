using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace AssemblyResolver
{
    /// <summary>
    /// Данный тип нужен для работы в рамках временного домена, 
    /// и передачи данных о разрешенных сборках типу SearchByTypePlugin
    /// через границы временного домена приложения
    /// PS:
    /// О том почему этот класс так нагружен == о причудах Jit-компилятора 
    /// можно почитать здесь:
    /// http://www.codemeit.com/code-collection/assemblyresolve-filenotfoundexception-load-assembly-from-resource.html
    /// </summary>
    [Serializable]
    public class AssemblyResolver : MarshalByRefObject
    {
        private string ApplicationBase { get; set; }
        
        public AssemblyResolver()
        {
            ApplicationBase = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveDependencies;            
        }

        public void LoadAssembly(byte[] content)
        {
            Assembly.Load(content);
        }

        public string FileName { get; set; }


        private Assembly LoadedAssembly
        {
            get
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (asm.ManifestModule.ScopeName.Equals(FileName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return asm;
                    }
                }
                return null;                
            }
        }

        public string ErrorMessage { get; private set; }

        public bool ContainsInterfaceImplementation(string interfaceName)
        {
            try
            {
                var asm = LoadedAssembly;
                if (asm == null)
                {
                    return false;
                }

                IEnumerable<Type> interfaceImpls = from t in asm.GetTypes()
                                      from i in t.GetInterfaces()
                                      where i.Name.Equals(interfaceName, StringComparison.InvariantCultureIgnoreCase) 
                                      && t.IsClass
                                      select t;
                return interfaceImpls.Any();
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void InitErrorMessage(string resolveAssemblyPath)
        {
            ErrorMessage = string.Format(@"Failed to resolve dependency '{0}' during loading assembly '{1}\{2}'",
                             resolveAssemblyPath, ApplicationBase, FileName);
        }

        private Assembly ResolveDependencies(object sender, ResolveEventArgs args)
        {
            string assemblyPath = "";
            try
            {
                AssemblyName assemblyName = new AssemblyName(args.Name);
                string fileName = string.Format("{0}.dll", assemblyName.Name);
                assemblyPath = Path.Combine(ApplicationBase, fileName);                
                //todo:изолировать от жесткого диска, нужно дергать callback-ами FileSystem из главного домена
                if (File.Exists(assemblyPath))
                {
                    return Assembly.LoadFile(assemblyPath);    
                }
                else
                {
                    InitErrorMessage(assemblyPath);
                }
                return null;

            }
            catch (Exception)
            {
                InitErrorMessage(assemblyPath);
                return null;
            }   
        }

    }
}
