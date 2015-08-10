using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Testility.Logger.Abstract;
using Testility.WebUI.Infrastructure;

namespace Testility.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            NinjectDependencyResolver diResolver = new NinjectDependencyResolver();
            AreaRegistration.RegisterAllAreas();
            DependencyResolver.SetResolver(diResolver);
            Logger.Concrete.Logger.Initalize(diResolver.GetService(typeof(ILogger)) as ILogger);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
