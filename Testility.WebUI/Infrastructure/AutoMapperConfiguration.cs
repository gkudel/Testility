using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Infrastructure.Mappings;

namespace Testility.WebUI.Infrastructure
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            AutoMapperConfigurationDomain.Configure();
        }
    }
}
