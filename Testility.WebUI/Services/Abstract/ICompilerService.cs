using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;
using Testility.Engine.Model;

namespace Testility.WebUI.Services.Abstract
{
    public interface ICompilerService
    {
        IList<Error> Compile(Solution solution, int[] referencesIds);
        IList<Error> RunTests(Solution solution, int[] referencesIds);
        
    }
}
