using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Testility.Domain.Abstract;

namespace Testility.WebUI.Controllers
{
    public class UnitTestController : Controller
    {
        private IUnitTestRepository unitTestRepository;
        public int PageSize { get; set; }

        public UnitTestController(IUnitTestRepository unitTestRepository)
        {
            this.unitTestRepository = unitTestRepository;
            PageSize = 3;
        }
        // GET: UnitTest
        public ActionResult List()
        {
            return View();
        }
    }
}