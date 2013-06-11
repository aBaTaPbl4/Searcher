using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SearchByType
{
    /// <summary>
    /// Данный тип нужен для работы в рамках временного домена, 
    /// и передачи данных о разрешенных сборках типу SearchByTypePlugin
    /// через границы временного домена
    /// </summary>
    [Serializable]
    public class AssemblyResolver : MarshalByRefObject
    {
        
        public AssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveDependencies);
        }

        public Assembly ResolvedAssembly { get; private set; }
        
        public bool ResolvedProblemOccured { get; private set; }
        
        public Assembly ResolveDependencies(object sender, ResolveEventArgs args)
        {
            
            ResolvedProblemOccured = true;
            //allow to resolve conflicts here
            //try to find assembly

            return null;
        }
    }
}
