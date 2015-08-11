using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Testility.Engine.Abstract;
using Testility.Domain.Abstract;
using Testility.Domain.Concrete;
using Testility.Engine.Concrete;
using Testility.Logger.Abstract;
using Testility.Logger.Concrete;
using Testility.WebUI.Services;
using Testility.Egine.Concrete;

namespace Testility.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<ISetupRepository>().To<EFSetupRepository>();
            kernel.Bind<ICompiler>().To<CompilerProxy>();
            kernel.Bind<ICreateInputClassFromFile>().To<CreateInputClassFromFile>();
            kernel.Bind<ILogger>().To<TraceLogger>();
        }
    }
}
