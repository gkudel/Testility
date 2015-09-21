using AutoMapper;
using System.Collections.Generic;
using Testility.Domain.Entities;
using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Testility.Engine.Model;
using Testility.WebUI.Model;
using Testility.WebUI.Areas.Setup.Model;
using Testility.WebUI.Areas.Authorization.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;

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
            Mapper.CreateMap<UnitTestSolution, SolutionViewModel>()
                .ForMember(s => s.References, opt => opt.MapFrom(s => s.References != null ? s.References.Select(solution => solution.Id).ToArray() : new int[0]))
                .ForMember(s => s.Items, opt => opt.MapFrom(s => s.Items == null || s.Items.Count == 0 ? new ItemViewModel[0] : s.Items.Select(i => Mapper.Map<ItemViewModel>(i)).ToArray()))
                .ForMember(s => s.SetupId, opt => opt.MapFrom(u => u.SetupSolutionId));

            Mapper.CreateMap<SetupSolution, SolutionViewModel>()
                .ForMember(s => s.References, opt => opt.MapFrom(s => s.References != null ? s.References.Select(solution => solution.Id).ToArray() : new int[0]))
                .ForMember(s => s.Items, opt => opt.MapFrom(s => s.Items == null || s.Items.Count == 0 ? new ItemViewModel[0] : s.Items.Select(i => Mapper.Map<ItemViewModel>(i)).ToArray()))
                .ForMember(s => s.SetupId, opt => opt.MapFrom(s => s.Id));                

            Mapper.CreateMap<SolutionViewModel, UnitTestSolution>()
                .ForMember(s => s.References, opt => opt.Ignore())
                .ForMember(s => s.SetupSolutionId, opt => opt.MapFrom(u => u.SetupId));

            Mapper.CreateMap<SolutionViewModel, SetupSolution>()
                .ForMember(s => s.References, opt => opt.Ignore());               

            Mapper.CreateMap<ICollection<ItemViewModel>, ICollection<Item>>()
                .ConvertUsing(new CustomConvwerter<ItemViewModel, Item>((v, t) => v.Id == t.Id));

            Mapper.CreateMap<Item, ItemViewModel>();
            Mapper.CreateMap<ItemViewModel, Item>()
                .ForMember(i => i.Solution, opt => opt.Ignore());

            Mapper.CreateMap<SetupSolution, SolutionIndexItemViewModel>()
                .ForMember(i => i.Classes, opt => opt.ResolveUsing<ClassesCountResolver>().ConstructedBy(() => new ClassesCountResolver()))
                .ForMember(i => i.Methods, opt => opt.ResolveUsing<MethodsCountResolver>().ConstructedBy(() => new MethodsCountResolver()))
                .ForMember(i => i.Tests, opt => opt.ResolveUsing<TestsCountResolver>().ConstructedBy(() => new TestsCountResolver()));

            Mapper.CreateMap<Solution, Input>();
            
            Mapper.CreateMap<SetupSolution, Input>()
              .IncludeBase<Solution, Input>()
              .ForMember(i => i.SolutionName, opt => opt.MapFrom(s => s.Name))
              .ForMember(i => i.Code, opt => opt.ResolveUsing<SetupCodeResolver>().ConstructedBy(() => new SetupCodeResolver()));

            Mapper.CreateMap<UnitTestSolution, Input>()
                .IncludeBase<Solution, Input>()
                .ForMember(i => i.SolutionName, opt => opt.MapFrom(s => s.Name))
                .ForMember(i => i.Code, opt => opt.ResolveUsing<UnitTestCodeResolver>().ConstructedBy(() => new UnitTestCodeResolver()));

            Mapper.CreateMap<Result, Solution>()
                .Include<Result, UnitTestSolution>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore())
                .ForMember(e => e.Language, opt => opt.Ignore())
                .ForMember(e => e.References, opt => opt.Ignore())
                .AfterMap((r, s) =>
                {
                    s.Items.Where(i => i.SolutionId == 0).ToList().ForEach(i => i.Id = s.Id);
                });


            Mapper.CreateMap<Result, SetupSolution>()
                .IncludeBase<Result, Solution>()
                .ForMember(e => e.Classes, opt => opt.MapFrom(src => src.Classes))
                .AfterMap((r, s) =>
                {
                    s.Classes.Where(c => c.SolutionId == 0).ToList().ForEach(c => c.SolutionId = s.Id);
                });

            Mapper.CreateMap<Result, IList<Engine.Model.Test>>()
                .ConvertUsing((r) => {
                    List<Engine.Model.Test> testList = new List<Engine.Model.Test>();
                    foreach (Engine.Model.Class c in r.Classes?.Where(c => c.Methods?.Where(m => m.Tests?.Count() > 0)?.Count() > 0))
                    {
                        foreach (Testility.Engine.Model.Test t in c.Methods.Where(m => m.Tests?.Count() > 0).SelectMany(m => m.Tests, (m, t) => t))
                        {
                            testList.Add(t);
                        }
                    }
                    return testList;
                });

            Mapper.CreateMap<ICollection<Engine.Model.Class>, ICollection<Domain.Entities.Class>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.Class, Domain.Entities.Class>((i, o) => i.Name == o.Name));

            Mapper.CreateMap<Engine.Model.Class, Domain.Entities.Class>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .AfterMap((c1, c2) =>
                {
                    c2.Methods.Where(m => m.ClassId == 0).ToList().ForEach(m => m.ClassId = c2.Id);
                });

            Mapper.CreateMap<ICollection<Engine.Model.Method>, ICollection<Domain.Entities.Method>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.Method, Domain.Entities.Method>((i, o) => i.Name == o.Name));

            Mapper.CreateMap<Engine.Model.Method, Domain.Entities.Method>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .AfterMap((m1, m2) =>
                {
                    m2.Tests.Where(t => t.MethodId == 0).ToList().ForEach(t => t.MethodId = m2.Id);
                });


            Mapper.CreateMap<ICollection<Engine.Model.Test>, ICollection<Domain.Entities.Test>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.Test, Domain.Entities.Test>((i, o) => i.Name == o.Name));
            Mapper.CreateMap<Engine.Model.Test, Domain.Entities.Test>()
                .ForMember(e => e.Id, opt => opt.Ignore());

            Mapper.CreateMap<ReferencesViewModel, Reference>();
            Mapper.CreateMap<Reference, ReferencesViewModel>();
            #endregion Setup

            #region UnitTes
            Mapper.CreateMap<UnitTestSolution, UnitTestIndexItemViewModel>()
                .ForMember(e => e.SetupName, opt => opt.MapFrom(u => u.SetupSolution.Name));
            #endregion UnitTes


            #region Identity

            Mapper.CreateMap<RegisterVM, IdentityUser>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(a => a.Name))
                .ForMember(z=>z.Id, opt => opt.Ignore());

            Mapper.CreateMap<IdentityUser, RegisterVM>().ForMember(x => x.Name, opt => opt.MapFrom(a => a.UserName));

            Mapper.CreateMap<LoginVM, IdentityUser>();

            Mapper.CreateMap<ExternalLoginConfirmationVM, IdentityUser>()
                  .ForMember(x => x.UserName, opt => opt.MapFrom(a => a.Name))
                  .ForMember(x => x.Email, opt => opt.MapFrom(a => a.Email));

            #endregion Identity
        }
    }

    public class SetupCodeResolver : ValueResolver<SetupSolution, string[]>
    {
        protected override string[] ResolveCore(SetupSolution solution)
        {
            List<string> codes = new List<string>();
            var items = solution.Items?.ToList() ?? new List<Item>();
            foreach (Item i in items)
            {
                string code = "using Testility.Engine.Attribute; " + i.Code;                
                codes.Add(code);
            }
            return codes.ToArray();
        }
    }
    public class UnitTestCodeResolver : ValueResolver<UnitTestSolution, string[]>
    {
        protected override string[] ResolveCore(UnitTestSolution solution)
        {
            List<string> codes = new List<string>();
            var items = solution.Items?.ToList() ?? new List<Item>();            
            foreach (Item i in items)
            {
                codes.Add(i.Code);
            }
            items = solution.SetupSolution?.Items?.ToList() ?? new List<Item>();
            foreach (Item i in items)
            {
                string code = "using Testility.Engine.Attribute; " + i.Code;
                codes.Add(code);
            }

            return codes.ToArray();
        }
    }
    
    public class ClassesCountResolver : ValueResolver<SetupSolution, int>
    {
        protected override int ResolveCore(SetupSolution solution)
        {
            return solution.Classes?.Count() ?? 0;
        }
    }

    public class MethodsCountResolver : ValueResolver<SetupSolution, int>
    {
        protected override int ResolveCore(SetupSolution solution)
        {
            return solution.Classes?.SelectMany(c => c.Methods)?.Count() ?? 0;
        }
    }
    public class TestsCountResolver : ValueResolver<SetupSolution, int>
    {
        protected override int ResolveCore(SetupSolution solution)
        {
            return solution.Classes?.SelectMany(c => c.Methods)?.SelectMany(m => m.Tests)?.Count() ?? 0;
        }
    }

    public class CustomConvwerter<T, V> : ITypeConverter<ICollection<T>, ICollection<V>>
    {
        private Func<T, V, bool> predicate;
        public CustomConvwerter(Func<T, V, bool> predicate)
        {
            this.predicate = predicate;
        }

        public ICollection<V> Convert(ResolutionContext context)
        {
            ICollection<T> s = context.SourceValue as ICollection<T>;
            ICollection<V> dest = null;
            if (s != null)
            {
                dest = context.DestinationValue as ICollection<V>;
                if (dest == null)
                {
                    dest = new HashSet<V>(s.Select(c => Mapper.Map<T, V>(c)));
                }
                else
                {
                    List<V> toRemove = new List<V>();
                    foreach (V v in dest)
                    {
                        if (!s.Any(i => predicate(i, v)))
                        {
                            toRemove.Add(v);
                        }
                    }
                    toRemove.ForEach(r => dest.Remove(r));
                    foreach (T i in s)
                    {
                        V o = dest.FirstOrDefault(c => predicate(i, c));
                        if (o == null)
                        {
                            dest.Add(Mapper.Map<T, V>(i));
                        }
                        else
                        {
                            Mapper.Map<T, V>(i, o);
                        }
                    }
                }
            }
            return dest;
        }
    }
}