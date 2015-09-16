using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.Engine.Abstract;
using Testility.Engine.Model;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Services.Concrete
{
    public class UnitTestCompilerService : ICompilerService
    {
        private readonly ICompiler compilerRepository;
        private IDbRepository setupRepository;
        public UnitTestCompilerService(ICompiler compilerRepository, IDbRepository setupRepository)
        {
            this.compilerRepository = compilerRepository;
            this.setupRepository = setupRepository;
        }

        public IList<Error> Compile(Solution solution, int[] referencesIds)
        {
            Result r = Compiler.GetInstance(compilerRepository, setupRepository).Compile<UnitTestSolution>(solution as UnitTestSolution, referencesIds);
            return r.Errors;
        }

        public IList<Error> RunTests(Solution solution, int[] referencesIds)
        {
            Result r = Compiler.GetInstance(compilerRepository, setupRepository).RunTests<UnitTestSolution>(solution as UnitTestSolution, referencesIds);
            return r.Errors;
        }
    }
}
