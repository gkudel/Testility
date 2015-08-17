using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;
using Testility.Engine.Abstract;
using Testility.Engine.Model;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Services.Concrete
{
    public class CompilerService : ICompilerService
    {
        private ICompiler compilerRepository;
        public CompilerService(ICompiler compilerRepository)
        {
            this.compilerRepository = compilerRepository;
        }

        public void compile(Solution solution)
        {
            Input input = Mapper.Map<Input>(solution);                   
        }
    }
}
