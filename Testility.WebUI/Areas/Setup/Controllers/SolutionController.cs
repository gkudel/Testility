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
using AutoMapper.QueryableExtensions;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class SolutionController : Controller
    {
        private readonly ISetupRepository setupRepository;
        private readonly ICompilerService compilerService;
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
            SolutionIndexVM data = new SolutionIndexVM()
            {
                List = setupRepository.GetSolutions(false)
                    .OrderBy(p => p.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList()
                    .Select(s => Mapper.Map<Solution, SolutionIndexItemVM>(s)),
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
                Items = new HashSet<Item>()
                {
                    new Item()
                }
            };            
            return View("Solution", Mapper.Map<SolutionVM>(s));
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
            return View("Solution", Mapper.Map<SolutionVM>(solution));
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(SolutionVM model)
        {
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Solution solution = new Solution();
            if (ModelState.IsValid)
            {
                if (model?.Id > 0)
                {
                    solution = setupRepository.GetSolution(model.Id);
                    if (solution == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);                       
                }
              
                Mapper.Map(model, solution);

                try
                {
                    IList<Error> errors = compilerService.Compile(solution, model.Refrences);
                    if (errors.Count == 0)
                    {
                        setupRepository.Save(solution, model.Refrences);
                        TempData["savemessage"] = string.Format("{0} has been edited", model.Name);
                        return RedirectToAction("List");
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, string.Format("An error occurred when updating {0}", solution.Name));
                        return View("Solution", model);
                    }
                }
                catch (Exception /* ex */ )
                {
                    ModelState.AddModelError(String.Empty, string.Format("An error occurred when updating {0}", solution.Name));
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
            return View(Mapper.Map<SolutionVM>(solution));
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
