using AutoMapper;
using System.Collections.Generic;
using Testility.Domain.Entities;
using Testility.Engine.Model;
using System;
using System.Linq;
using AutoMapper.QueryableExtensions;

namespace Testility.WebUI.Mappings.Infrastructure
{
    public class UIModelToEngineModelMappings : Profile
    {        
         public override string ProfileName
        {
            get { return "UiModelToEngineModelMappings"; }
        }
        protected override void Configure()
        {
            Mapper.CreateMap<Item, Input>();
            Mapper.CreateMap<Input, Item>();
            Mapper.CreateMap<Item, Item>()
                .ForMember(s => s.Clasess, opt => opt.Ignore());

            Mapper.CreateMap<Result, Item>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore())
                .ForMember(e => e.Code, opt => opt.Ignore())
                .ForMember(e => e.Language, opt => opt.Ignore())
                .ForMember(e => e.ReferencedAssemblies, opt => opt.Ignore())
                .ForMember(e => e.Clasess, opt => opt.MapFrom(src => src.TestedClasses));

            Mapper.CreateMap<IList<Engine.Model.TestedClass>, ICollection<Domain.Entities.TestedClass>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.TestedClass, Domain.Entities.TestedClass>((i,o) => i.Name == o.Name));

            Mapper.CreateMap<Engine.Model.TestedClass, Domain.Entities.TestedClass>()
               .ForMember(e => e.Id, opt => opt.Ignore());

            Mapper.CreateMap<IList<Engine.Model.TestedMethod>, ICollection<Domain.Entities.TestedMethod>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.TestedMethod, Domain.Entities.TestedMethod>((i, o) => i.Name == o.Name));
            Mapper.CreateMap<Engine.Model.TestedMethod, Domain.Entities.TestedMethod>()
                .ForMember(e => e.Id, opt => opt.Ignore());

            Mapper.CreateMap<IList<Engine.Model.Test>, ICollection<Domain.Entities.Test>>()
                .ConvertUsing(new CustomConvwerter<Engine.Model.Test, Domain.Entities.Test>((i, o) => i.Name == o.Name));
            Mapper.CreateMap<Engine.Model.Test, Domain.Entities.Test>()
                .ForMember(e => e.Id, opt => opt.Ignore());
        }
    }

    public class CustomConvwerter<T, V> : ITypeConverter<IList<T>, ICollection<V>>
    {
        private Func<T, V, bool> predicate;
        public CustomConvwerter(Func<T, V,bool> predicate)
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