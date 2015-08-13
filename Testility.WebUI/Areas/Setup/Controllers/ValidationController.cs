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
        private ISetupRepository setupRepository;

        public ValidationController(ISetupRepository setupRepositor)
        {
            setupRepository = setupRepositor;
        }

        public JsonResult IsNameUnique([Bind(Prefix = "SourceCode.Name")]string name, [Bind(Prefix = "SourceCode.Id")]int id)
        {
            if (id != 0)
               return Json(true, JsonRequestBehavior.AllowGet);
            return setupRepository.CheckSourceCodeNameIsUnique(name) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}