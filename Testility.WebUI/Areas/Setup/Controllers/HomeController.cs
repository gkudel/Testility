using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Testility.Domain.Abstract;

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
            return View(repository.Files);
        }
    }
}