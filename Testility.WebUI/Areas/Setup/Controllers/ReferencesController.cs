using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Testility.Domain.Abstract;
using Testility.WebUI.Areas.Setup.Model;
using AutoMapper;
using Testility.Domain.Entities;
using System.Net;
using AutoMapper.QueryableExtensions;
using Testility.WebUI.Model;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class ReferencesController : Controller
    {
        private readonly ISetupRepository setupRepository;
        public int PageSize { get; set; }

        public ReferencesController(ISetupRepository setupRepository)
        {
            this.setupRepository = setupRepository;
            PageSize = 3;
        }

        public ActionResult List(int page = 1)
        {
            IndexViewModel<ReferencesViewModel> data = new IndexViewModel<ReferencesViewModel>()
            {
                List = setupRepository.GetReferences()
                    .OrderBy(p => p.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList()
                    .Select(r => Mapper.Map<Reference, ReferencesViewModel>(r)),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = setupRepository.GetSolutions().Count()
                }
            };
            return View(data);
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
                try {
                    Reference reference = Mapper.Map<Reference>(model);
                    setupRepository.Save(reference);
                    TempData["savemessage"] = string.Format("{0} has been added", model.Name);
                    return RedirectToAction("List");
                }
                catch(Exception /*ex*/)
                {
                    return View("Reference", model);
                }
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
                setupRepository.DeleteReference(model.Id);
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