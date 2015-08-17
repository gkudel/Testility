using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Infrastructure.Mapping;
using Testility.WebUI.Infrastructure.Mapping;

namespace Testility.WebUI.Infrastructure
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainModelMapping>();
                x.AddProfile<WebUIModelMapping>();
            });
        }
    }
}
