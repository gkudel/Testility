using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
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
    public class Compiler : MarshalByRefObject, ICompiler
    {
        public Result Compile(Input input)
        {
            Contract.Requires<ArgumentNullException>(input.Code != null && input.Code.Length > 0);            
            Result result = new Result();
            CodeDomProvider provider = null;
            try
            {
                provider = CodeDomProvider.CreateProvider(input.Language);
            }
            catch (ConfigurationException)
            { }
            if (provider != null)
            {
                CompilerParameters compilerparameters = new CompilerParameters();
                CompilerResults compilingResult = null;
                compilerparameters.GenerateExecutable = false;
                compilerparameters.GenerateInMemory = false;
                result.TemporaryFile = compilerparameters.OutputAssembly = string.Format(@"{0}\{1}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), input.SolutionName+ ".dll");
                compilerparameters.TreatWarningsAsErrors = false;

                for(int i =0; i<input.Code.Length; i++)
                {
                    input.Code[i] = "using Testility.Engine.Attribute; " + input.Code[i];
                    
                }
                compilerparameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

                foreach (String references in input.ReferencedAssemblies)
                {
                    compilerparameters.ReferencedAssemblies.Add(references);
                }

                compilingResult = provider.CompileAssemblyFromSource(compilerparameters, input.Code);

                if (compilingResult.Errors.Count == 0)
                {
                    var types = compilingResult.CompiledAssembly.GetTypes();

                    foreach (var t in types)
                    {
                        TestedClassesAttribute attribute = System.Attribute.GetCustomAttributes(t)
                            .Where(a => typeof(TestedClassesAttribute).IsInstanceOfType(a)).FirstOrDefault() as TestedClassesAttribute;
                        Class testedclass = new Class()
                        {
                            Name = attribute?.Name ?? t.Name,
                            Description = attribute?.Description ?? t.Name,
                        };
                        var methods = t.GetMethods().Where(m => m.IsPublic 
                                        && !m.IsConstructor && m.DeclaringType == t
                                        && !m.IsSpecialName);
                        foreach (var m in methods)
                        {
                            TestedMethodAttribute methodAttribiute = System.Attribute.GetCustomAttributes(m)
                                .Where(a => typeof(TestedMethodAttribute).IsInstanceOfType(a)).FirstOrDefault() as TestedMethodAttribute;
                            Method testedmethod = new Method()
                            {
                                Name = methodAttribiute?.Name ?? m.Name,
                                Description = methodAttribiute?.Description ?? m.Name
                            };
                            testedclass.Methods.Add(testedmethod);
                            var tests = System.Attribute.GetCustomAttributes(m)
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
                        if (testedclass.Methods.Count() > 0)
                        {
                            result.Classes.Add(testedclass);
                        }
                    }
                }
                else
                {
                    foreach (CompilerError error in compilingResult.Errors)
                    {
                        result.Errors.Add(new Error() { Message = error.ErrorText });
                    }
                }
            }
            else
            {
                throw new ArgumentException("Language cannot be recognised");
            }
            return result;
        }
    }
}
