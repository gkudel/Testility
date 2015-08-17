using AutoMapper;

namespace Testility.Domain.Infrastructure.Mapping
{
    public class DomainConfiguration
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