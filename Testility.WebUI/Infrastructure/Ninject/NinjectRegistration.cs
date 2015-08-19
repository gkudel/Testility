using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Bind<ISetupRepository>().To<EFSetupRepository>();
            Bind<IUnitTestRepository>().To<UnitTestRepository>();
            Bind<ICompiler>().To<CompilerProxy>();
            Bind<ICompilerService>().To<CompilerService>();
            Bind<ILogger>().To<TraceLogger>();
        }
    }
}
