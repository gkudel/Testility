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
using NUnit.Core;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Testility.Engine.Concrete
{
    public class Compiler : MarshalByRefObject, ICompiler
    {

        Result ICompiler.Compile(Input input)
        {                         
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

                var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().ToList();
                var assemblyLocations = assemblies.Select(a =>
                  Assembly.ReflectionOnlyLoad(a.FullName).Location).ToList();
                assemblyLocations.Add(Assembly.GetExecutingAssembly().Location);

                foreach (string reference in input.ReferencedAssemblies)
                {                    
                    if (assemblies.FirstOrDefault(a => a.Name == Path.GetFileNameWithoutExtension(reference)) == null)
                    {
                        compilerparameters.ReferencedAssemblies.Add(reference);
                    }
                }
                compilerparameters.ReferencedAssemblies.AddRange(assemblyLocations.ToArray());


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
                                .Where(a => typeof(Attribute.TestAttribute).IsInstanceOfType(a))
                                .Select(a => a as Attribute.TestAttribute);
                            foreach (Attribute.TestAttribute a in tests)
                            {
                                testedmethod.Tests.Add(new Model.Test()
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
                        result.Errors.Add(new Error()
                        {
                                ErrorText = error.ErrorText,
                                Column = error.Column,
                                ErrorNumber = error.ErrorNumber,
                                IsWarning = error.IsWarning,
                                Line = error.Line
                        });
                    }
                }
            }
            else
            {
                throw new ArgumentException("Language cannot be recognised");
            }
            return result;
        }

        [SuppressMessage("Microsoft.Performance",
                     "CA1822:MarkMembersAsStatic")]
        public void LoadAssembly(String assemblyPath)
        {
            try
            {
                Assembly.LoadFile(assemblyPath);
            }
            catch (FileNotFoundException)
            {
            }
        }

        [TestFixture]
        private class FakeClass
        {
            [TestFixtureSetUp]
            private void Init()
            {
                {
                }
            }

            [TestFixtureTearDown]
            private void Dispose()
            {
                {
                }
            }

            [NUnit.Framework.Test]
            private void Test()
            { }
        }

    }
}
