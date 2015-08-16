using AutoMapper;

namespace Testility.Domain.Infrastructure.Mappings
{
    public class AutoMapperConfigurationDomain
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainModelMapping>();
            });
        }
    }
}