using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
        private ICompiler compiler;
        System.AppDomain unitDomain;
        private void Init()
        {
            Evidence evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
            AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
            unitDomain = AppDomain.CreateDomain("DiscoveryRegion", evidence, setup);
            Type type = typeof(Compiler);
            compiler = (Compiler)unitDomain.CreateInstanceFrom(
                    type.Assembly.Location,
                    type.FullName).Unwrap();
            foreach (string s in GetAssemblies(type.Assembly))
            {
                compiler.LoadAssembly(s);
            }
        }

        private IEnumerable<string> GetAssemblies(Assembly ass)
        {
            AssemblyName[] assNames = ass.GetReferencedAssemblies();
            foreach (AssemblyName assName in assNames)
            {
                Assembly referedAss = Assembly.Load(assName);
                if (!referedAss.GlobalAssemblyCache)
                {
                    yield return referedAss.Location;
                    GetAssemblies(referedAss);
                }
            }
        }

        private void Finish(Result r)
        {
            if (unitDomain != null) System.AppDomain.Unload(unitDomain);
            if (!string.IsNullOrEmpty(r?.TemporaryFile ?? ""))
            {
                if (File.Exists(r.TemporaryFile))
                {
                    File.Delete(r.TemporaryFile);
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
            Finish(null);
        }

        public void LoadAssembly(string assemblyPath)
        {
            throw new NotImplementedException();
        }
    }
}
