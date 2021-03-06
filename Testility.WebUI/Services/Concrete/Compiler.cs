﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.Engine.Abstract;
using Testility.Engine.Model;

namespace Testility.WebUI.Services.Concrete
{
    public sealed class Compiler
    {
        private readonly ICompiler compilerRepository;
        private IDbRepository setupRepository;
        private Compiler(ICompiler compilerRepository, IDbRepository setupRepository)
        {
            this.compilerRepository = compilerRepository;
            this.setupRepository = setupRepository;
        }
        public static Compiler GetInstance(ICompiler compilerRepository, IDbRepository setupRepository)
        {
            return new Compiler(compilerRepository, setupRepository);
        }

        public Result Compile<T>(T solution, int[] referencesIds)
        {
            Input input = Mapper.Map<Input>(solution);

            input.ReferencedAssemblies = setupRepository.GetSelectedReferencesNames(referencesIds);
            return compilerRepository.Compile(input);
        }

        public Result RunTests<T>(T solution, int[] referencesIds)
        {
            Input input = Mapper.Map<Input>(solution);

            input.ReferencedAssemblies = setupRepository.GetSelectedReferencesNames(referencesIds);
            return compilerRepository.RunTests(input);
        }
    }
}