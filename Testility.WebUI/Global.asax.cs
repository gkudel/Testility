using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Testility.Logger.Abstract;
using Testility.WebUI.Infrastructure;

namespace Testility.WebUI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            NinjectDependencyResolver diResolver = new NinjectDependencyResolver();           
            AreaRegistration.RegisterAllAreas();
            DependencyResolver.SetResolver(diResolver);
            Logger.Concrete.Logger.Initalize(diResolver.GetService(typeof(ILogger)) as ILogger);
            AutoMapperConfiguration.Configure();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
