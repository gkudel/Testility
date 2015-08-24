﻿using AutoMapper;
using System.Collections.Generic;
using Testility.Domain.Entities;
using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Testility.Engine.Model;
using Testility.WebUI.Model;
using Testility.Domain.Infrastructure.Mapping;
using Testility.WebUI.Areas.Setup.Model;

namespace Testility.WebUI.Infrastructure.Mapping
{
    public class WebUIModelMapping : Profile
    {
        public override string ProfileName
        {
            get { return "WebUIModelMapping"; }
        }
        protected override void Configure()
        {
            #region Setup
            Mapper.CreateMap<Solution, SolutionApi>()
                .ForMember(s => s.References, opt => opt.MapFrom(s => s.References.Select(solution => solution.Id).ToArray()));
            Mapper.CreateMap<Item, ItemApi>();

            Mapper.CreateMap<Solution, SolutionIndexItemViewModel>()
                .ForMember(i => i.Classes, opt => opt.ResolveUsing<ClassesCountResolver>().ConstructedBy(() => new ClassesCountResolver()))
                .ForMember(i => i.Methods, opt => opt.ResolveUsing<MethodsCountResolver>().ConstructedBy(() => new MethodsCountResolver()))
                .ForMember(i => i.Tests, opt => opt.ResolveUsing<TestsCountResolver>().ConstructedBy(() => new TestsCountResolver()));

            Mapper.CreateMap<Solution, SolutionViewModel>()
                .ForMember(s => s.References, opt => opt.Ignore());
            Mapper.CreateMap<SolutionViewModel, Solution>()
                .ForMember(s => s.CompiledDll, opt => opt.Ignore());                

            Mapper.CreateMap<ICollection<ItemViewModel>, ICollection<Item>>()
                .ConvertUsing(new CustomConvwerter<ItemViewModel, Item>((v, t) => v.Id == t.Id));

            Mapper.CreateMap<Item, ItemViewModel>();
            Mapper.CreateMap<ItemViewModel, Item>()
                .ForMember(i => i.Solution, opt => opt.Ignore());

            Mapper.CreateMap<Solution, Input>()
              .ForMember(i => i.SolutionName, opt => opt.MapFrom(s => s.Name))
              .ForMember(i => i.Code, opt => opt.ResolveUsing<CodeResolver>().ConstructedBy(() => new CodeResolver()));


            Mapper.CreateMap<ReferencesViewModel, Reference>();
            Mapper.CreateMap<Reference, ReferencesViewModel>();
            Mapper.CreateMap<Reference, ReferenceApi>();

            Mapper.CreateMap<Result, Solution>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore())
                .ForMember(e => e.Language, opt => opt.Ignore())
                .ForMember(e => e.References, opt => opt.Ignore())
                .ForMember(e => e.Classes, opt => opt.MapFrom(src => src.Classes));

            Mapper.CreateMap<ICollection<Engine.Model.Class>, ICollection<Domain.Entities.Class>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.Class, Domain.Entities.Class>((i, o) => i.Name == o.Name));

            Mapper.CreateMap<Engine.Model.Class, Domain.Entities.Class>()
               .ForMember(e => e.Id, opt => opt.Ignore());

            Mapper.CreateMap<ICollection<Engine.Model.Method>, ICollection<Domain.Entities.Method>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.Method, Domain.Entities.Method>((i, o) => i.Name == o.Name));
            Mapper.CreateMap<Engine.Model.Method, Domain.Entities.Method>()
                .ForMember(e => e.Id, opt => opt.Ignore());

            Mapper.CreateMap<ICollection<Engine.Model.Test>, ICollection<Domain.Entities.Test>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.Test, Domain.Entities.Test>((i, o) => i.Name == o.Name));
            Mapper.CreateMap<Engine.Model.Test, Domain.Entities.Test>()
                .ForMember(e => e.Id, opt => opt.Ignore());
            #endregion Setup

            #region UnitTes
            Mapper.CreateMap<UnitTestSolution, UnitTestIndexItemViewModel>();
            #endregion UnitTes
        }
    }

    public class CodeResolver : ValueResolver<Solution, string[]>
    {
        protected override string[] ResolveCore(Solution solution)
        {
            List<string> codes = new List<string>();
            var items = solution.Items?.ToList() ?? new List<Item>();
            foreach (Item i in items)
            {
                codes.Add(i.Code);
            }
            return codes.ToArray();
        }
    }

    public class ClassesCountResolver : ValueResolver<Solution, int>
    {
        protected override int ResolveCore(Solution solution)
        {
            return solution.Classes?.Count() ?? 0;
        }
    }

    public class MethodsCountResolver : ValueResolver<Solution, int>
    {
        protected override int ResolveCore(Solution solution)
        {
            return solution.Classes?.SelectMany(c => c.Methods)?.Count() ?? 0;
        }
    }
    public class TestsCountResolver : ValueResolver<Solution, int>
    {
        protected override int ResolveCore(Solution solution)
        {
            return solution.Classes?.SelectMany(c => c.Methods)?.SelectMany(m => m.Tests)?.Count() ?? 0;
        }
    }

}