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
using Testility.WebUI.Services;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class SourceCodesController : Controller
    {
        private ISetupRepository setupRepository;
        private IFileRepository fileRepository;

        public SourceCodesController(ISetupRepository setupRepositor, IFileRepository fileRepositor)
        {
            setupRepository = setupRepositor;
            fileRepository = fileRepositor;
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
            TempData["header"] = string.Format("Create");
            TempData["action"] = "Create";
            return View("CreateAndEdit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Code,Language,ReferencedAssemblies")] SourceCode sourceCode, HttpPostedFileBase uploadedFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    setupRepository.SaveSourceCode(sourceCode);
                    fileRepository.Save(uploadedFile, x=>Server.MapPath(x));

                    TempData["savemessage"] = string.Format("{0} has been saved", sourceCode.Name);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["errormessage"] = string.Format("An error occurred when saving {0}", sourceCode.Name);
                    return RedirectToAction("Index");
                }
            }

            return View("CreateAndEdit", sourceCode);
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
            TempData["header"] = string.Format("Edit");
            TempData["action"] = "Edit";
            return View("CreateAndEdit", sourceCode);
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
            return View("CreateAndEdit", sourceCode);
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
