using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testility.Engine.Concrete;
using Testility.Engine.Model;
using Testility.Engine.Abstract;

namespace Testility.UnitTests
{
    [TestClass]
    public class CompilerTest
    {
        public ICompiler compiler;

        [TestInitialize]
        public void Int()
        {
            compiler = new Compiler();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Compile_WithOutSourceCode_ArgumentException()
        {
            Input input = new Input();
            compiler.compile(input);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Compile_WithUnknowLanguage_ArgumentException()
        {
            Input input = new Input()
            {
                Code = "public class Test{}",
                Language = "Pascal"
            };
            compiler.compile(input);
        }

        [TestMethod]
        public void Compile_Improper_CompilingError()
        {
            Input input = new Input()
            {
                Code = "using System public class Test{}",
                Language = "CSharp",
                Name = "Test",
                ReferencedAssemblies = "System.dll"
            };

            Result result = compiler.compile(input);

            Assert.AreEqual(1, result.Errors.Count);
        }

        [TestMethod]
        public void Compile_ProperClass_Compiled()
        {
            Input input = new Input()
            {
                Code = "using System; public class Test{}",
                Language = "CSharp",
                Name = "Test",
                ReferencedAssemblies = "System.dll"
            };

            Result result = compiler.compile(input);

            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void Compile_TestedClassesWithOutTestedMethods_OneError()
        {
            Input input = new Input()
            {
                Code = @" using System;
                                        
                    [TestedClasses(Name = ""Account"", Description = ""Account class"")]
                    public class Account
                    {}", 
                Language = "CSharp",
                Name = "Test",
                ReferencedAssemblies = "System.dll"
            };

            Result result = compiler.compile(input);

            Assert.AreEqual(1, result.Errors.Count);
        }

        [TestMethod]
        public void Compile_TestedClassesWithOneTestedMethod_Success()
        {
            Input input = new Input()
            {
                Code = @" using System;
                                        
                    [TestedClasses(Name = ""Account"", Description = ""Account class"")]
                    public class Account
                    {
                        public double Current { get; private set; }

                        [TestedMethod(Name = ""add"", 
                            Description =@""add amount to your account, can not by less then 0 and more then 100, 
                                            in such cases ArgumentException should be thrown"")]
                        public void add(double amount)
                        {
                            Current += amount;
                        }   
                    }",
                Language = "CSharp",
                Name = "Test",
                ReferencedAssemblies = "System.dll"
            };

            Result result = compiler.compile(input);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.AreEqual(1, result.TestedClasses.Count);
            Assert.AreEqual("Account", result.TestedClasses[0].Name);
            Assert.AreEqual(1, result.TestedClasses[0].Methods.Count);
            Assert.AreEqual("add", result.TestedClasses[0].Methods[0].Name);
            Assert.AreEqual(0, result.TestedClasses[0].Methods[0].Tests.Count);
        }


        [TestMethod]
        public void Compile_TestedClassesWithThreeTests_Success()
        {
            Input input = new Input()
            {
                Code = @" using System;
                                        
                    [TestedClasses(Name = ""Account"", Description = ""Account class"")]
                    public class Account
                    {
                        public double Current { get; private set; }

                        [TestedMethod(Name = ""add"", 
                            Description =@""add amount to your account, can not by less then 0 and more then 100, 
                                            in such cases ArgumentException should be thrown"")]
                        [Test(Name = ""Less_Then_Zero"", 
                                Description = @""Should check if amount is les then 0, ArgumentException is thrown"",
                                Fail = true)]
                        [Test(Name = ""More_Then_Hundred"", 
                                Description = @""Should check if amount is more then 100, ArgumentException is thrown"",
                                Fail = true)]
                        [Test(Name = ""Add_To_Account"", 
                                Description = @""Should check if amount between 0 to 100 is proper added"",
                                Fail = false)]
                        public void add(double amount)
                        {
                            Current += amount;
                        }   
                    }",
                Language = "CSharp",
                Name = "Test",
                ReferencedAssemblies = "System.dll"
            };

            Result result = compiler.compile(input);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.AreEqual(1, result.TestedClasses.Count);
            Assert.AreEqual("Account", result.TestedClasses[0].Name);
            Assert.AreEqual(1, result.TestedClasses[0].Methods.Count);
            Assert.AreEqual("add", result.TestedClasses[0].Methods[0].Name);
            Assert.AreEqual(3, result.TestedClasses[0].Methods[0].Tests.Count);
            Assert.AreEqual("Less_Then_Zero", result.TestedClasses[0].Methods[0].Tests[0].Name);
            Assert.AreEqual("More_Then_Hundred", result.TestedClasses[0].Methods[0].Tests[1].Name);
            Assert.AreEqual("Add_To_Account", result.TestedClasses[0].Methods[0].Tests[2].Name);
        }

    }
}
