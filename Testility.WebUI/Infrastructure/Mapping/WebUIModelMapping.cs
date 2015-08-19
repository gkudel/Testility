using AutoMapper;
using System.Collections.Generic;
using Testility.Domain.Entities;
using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Testility.Engine.Model;
using Testility.WebUI.Model;
using Testility.Domain.Infrastructure.Mapping;
using Testility.WebUI.Areas.Setup.Models;

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
            Mapper.CreateMap<Solution, SolutionApi>();

            Mapper.CreateMap<Solution, SolutionIndexItemVM>()
                .ForMember(i => i.Summary, opt => opt.ResolveUsing<SummaryResolver>().ConstructedBy(() => new SummaryResolver()));

            Mapper.CreateMap<Solution, SolutionVM>();
            Mapper.CreateMap<SolutionVM, Solution>()
                .ForMember(s => s.CompiledDll, opt => opt.Ignore());

            Mapper.CreateMap<Reference, ReferencedAssemblies>()
                 .ForMember(i => i.ReferenceId, opt => opt.MapFrom(s=>s.Id));

            Mapper.CreateMap<ICollection<ItemVM>, ICollection<Item>>()
                .ConvertUsing(new CustomConvwerter<ItemVM, Item>((v, t) => v.Id == t.Id));

            Mapper.CreateMap<Item, ItemVM>();
            Mapper.CreateMap<ItemVM, Item>()
                .ForMember(i => i.Solution, opt => opt.Ignore());

            Mapper.CreateMap<Solution, Input>()
              .ForMember(i => i.SolutionName, opt => opt.MapFrom(s => s.Name))
              .ForMember(i => i.Code, opt => opt.ResolveUsing<CodeResolver>().ConstructedBy(() => new CodeResolver()));


            Mapper.CreateMap<ReferencesViewModel, Reference>();
            Mapper.CreateMap<Reference, ReferencesViewModel>();
            Mapper.CreateMap<IQueryable<Reference>, IQueryable<ReferencesViewModel>>();

            Mapper.CreateMap<Result, Solution>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore())
                .ForMember(e => e.Language, opt => opt.Ignore())
                .ForMember(e => e.ReferencedAssemblies, opt => opt.Ignore())
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
            Mapper.CreateMap<UnitTestSolution, UnitTestIndexItemVM>();
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

    public class SummaryResolver : ValueResolver<Solution, string>
    {
        protected override string ResolveCore(Solution solution)
        {
            string ret = "Classes[{0}], Methods[{1}], Tests[{2}]";
            ret = string.Format(ret, solution.Classes?.Count() ?? 0,
                solution.Classes?.SelectMany(c => c.Methods)?.Count() ?? 0 ,
                solution.Classes?.SelectMany(c => c.Methods)?.SelectMany(m => m.Tests)?.Count() ?? 0);
            return ret;
        }
    }
}