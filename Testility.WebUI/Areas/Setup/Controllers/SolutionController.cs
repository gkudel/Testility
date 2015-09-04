using System;
using System.Data.Entity.Validation;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.Engine.Abstract;
using Testility.Engine.Model;
using System.Collections.Generic;
using System.Linq;
using Testility.WebUI.Areas.Setup.Model;
using Testility.WebUI.Model;
using Testility.WebUI.Services.Abstract;
using AutoMapper.QueryableExtensions;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class SolutionController : Controller
    {
        private readonly IDbRepository setupRepository;
        public int PageSize { get; set; }

        public SolutionController(IDbRepository setupRepository)
        {
            this.setupRepository = setupRepository;
            PageSize = 3;
        }

        public ViewResult List(int? selecttedSolution, int page = 1)
        {
            ViewBag.SelecttedSolution = selecttedSolution;
            IndexViewModel<SolutionIndexItemViewModel> data = new IndexViewModel<SolutionIndexItemViewModel>()
            {
                List = setupRepository.GetSetupSolutions(false)
                    .OrderBy(p => p.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList()
                    .Select(s => Mapper.Map<SetupSolution, SolutionIndexItemViewModel>(s)),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = setupRepository.GetSetupSolutions().Count()
                }
            };

            return View(data);
        }

        public ActionResult Create()
        {
            ViewBag.Menu = "~/Areas/Setup/Views/Shared/_Menu.cshtml";
            ViewBag.Title = "Solution Explorer";
            return View("Solution");
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Menu = "~/Areas/Setup/Views/Shared/_Menu.cshtml";
            ViewBag.Title = "Solution Explorer";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SetupSolution solution = setupRepository.GetSetupSolution(id.Value);
            if (solution == null)
            {
                return HttpNotFound();
            }
            return View("Solution");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SetupSolution solution = setupRepository.GetSetupSolution(id.Value);
            if (solution == null)
            {
               return HttpNotFound();
            }
            return View(Mapper.Map<SolutionViewModel>(solution));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                setupRepository.DeleteSolution(id);
                TempData["savemessage"] = string.Format("Solution has been deleted");
            }
            catch (Exception /*ex*/ )
            {
                ModelState.AddModelError(String.Empty, string.Format("An error occurred when deleting"));
            } 
            
            return RedirectToAction("List");
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
