using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Abstract;
using Testility.Domain.Attribute;
using Testility.Domain.Entities;

namespace Testility.Domain.Concrete
{
    public class Compiler : ICompiler
    {
        public IEnumerable<TestedClass> compile(string sourceCode)
        {
            List<TestedClass> list = new List<TestedClass>();
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            if (provider != null)
            {
                CompilerParameters cp = new CompilerParameters();

                cp.GenerateExecutable = false;
                cp.GenerateInMemory = true;
                cp.TreatWarningsAsErrors = false;

                cp.ReferencedAssemblies.Add("System.dll");
                cp.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
                var results = provider.CompileAssemblyFromSource(cp, new string[] { sourceCode });

                if (results.Errors.Count == 0)
                {
                    var types = results.CompiledAssembly.GetTypes().SelectMany(t =>
                        System.Attribute.GetCustomAttributes(t).Where(a => typeof(TestedClassesAttribute).IsInstanceOfType(a)),
                                    (t, a) => new { type = t, attribute = a as TestedClassesAttribute });

                    foreach (var t in types)
                    {
                        TestedClass testedclass = new TestedClass()
                        {
                            Name = t.attribute.Name,
                            Description = t.attribute.Description,                            
                        };
                        var methods = t.type.GetMethods().Where(m => m.IsPublic)
                            .SelectMany(m => System.Attribute.GetCustomAttributes(m)
                                    .Where(a => typeof(TestedMethodAttribute).IsInstanceOfType(a)),
                                     (m, a) => new { method = m, attribute = a as TestedMethodAttribute });
                        foreach (var m in methods)
                        {
                            TestedMethod testedmethod = new TestedMethod()
                            {
                                Name = m.attribute.Name,
                                Description = m.attribute.Description
                            };
                            testedclass.Methods.Add(testedmethod);
                            var tests = System.Attribute.GetCustomAttributes(m.method)
                                .Where(a => typeof(TestAttribute).IsInstanceOfType(a))
                                .Select(a => a as TestAttribute);
                            foreach (TestAttribute a in tests)
                            {
                                testedmethod.Tests.Add(new Test()
                                {
                                    Name = a.Name,
                                    Description = a.Description,
                                    Fail = a.Fail
                                });
                            }
                        }
                        if (testedclass.Methods.Where(m => m.Tests.Count() > 0).Count() > 0) list.Add(testedclass);
                    }
                }
            }
            return list;
        }
    }
}
