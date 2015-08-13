using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Testility.Domain.Abstract;
using Testility.Domain.Concrete;
using Testility.Domain.Entities;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class TestedMethodsController : Controller
    {
        private ISetupRepository setupRepository;

        public TestedMethodsController(ISetupRepository setupRepositor)
        {
            setupRepository = setupRepositor;
        }

        public ActionResult Edit(int? id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,TestedClassId")] TestedMethod testedMethod)
        {
            return View();
        }

        public ActionResult Delete(int? id)
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction("Index", "SourceCodes");
        }
    }
}
