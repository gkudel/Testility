using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Testility.Logger.Abstract;
using Testility.WebUI.App_Start;
using Testility.WebUI.Infrastructure;
using Testility.WebUI.Mappings.Infrastructure;

namespace Testility.WebUI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            NinjectDependencyResolver diResolver = new NinjectDependencyResolver();           
            AreaRegistration.RegisterAllAreas();
            BindingConfig.RegisterBinding();
            DependencyResolver.SetResolver(diResolver);
            Logger.Concrete.Logger.Initalize(diResolver.GetService(typeof(ILogger)) as ILogger);
            AutoMapperConfigurationWebUI.Configure();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
