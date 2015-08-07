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
    public class SourceCodesController : Controller
    {
        private ISetupRepository setupRepository;

        public SourceCodesController(ISetupRepository setupRepositor)
        {
            setupRepository = setupRepositor;
        }

        public ActionResult Index()
        {
            return View(setupRepository.SourceCodes);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SourceCode sourceCode = setupRepository.GetSourceCode(id);
            if (sourceCode == null)
            {
                return HttpNotFound();
            }
            return View(sourceCode);
        }

        // GET: SourceCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Code,Language,ReferencedAssemblies")] SourceCode sourceCode)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    setupRepository.SaveSourceCode(sourceCode);
                    TempData["savemessage"] = string.Format("{0} has been saved", sourceCode.Name);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["errormessage"] = string.Format("An error occurred when creating when saving {0}", sourceCode.Name);
                    return RedirectToAction("Index");
                }
            }

            return View(sourceCode);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SourceCode sourceCode = setupRepository.GetSourceCode(id);
            if (sourceCode == null)
            {
                return HttpNotFound();
            }
           
            return View(sourceCode);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SourceCode sourceCode = setupRepository.GetSourceCode(id);
            if (sourceCode == null)
            {
                return HttpNotFound();
            }
            if (TryUpdateModel(sourceCode, "", new string[] { "Name", "Code", "Language", "ReferencedAssemblies" }))
            {
                try
                {
                    setupRepository.SaveSourceCode(sourceCode);
                    TempData["savemessage"] = string.Format("{0} has been edited", sourceCode.Name);
                    return RedirectToAction("Index");
                }
                catch( Exception ex)
                {
                    TempData["errormessage"] = string.Format("An error occurred when updating {0}", sourceCode.Name);
                }
            }
            return View(sourceCode);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SourceCode sourceCode = setupRepository.GetSourceCode(id);
            if (sourceCode == null)
            {
               return HttpNotFound();
            }
            return View(sourceCode);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                setupRepository.DeleteSourceCode(id);
                TempData["savemessage"] = string.Format("SourceCoude has been deleted");
            }
            catch (Exception ex)
            {
                TempData["errormessage"] = string.Format("An error occurred when deleting");
            } 
            
            return RedirectToAction("Index");
        }
    }
}
