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
    public class CompilerService : ICompilerService
    {
        private readonly ICompiler compilerRepository;
        private ISetupRepository setupRepository;
        public CompilerService(ICompiler compilerRepository, ISetupRepository setupRepository)
        {
            this.compilerRepository = compilerRepository;
            this.setupRepository = setupRepository;
        }

        public IList<Error> Compile(Solution solution, int[] referencesIds)
        {
            Input input = Mapper.Map<Input>(solution);
            input.ReferencedAssemblies = setupRepository.GetSelectedReferencesNames(referencesIds);
            Result r = compilerRepository.Compile(input);
            if (r.Errors.Count == 0)
            {
                Mapper.Map<Result, Solution>(r, solution);
            }
            return r.Errors;
        }
    }
}
