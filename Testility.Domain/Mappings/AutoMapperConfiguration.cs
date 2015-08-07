using AutoMapper;
using Testility.Domain.Mappings;

namespace Testility.Domain.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ModelToModelMapping>();
            });
        }
    }
}