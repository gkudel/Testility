using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Testility.Engine.Abstract;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class HomeController : Controller
    {
        private ISetupRepository repository;
        private ICompiler compiler;

        public HomeController(ISetupRepository repository, ICompiler compiler)
        {
            this.repository = repository;
            this.compiler = compiler;
        }

        // GET: Setup/Home
        public ActionResult Index()
        {
            SourceCode code = new SourceCode()
            {
                Code = @"      
                    using System;                                   

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
                Language = Language.CSharp,
                Name = "Account",
                ReferencedAssemblies = "System.dll"
            };
            compiler.compile(code);
            return View(repository.SourceCodes);
        }
    }
}