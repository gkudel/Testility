using AutoMapper;
using System.Collections.Generic;
using Testility.Domain.Entities;
using System;
using System.Linq;
using AutoMapper.QueryableExtensions;

namespace Testility.Domain.Infrastructure.Mapping
{
    public class DomainModelMapping : Profile
    {
        public override string ProfileName
        {
            get { return "DomainModelMapping"; }
        }
        protected override void Configure()
        {
            Mapper.CreateMap<Item, Item>()
                .ForMember(i => i.Solution, opt => opt.Ignore());

            Mapper.CreateMap<ICollection<Item>, ICollection<Item>>()
                .ConvertUsing(new CustomConvwerter<Item, Item>((v, t) => v.Id == t.Id));

            Mapper.CreateMap<SolutionApi, SolutionApi>()
                .ForMember(s => s.Classes, opt => opt.Ignore());
        }
    }

    public class CustomConvwerter<T, V> : ITypeConverter<ICollection<T>, ICollection<V>>
    {
        private Func<T, V, bool> predicate;
        public CustomConvwerter(Func<T, V,bool> predicate)
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