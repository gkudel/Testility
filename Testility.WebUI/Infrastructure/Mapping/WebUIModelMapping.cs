using AutoMapper;
using System.Collections.Generic;
using Testility.Domain.Entities;
using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Testility.Engine.Model;
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
            Mapper.CreateMap<Solution, Input>()
              .ForMember(i => i.SolutionName, opt => opt.MapFrom(s => s.Name))
              .ForMember(i => i.Code, opt => opt.ResolveUsing<CodeResolver>());


            Mapper.CreateMap<Result, Solution>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore())
                .ForMember(e => e.Language, opt => opt.Ignore())
                .ForMember(e => e.ReferencedAssemblies, opt => opt.Ignore())
                .ForMember(e => e.Classes, opt => opt.MapFrom(src => src.Classes));

            Mapper.CreateMap<IList<Engine.Model.Class>, ICollection<Domain.Entities.Class>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.Class, Domain.Entities.Class>((i, o) => i.Name == o.Name));

            Mapper.CreateMap<Engine.Model.Class, Domain.Entities.Class>()
               .ForMember(e => e.Id, opt => opt.Ignore());

            Mapper.CreateMap<IList<Engine.Model.Method>, ICollection<Domain.Entities.Method>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.Method, Domain.Entities.Method>((i, o) => i.Name == o.Name));
            Mapper.CreateMap<Engine.Model.Method, Domain.Entities.Method>()
                .ForMember(e => e.Id, opt => opt.Ignore());

            Mapper.CreateMap<IList<Engine.Model.Test>, ICollection<Domain.Entities.Test>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.Test, Domain.Entities.Test>((i, o) => i.Name == o.Name));
            Mapper.CreateMap<Engine.Model.Test, Domain.Entities.Test>()
                .ForMember(e => e.Id, opt => opt.Ignore());

            Mapper.CreateMap<ReferencesViewModel, Reference>();
            Mapper.CreateMap<Reference, ReferencesViewModel>();
            Mapper.CreateMap<IQueryable<Reference>, IQueryable<ReferencesViewModel>>();
            

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

    public class CustomConvwerter<T, V> : ITypeConverter<IList<T>, ICollection<V>>
    {
        private Func<T, V, bool> predicate;
        public CustomConvwerter(Func<T, V, bool> predicate)
        {
            this.predicate = predicate;
        }

        public ICollection<V> Convert(ResolutionContext context)
        {
            IList<T> s = context.SourceValue as IList<T>;
            ICollection<V> dest = null;
            if (s != null)
            {
                dest = context.DestinationValue as ICollection<V>;
                if (dest == null)
                {
                    dest = s.Select(c => Mapper.Map<T, V>(c)).ToList();
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