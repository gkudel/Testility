using AutoMapper;

namespace Testility.WebUI.Mappings.Infrastructure
{
    public class AutoMapperConfigurationWebUI
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<UIModelToEngineModelMappings>();
            });
        }
    }
}