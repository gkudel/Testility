using AutoMapper;

namespace Testility.WebUI.Infrastructure.Mapping
{
    public class WebUIConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<WebUIModelMapping>();
            });
        }
    }
}