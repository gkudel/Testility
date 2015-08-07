using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Testility.Engine.Abstract;
using Testility.Engine.Attribute;
using Testility.Engine.Model;

namespace Testility.Engine.Concrete
{
    public class Compiler : ICompiler
    {
        public Result compile(Input input)
        {
            Result result = new Result();
            CodeDomProvider provider = CodeDomProvider.CreateProvider(input.Language);
            if (provider != null)
            {
                CompilerParameters cp = new CompilerParameters();

                cp.GenerateExecutable = false;
                cp.GenerateInMemory = false;
                cp.OutputAssembly = string.Format(@"{0}\{1}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), input.Name + ".dll");
                cp.TreatWarningsAsErrors = false;

                string code = input.Code;
                code = "using Testility.Engine.Attribute; " + code;
                cp.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

                foreach (String references in input.ReferencedAssemblies.Split(';'))
                {
                    cp.ReferencedAssemblies.Add(references);
                }
                
                var results = provider.CompileAssemblyFromSource(cp, new string[] { code });

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
                        if (testedclass.Methods.Where(m => m.Tests.Count() > 0).Count() > 0) result.TestedClasses.Add(testedclass);
                    }
                }
            }
            return result;
        }
    }
}
