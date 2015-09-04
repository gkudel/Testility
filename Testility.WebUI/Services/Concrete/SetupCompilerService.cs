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
    public class SetupCompilerService : ICompilerService
    {
        private readonly ICompiler compilerRepository;
        private IDbRepository setupRepository;
        public SetupCompilerService(ICompiler compilerRepository, IDbRepository setupRepository)
        {
            this.compilerRepository = compilerRepository;
            this.setupRepository = setupRepository;
        }

        public IList<Error> Compile(Solution solution, int[] referencesIds)
        {
            Result r = Compiler.GetInstance(compilerRepository, setupRepository).Compile<SetupSolution>(solution as SetupSolution, referencesIds);
            if (r.Errors.Count == 0)
            {
                Mapper.Map<Result, Solution>(r, solution);
            }
            return r.Errors;
        }
    }
}
