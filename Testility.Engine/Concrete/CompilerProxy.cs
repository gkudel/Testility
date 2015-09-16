using NUnit.Core;
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
using Testility.Engine.Utils;

namespace Testility.Egine.Concrete
{
    public class CompilerProxy : ICompiler
    {
        private class CompileResult
        {
            public Result Result { get; set; }
            public System.AppDomain UnitDomain { get; set; }
        }

        private CompileResult compile(Input input)
        {
            Compiler compiler;
            Result r = null;
            System.AppDomain unitDomain = null;
            Evidence evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
            AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
            unitDomain = AppDomain.CreateDomain("uTestDomain", evidence, setup);
            Type type = typeof(Compiler);
            compiler = (Compiler)unitDomain.CreateInstanceFrom(
                    type.Assembly.Location,
                    type.FullName).Unwrap();
            foreach (string s in GetAssemblies(type.Assembly))
            {
                compiler.LoadAssembly(s);
            }
            r = compiler.Compile(input);
            return new CompileResult() { Result = r, UnitDomain = unitDomain };
        }

        private CompileResult runTests(Input i, CompileResult r)
        {
            if (r != null && r.Result != null)
            {
                if (r.Result.Errors.Count() == 0 && !string.IsNullOrEmpty(r.Result.TemporaryFile))
                {
                    if (r?.UnitDomain != null)
                    {
                        System.AppDomain.Unload(r.UnitDomain);
                        r.UnitDomain = null;
                    }
                    RemoteTestRunner runner = new RemoteTestRunner();
                    TestPackage package = new TestPackage("Test");
                    package.Assemblies.Add(r.Result.TemporaryFile);
                    foreach (string s in GetAssemblies(Assembly.GetExecutingAssembly()))
                    {
                        package.Assemblies.Add(s);
                    }
                    if (runner.Load(package))
                    {
                        TestResult result = runner.Run(new NullListener(), NUnit.Core.TestFilter.Empty, false, LoggingThreshold.All);
                    }
                }
            }
            return r;
        }

        private Result invoke(Func<Input, CompileResult> invoke, Input input)
        {
            CompileResult ret = null;
            try
            {
                ret = invoke(input);
            }
            finally
            {
                if (ret?.UnitDomain != null) System.AppDomain.Unload(ret.UnitDomain);
                if (!string.IsNullOrEmpty(ret?.Result?.TemporaryFile ?? ""))
                {
                    if (File.Exists(ret.Result.TemporaryFile))
                    {
                        File.Delete(ret.Result.TemporaryFile);
                    }
                }
            }
            return ret?.Result ?? null;
        }

        public Result Compile(Input input)
        {
            return invoke(compile, input);
        }

        public Result RunTests(Input input)
        {
            Func<CompileResult, Input, CompileResult> testRuner = (x, y) => runTests(y, x);
            return invoke(testRuner.Curry()(compile(input)), input);
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

    }
}
