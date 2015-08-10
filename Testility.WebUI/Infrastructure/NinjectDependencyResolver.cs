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
<<<<<<< HEAD
using Testility.WebUI.Services;
=======
using Testility.Logger.Abstract;
using Testility.Logger.Concrete;
>>>>>>> f309a544679c6ea2706a988d4e950cf8a5c40920

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
            kernel.Bind<IFileRepository>().To<FileService>();
            kernel.Bind<ICompiler>().To<Compiler>();
            kernel.Bind<ILogger>().To<TraceLogger>();
        }
    }
}
