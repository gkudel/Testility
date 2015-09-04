using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ViewBag.Title = "UnitTest Entry";
            return View("Solution");
        }
    }
}