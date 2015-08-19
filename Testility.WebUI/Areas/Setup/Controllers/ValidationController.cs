using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Testility.Domain.Abstract;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class ValidationController : Controller
    {
        private readonly ISetupRepository setupRepository;

        public ValidationController(ISetupRepository setupRepositor)
        {
            setupRepository = setupRepositor;
        }

        public JsonResult IsNameUnique(string name, int? id)
        {
            return Json(!setupRepository.IsAlreadyDefined(name, id), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                setupRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}