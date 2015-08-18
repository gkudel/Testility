using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Testility.Domain.Abstract;
using Testility.WebUI.Areas.Setup.Models;
using AutoMapper;
using Testility.Domain.Entities;
using System.Net;
using AutoMapper.QueryableExtensions;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class ReferencesController : Controller
    {

        private ISetupRepository setupRepository;

        public ReferencesController(ISetupRepository setupRepository)
        {
            this.setupRepository = setupRepository;
        }

        public ActionResult List()
        {
            IList<ReferencesViewModel> model = Mapper.Map<IList<ReferencesViewModel>>(setupRepository.GetReferences().Project().To< ReferencesViewModel>());
            return View(model);
        }

        public ActionResult GetListOfReferences(int id)
        {
            return Json(new ReferencesJsonVM(setupRepository.GetReferences().OrderBy(x => x.Name), setupRepository.GetReferenceForSolution(id)), JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            return View("Reference");
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference reference = setupRepository.GetReference(id.Value);
            if (reference == null)
            {
                return HttpNotFound();
            }

            ReferencesViewModel model = Mapper.Map<ReferencesViewModel>(reference);
            return View("Reference", model);
        }


        [HttpPost ActionName("Edit")]
        public ActionResult Edit(ReferencesViewModel model)
        {
            if (ModelState.IsValid)
            {
                Reference reference = Mapper.Map<Reference>(model);
                setupRepository.SaveReferences(reference);
                TempData["savemessage"] = string.Format("{0} has been added", model.Name);
                return RedirectToAction("List");
            }
            else
            {
                return View("Reference", model);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference reference = setupRepository.GetReference(id.Value);
            if (reference == null)
            {
                return HttpNotFound();
            }

            ReferencesViewModel model = Mapper.Map<ReferencesViewModel>(reference);
            return View(model);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ReferencesViewModel model)
        {
            try
            {
                setupRepository.DeleteReferences(model.Id);
                TempData["savemessage"] = string.Format("Solution has been deleted");
            }
            catch (Exception /*ex*/ )
            {
                ModelState.AddModelError(String.Empty, string.Format("An error occurred when deleting"));
            }

            return RedirectToAction("List");

        }

    }
}