using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Testility.Engine.Abstract;
using Testility.Engine.Concrete;
using Testility.Engine.Model;

namespace Testility.Egine.Concrete
{
    public class CompilerProxy : ICompiler, IDisposable
    {
        private Compiler compiler;
        System.AppDomain unitDomain;
        private void Init()
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Evidence evidence = System.AppDomain.CurrentDomain.Evidence;

            unitDomain = System.AppDomain.CreateDomain("uTest", evidence, setup);
            
            Assembly asm = unitDomain.Load(File.ReadAllBytes(Assembly.GetExecutingAssembly().Location));
            Type type = typeof(Compiler);
            compiler = (Compiler)unitDomain.CreateInstanceAndUnwrap(
                    asm.GetName().Name,
                    type.FullName);
        }
        private void Finish(Result r)
        {
            if(unitDomain  != null) System.AppDomain.Unload(unitDomain);
            if (!string.IsNullOrEmpty(r?.OutputDll ?? ""))
            {
                if (File.Exists(r.OutputDll))
                {
                    File.Delete(r.OutputDll);
                }
            }
        }
        public Result Compile(Input input)
        {
            Result r = null;
            try
            {
                Init();
                r = compiler.Compile(input);
            }
            finally
            {
                Finish(r);
            }
            return r;
        }
        public void Dispose()
        {
            System.AppDomain.Unload(unitDomain);
        }
    }
}
