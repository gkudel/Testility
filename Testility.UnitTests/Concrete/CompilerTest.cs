using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testility.Engine.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Engine.Model;
using Testility.Engine.Abstract;
using Testility.Egine.Concrete;

namespace Testility.Engine.Concrete.Tests
{
    [TestClass()]
    public class CompilerTest
    {
        public ICompiler compiler;

        [TestInitialize]
        public void Int()
        {
            compiler = new CompilerProxy();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Compile_WithOutSourceCode_ArgumentException()
        {
            Input input = new Input();
            compiler.Compile(input);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Compile_WithUnknowLanguage_ArgumentException()
        {
            Input input = new Input()
            {
                Code = new string[] { "public class Test{}" },
                Language = "Pascal"
            };
            compiler.Compile(input);
        }

        [TestMethod]
        public void Compile_Improper_CompilingError()
        {
            Input input = new Input()
            {
                Code = new string[] { "using System public class Test{}" },
                Language = "CSharp",
                SolutionName = "Test",
                ReferencedAssemblies = new string[] { "System.dll" }
            };

            Result result = compiler.Compile(input);

            Assert.AreEqual(1, result.Errors.Count);
        }

        [TestMethod]
        public void Compile_ProperClass_Compiled()
        {
            Input input = new Input()
            {
                Code = new string[] { "using System; public class Test{}" },
                Language = "CSharp",
                SolutionName = "Test",
                ReferencedAssemblies = new string [] { "System.dll" }
            };

            Result result = compiler.Compile(input);

            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void Compile_TestedClassesWithOneTestedMethod_Success()
        {
            Input input = new Input()
            {
                Code = new string[] { @" using System;
                                        
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
                    }" },
                Language = "CSharp",
                SolutionName = "Test",
                ReferencedAssemblies = new string[] { "System.dll" }
            };

            Result result = compiler.Compile(input);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.AreEqual(1, result.Classes.Count);
            Assert.AreEqual("Account", result.Classes.First().Name);
            Assert.AreEqual(1, result.Classes.First().Methods.Count);
            Assert.AreEqual("add", result.Classes.First().Methods.First().Name);
            Assert.AreEqual(0, result.Classes.First().Methods.First().Tests.Count);
        }


        [TestMethod]
        public void Compile_TestedClassesWithThreeTests_Success()
        {
            Input input = new Input()
            {
                Code = new string[] { @" using System;
                                        
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
                    }" },
                Language = "CSharp",
                SolutionName = "Test",
                ReferencedAssemblies = new string[] { "System.dll" }
            };

            Result result = compiler.Compile(input);

            Assert.AreEqual(0, result.Errors.Count);
            Assert.AreEqual(1, result.Classes.Count);
            Assert.AreEqual("Account", result.Classes.First().Name);
            Assert.AreEqual(1, result.Classes.First().Methods.Count);
            Assert.AreEqual("add", result.Classes.First().Methods.First().Name);
            Assert.AreEqual(3, result.Classes.First().Methods.First().Tests.Count);
            Assert.AreEqual("Less_Then_Zero", result.Classes.First().Methods.First().Tests.First().Name);
            Assert.AreEqual("More_Then_Hundred", result.Classes.First().Methods.First().Tests.Skip(1).Take(1).First().Name);
            Assert.AreEqual("Add_To_Account", result.Classes.First().Methods.First().Tests.Skip(2).Take(1).First().Name);
        }
    }
}