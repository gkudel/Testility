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
        private IUnitTestRepository unitTestRepository;
        public int PageSize { get; set; }

        public UnitTestController(IUnitTestRepository unitTestRepository)
        {
            this.unitTestRepository = unitTestRepository;
            PageSize = 3;
        }

        // GET: UnitTest
        public ViewResult List(int page = 1)
        {
            UnitTestIndexVM data = new UnitTestIndexVM()
            {
                List = unitTestRepository.GetSolutions()
                        .OrderBy(p => p.Id)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize)
                        .ToList()
                        .Select(s => Mapper.Map<UnitTestSolution, UnitTestIndexItemVM>(s)),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = unitTestRepository.GetSolutions().Count()
                },
                BrowserConfig = new BrowserConfig() {  Name = "Solutions" }
            };
            return View(data);
        }

        public ActionResult Create(int? solutionId)
        {
            return View();
        }
    }
}