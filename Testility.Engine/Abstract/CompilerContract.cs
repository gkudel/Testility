using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Engine.Model;

namespace Testility.Engine.Abstract
{
    [ContractClassFor(typeof(ICompiler))]
    abstract public class CompilerContract : ICompiler
    {        
        public Result Compile(Input input)
        {
            Contract.Requires<ArgumentNullException>(input.Code != null && input.Code.Length > 0);
            return null;
        }

        public void LoadAssembly(string assemblyPath)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(assemblyPath));
        }
    }
}
