using AutoMapper;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.Engine.Model;
using Testility.WebUI.Infrastructure.Filters;
using Testility.WebUI.Model;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Areas.WebApi.Controllers
{
    public class UnitTestController : ApiController
    {
        private readonly IDbRepository dbRepository;
        private readonly ITestGenarator testGenerator;
        private readonly ICompilerService compilerService;

        public UnitTestController(IDbRepository setupRepository, ITestGenarator testGenerator, ICompilerService unitTestCompilerService)
        {
            this.dbRepository = setupRepository;
            this.testGenerator = testGenerator;
            this.compilerService = unitTestCompilerService;
        }

        [HttpGet]
        [Route("api/UnitTest/Init/{solutionId}")]
        public HttpResponseMessage Init(int solutionId)
        {
            SetupSolution s = dbRepository.GetSetupSolution(solutionId);
            if (s == null)
            {
                var message = string.Format("Solution with id = {0} not found", solutionId);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
            SolutionViewModel ret = Mapper.Map<SolutionViewModel>(s);
            ret.Name = "";
            ret.Id = 0;
            List<ItemViewModel> items = new List<ItemViewModel>();
            foreach (Domain.Entities.Class c in s.Classes)
            {
                ItemViewModel item = new ItemViewModel() { Id = 0, SolutionId = ret.Id, Name = c.Name + "Test.cs" };
                item.Code = testGenerator.Create(c, ret.Language);
                items.Add(item);
            }
            ret.Items = items.ToArray();
            return Request.CreateResponse<SolutionViewModel>(HttpStatusCode.OK, ret);
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        public HttpResponseMessage Get(int id)
        {
            UnitTestSolution s = dbRepository.GetUnitTestSolution(id);
            if (s == null)
            {
                var message = string.Format("Solution with id = {0} not found", id);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
            return Request.CreateResponse<SolutionViewModel>(HttpStatusCode.OK, Mapper.Map<SolutionViewModel>(s));
        }

        [NonAction]
        private HttpResponseMessage CreateOrUpdate(SolutionViewModel model)
        {
            if (model == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Solution can't be null");
            UnitTestSolution solution = new UnitTestSolution();
            if (ModelState.IsValid)
            {
                if (model?.Id > 0)
                {
                    solution = dbRepository.GetUnitTestSolution(model.Id);
                    if (solution == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Solution doesn't exist");
                }

                Mapper.Map(model, solution);
                List<object> ret = new List<object>();
                IList<Error> errors = compilerService.Compile(solution, model.References);
                if (errors.Count > 0)
                {
                    foreach (Error e in errors)
                    {
                        ret.Add(new { Message = e.ToString(), Alert = e.IsWarning ? "warning" : "danger" });
                    }
                }
                dbRepository.SaveUnitTestSolution(solution, model.References);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    compileErrors = ret.ToArray(),
                    solution = Mapper.Map<SolutionViewModel>(solution)
                });
            }
            else
            {
                List<object> ret = new List<object>();
                foreach (var m in ModelState.Values)
                {
                    foreach (var e in m.Errors)
                    {
                        ret.Add(new { Message = e.ErrorMessage, Alert = "danger" });
                    }
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, ret.ToArray());
            }
        }

        [ArgumentNullExceptionFilter]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public HttpResponseMessage Post(SolutionViewModel model)
        {
            return CreateOrUpdate(model);
        }

        [ArgumentNullExceptionFilter]
        [HttpPost]
        [Route("api/UnitTest/Compile")]
        public HttpResponseMessage Compile(SolutionViewModel solution)
        {
            if (solution == null) return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Solution can't be null");
            UnitTestSolution u = Mapper.Map<UnitTestSolution>(solution);
            if (u.SetupSolutionId > 0)
            {
                u.SetupSolution = dbRepository.GetSetupSolution(u.SetupSolutionId);
            }
            IList<Error> errors = compilerService.Compile(u, solution.References);
            if (errors.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                List<object> ret = new List<object>();
                foreach (Error e in errors)
                {
                    ret.Add(new { Message = e.ToString(), Alert = e.IsWarning ? "warning" : "danger" });
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, ret.ToArray());
            }
        }
    }
}