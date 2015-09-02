using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ninject;
using Testility.Domain.Abstract;
using Testility.Domain.Concrete;
using Testility.Egine.Concrete;
using Testility.Engine.Abstract;
using Testility.Logger.Abstract;
using Testility.Logger.Concrete;
using Testility.WebUI.Services.Abstract;
using Testility.WebUI.Services.Concrete;

namespace Testility.WebUI.Infrastructure.Ninject
{
    public class NinjectRegistration : NinjectModule
    {
        public override void Load()
        {
            Bind<IEFDbContext>().To<EFDbContext>();
            Bind<ISetupRepository>().To<EFSetupRepository>();
            Bind<IUnitTestRepository>().To<UnitTestRepository>();
            Bind<ICompiler>().To<CompilerProxy>();
            Bind<ICompilerService>().To<CompilerService>();
            Bind<ITestGenarator>().To<TestGenarator>();
            Bind<ILogger>().To<TraceLogger>();
            Bind<IIdentityServices>().To<IdentityServices>();

            Bind<EFDbContext>().ToSelf();
            Bind(typeof(IUserStore<>)).To(typeof(UserStore<>)).WithConstructorArgument("context", Kernel.Get<EFDbContext>());
            Bind(typeof(UserManager<>)).To(typeof(UserManager<>)).WithConstructorArgument("store", Kernel.Get<IUserStore<IdentityUser>>());

            Bind<IAuthenticationManager>().ToMethod(c => HttpContext.Current.GetOwinContext().Authentication);
        }
    }
}
