﻿using System;
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
            if(input.Code == null || input.Code.Length == 00) throw new ArgumentNullException("Source Code can not be null");
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

                foreach (String references in input.ReferencedAssemblies.Split(';'))
                {
                    compilerparameters.ReferencedAssemblies.Add(references);
                }

                compilingResult = provider.CompileAssemblyFromSource(compilerparameters, input.Code);

                if (compilingResult.Errors.Count == 0)
                {
                    var types = compilingResult.CompiledAssembly.GetTypes().SelectMany(t =>
                        System.Attribute.GetCustomAttributes(t).Where(a => typeof(TestedClassesAttribute).IsInstanceOfType(a)),
                                    (t, a) => new { type = t, attribute = a as TestedClassesAttribute });

                    foreach (var t in types)
                    {
                        Class testedclass = new Class()
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
                            Method testedmethod = new Method()
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
                        if (testedclass.Methods.Count() > 0)
                        {
                            result.Classes.Add(testedclass);
                        }
                        else
                        {
                            result.Errors.Add(new Error() { Message = "TestedClass class defined without TestedMethods" });
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
