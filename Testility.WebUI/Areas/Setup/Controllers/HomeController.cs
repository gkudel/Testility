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

        public HomeController(ISetupRepository repository)
        {
            this.repository = repository;
        }

        // GET: Setup/Home
        public ActionResult Index()
        {           
            return View(repository.TestedClasses);
        }
    }
}