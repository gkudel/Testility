using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Testility.Logger.Abstract;
using Testility.WebUI.Infrastructure;
using Testility.WebUI.Infrastructure.Ninject;

namespace Testility.WebUI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {            
            AreaRegistration.RegisterAllAreas();
            Infrastructure.Ninject.Ninject.Register();
            Logger.Concrete.Logger.Initalize(Infrastructure.Ninject.Ninject.GetService(typeof(ILogger)) as ILogger);
            AutoMapperConfiguration.Configure();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
