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
using Testility.Utils.Extensions;

namespace Testility.Egine.Concrete
{
    public class CompilerProxy : ICompiler
    {
        private Result compile(Input input)
        {
            Compiler compiler;
            Result r;
            System.AppDomain unitDomain = null; 
            try
            {
                Evidence evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
                AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
                unitDomain = AppDomain.CreateDomain("uTestDomain", evidence, setup);
                Type type = typeof(Compiler);
                compiler = (Compiler)unitDomain.CreateInstanceFrom(
                        type.Assembly.Location,
                        type.FullName).Unwrap();
                foreach (string s in GetAssemblies(Assembly.GetExecutingAssembly()))
                {
                    compiler.LoadAssembly(s);
                }
                r = compiler.Compile(input);
            }
            finally
            {
                if (unitDomain != null) System.AppDomain.Unload(unitDomain);
            }
            return r;
        }

        private Result runTests(Input i, Result r)
        {
            if (r != null)
            {
                if (r.Errors.Count() == 0 && !string.IsNullOrEmpty(r.TemporaryFile))
                {
                    System.AppDomain unitDomain = null;
                    TestRuner runner;
                    try
                    {
                        Evidence evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
                        AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
                        unitDomain = AppDomain.CreateDomain("uTestDomain", evidence, setup);
                        Type type = typeof(TestRuner);
                        runner = (TestRuner)unitDomain.CreateInstanceFrom(
                                type.Assembly.Location,
                                type.FullName).Unwrap();
                        foreach (string s in GetAssemblies(Assembly.GetExecutingAssembly()))
                        {
                            runner.LoadAssembly(s);
                        }
                        r = runner.Run(r.TemporaryFile, r);
                    }
                    finally
                    {
                        if (unitDomain != null) System.AppDomain.Unload(unitDomain);
                    }
                }
            }
            return r;
        }

        private Result invoke(Func<Input, Result> invoke, Input input)
        {
            Result ret = null;
            try
            {
                ret = invoke(input);
            }
            finally
            {                
                if (!string.IsNullOrEmpty(ret?.TemporaryFile ?? ""))
                {
                    if (File.Exists(ret.TemporaryFile))
                    {
                        File.Delete(ret.TemporaryFile);
                    }
                }
            }
            return ret;
        }

        public Result Compile(Input input)
        {
            return invoke(compile, input);
        }

        public Result RunTests(Input input)
        {
            Func<Result, Input, Result> testRuner = (x, y) => runTests(y, x);
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
