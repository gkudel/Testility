using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.Engine.Abstract
{
    public interface ICompiler
    {
        IEnumerable<TestedClass> compile(SourceCode sourceCode);
    }
}
