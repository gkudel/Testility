﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.WebUI.Model;

namespace Testility.WebUI.Controllers
{
    public class UnitTestController : Controller
    {
        private readonly IDbRepository dbRepository;
        public int PageSize { get; set; }

        public UnitTestController(IDbRepository unitTestRepository)
        {
            this.dbRepository = unitTestRepository;
            PageSize = 3;
        }

        // GET: UnitTest
        public ViewResult List(int page = 1)
        {
            IndexViewModel<UnitTestIndexItemViewModel> data = new IndexViewModel<UnitTestIndexItemViewModel>()
            {
                List = dbRepository.GetUnitTestSolutions()
                        .OrderBy(p => p.Id)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize)
                        .ToList()
                        .Select(s => Mapper.Map<UnitTestSolution, UnitTestIndexItemViewModel>(s)),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = dbRepository.GetUnitTestSolutions().Count()
                }
            };
            return View(data);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Unit Test";
            return View("Solution");
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Unit Test";            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitTestSolution solution = dbRepository.GetUnitTestSolution(id.Value);
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
            UnitTestSolution solution = dbRepository.GetUnitTestSolution(id.Value);
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
                dbRepository.DeleteUnitSolution(id);
                TempData["savemessage"] = string.Format("Solution has been deleted");
            }
            catch (Exception /*ex*/ )
            {
                ModelState.AddModelError(string.Empty, string.Format("An error occurred when deleting"));
            }

            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}