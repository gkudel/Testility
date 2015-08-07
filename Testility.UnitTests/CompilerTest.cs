using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testility.Engine.Concrete;
using Testility.Engine.Model;

namespace Testility.UnitTests
{
    [TestClass]
    public class CompilerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Compile_WhenCalledWithOutSourcCode_ThrowException()
        {
            Input input = new Input();
            Compiler compiler = new Compiler();

            compiler.compile(input);
        }
    }
}
