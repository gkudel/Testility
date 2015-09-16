using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Engine.Model;

namespace Testility.Engine.Abstract
{
    [ContractClass(typeof(CompilerContract))]

    public interface ICompiler
    {
        Result Compile(Input input);
        Result RunTests(Input input);
    }
}
