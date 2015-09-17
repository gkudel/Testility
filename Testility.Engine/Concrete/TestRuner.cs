using NUnit.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Testility.Engine.Model;

namespace Testility.Engine.Concrete
{
    public class TestRuner : MarshalByRefObject
    {
        public Result Run(string file, Result compileResult)
        {
            RemoteTestRunner runner = new RemoteTestRunner();
            TestPackage package = new TestPackage("Test");
            package.Assemblies.Add(file);
            if (runner.Load(package))
            {
                TestResult result = runner.Run(new TestListner(compileResult), new TestFilter(), false, LoggingThreshold.All);
            }
            return compileResult;
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void LoadAssembly(String assemblyPath)
        {
            try { Assembly.LoadFile(assemblyPath); } catch (FileNotFoundException){}
        }

        private class TestFilter : NUnit.Core.TestFilter
        {
            public override bool Match(ITest test)
            {
                return true;             
            }
        }
        private class TestListner : EventListener
        {
            private List<Testility.Engine.Model.Test> testList;
            public TestListner(Result compileResult)
            {
                testList = new List<Model.Test>();
                foreach (Class c in compileResult.Classes?.Where(c => c.Methods?.Where(m => m.Tests?.Count() > 0)?.Count() > 0))
                {
                    foreach (Testility.Engine.Model.Test t in c.Methods.Where(m => m.Tests?.Count() > 0).SelectMany(m => m.Tests, (m, t) => t))
                    {
                        testList.Add(t);
                    }
                }
            }

            public void RunFinished(Exception exception)
            {
            }

            public void RunFinished(TestResult result)
            {
            }

            public void RunStarted(string name, int testCount)
            {
            }

            public void SuiteFinished(TestResult result)
            {
            }

            public void SuiteStarted(TestName testName)
            {
            }

            public void TestFinished(TestResult result)
            {
                Testility.Engine.Model.Test t = testList.FirstOrDefault(test => test.Name + "_Test" == result.Name);
                if (t != null)
                {
                    t.RunFail = !result.IsSuccess;
                }
            }

            public void TestOutput(TestOutput testOutput)
            {
            }

            public void TestStarted(TestName testName)
            {
                Testility.Engine.Model.Test t = testList.FirstOrDefault(test => test.Name + "_Test" == testName.Name);
                if (t != null) t.HasBeenRun = true;
            }

            public void UnhandledException(Exception exception)
            {
            }
        }
    }
}
