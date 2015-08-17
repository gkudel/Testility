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
using Testility.WebUI.Areas.Setup.Models;
using Testility.WebUI.Model;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class SolutionController : Controller
    {
        private ISetupRepository setupRepository;
        private ICompilerService compilerService;
        public int PageSize { get; set; }

        public SolutionController(ISetupRepository setupRepository, ICompilerService compilerService)
        {
            this.setupRepository = setupRepository;
            this.compilerService = compilerService;
            PageSize = 3;
        }

        public ActionResult List(int? selecttedSolution, int page = 1)
        {
            ViewBag.SelecttedSolution = selecttedSolution;
            ProcjetsIndexData data = new ProcjetsIndexData()
            {
                Solutions = setupRepository.GetSolutions()                    
                    .OrderBy(p => p.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
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
            Solution s = new Solution()
            {
                ReferencedAssemblies = "System.dll",
                Items = new HashSet<Item>()
                {
                    new Item()
                }
            };            
            return View("Solution", s);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solution solution = setupRepository.GetSolution(id.Value);
            if (solution == null)
            {
                return HttpNotFound();
            }
            return View("Solution", solution);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "Id, Name, Language, ReferencedAssemblies, Items")]Solution model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (model.Id > 0)
            {
                Solution solution = setupRepository.GetSolution(model.Id);
                if (solution == null)
                {
                    return HttpNotFound();
                }
                model = Mapper.Map(model, solution);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    compilerService.compile(model);
                    setupRepository.Save(model);
                    TempData["savemessage"] = string.Format("{0} has been edited", model.Name);
                    return RedirectToAction("List");
                }
                catch (Exception  ex  )
                {
                    ModelState.AddModelError(String.Empty, string.Format("An error occurred when updating {0}", model.Name));
                    return View("Solution", model);
                }
            }
            else
            {
                return View("Solution", model);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solution solution = setupRepository.GetSolution(id.Value);
            if (solution == null)
            {
               return HttpNotFound();
            }
            return View(solution);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                setupRepository.Delete(id);
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
