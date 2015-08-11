using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Engine.Model;

namespace Testility.Engine.Abstract
{
    public interface ICompiler
    {
        Result Compile(Input input);
    }
}
