using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testility.Engine.Concrete;

namespace Testility.UnitTests
{
    [TestClass]
    public class CompilerTest
    {
        [TestMethod]
        public void Cannot_Compile_WithOut_SourCode()
        {
            Compiler compiler = new Compiler();
        }
    }
}
