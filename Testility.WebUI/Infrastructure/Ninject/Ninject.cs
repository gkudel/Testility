using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace Testility.WebUI.Infrastructure.Ninject
{
    public static class Ninject
    {
        public static void Register()
        {
            NinjectModule registrations = new NinjectRegistration();
            var kernel = new StandardKernel(registrations);
            var ninjectResolver = new NinjectDependencyResolver(kernel);

            DependencyResolver.SetResolver(ninjectResolver); 
            GlobalConfiguration.Configuration.DependencyResolver = ninjectResolver;
        }

        public static object GetService(Type serviceType)
        {
            return DependencyResolver.Current.GetService(serviceType);
        }
    }
}
