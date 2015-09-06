using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.WebUI.Model;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Testility.WebUI.Services.Abstract;
using Testility.Engine.Model;
using System.Data.Entity.Validation;
using Testility.WebUI.Infrastructure.Filters;
using Testility.WebUI.Areas.Setup.Model;
using Ninject;

namespace Testility.WebUI.Areas.WebApi.Controllers
{
    public class SolutionController : ApiController
    {
        private readonly IDbRepository setupRepository;
        private readonly ICompilerService compilerService;

        public SolutionController(IDbRepository setupRepository, ICompilerService setupCompilerService)
        {
            this.setupRepository = setupRepository;
            this.compilerService = setupCompilerService;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse<IEnumerable<SolutionIndexItemViewModel>>(HttpStatusCode.OK, setupRepository.GetSetupSolutions().ToList().Select(s => Mapper.Map<Solution, SolutionIndexItemViewModel>(s)).ToArray());
        }

        public HttpResponseMessage Get(int id)
        {
            Solution s = setupRepository.GetSetupSolution(id);
            if(s == null)
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
            SetupSolution solution = new SetupSolution();
            if (ModelState.IsValid)
            {
                if (model?.Id > 0)
                {
                    solution = setupRepository.GetSetupSolution(model.Id);
                    if (solution == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Solution doesn't exist");
                }

                Mapper.Map(model, solution);
                List<object> ret = new List<object>();
                IList<Error> errors = compilerService.Compile(solution, model.References);
                if (errors.Count > 0)
                {
                    if (solution.Classes?.Count() > 0) solution.Classes.Clear();
                    foreach (Error e in errors)
                    {
                        ret.Add(new { Message = e.ToString(), Alert = e.IsWarning ? "warning" : "danger" });
                    }
                }
                setupRepository.SaveSetupSolution(solution, model.References);
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
        public HttpResponseMessage Put(int id, SolutionViewModel model)
        {
            return CreateOrUpdate(model);
        }

        [ArgumentNullExceptionFilter]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public HttpResponseMessage Post(SolutionViewModel model)
        {
            return CreateOrUpdate(model);
        }

        [ArgumentNullExceptionFilter]
        [HttpPost]
        [Route("api/Solution/Compile")]
        public HttpResponseMessage Compile(SolutionViewModel solution)
        {
            if (solution == null) return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Solution can't be null");
            IList<Error> errors = compilerService.Compile(Mapper.Map<SetupSolution>(solution), solution.References);
            List<object> ret = new List<object>();
            foreach (Error e in errors)
            {
                ret.Add(new { Message = e.ToString(), Alert = e.IsWarning ? "warning" : "danger" });
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret.ToArray());
        }

        [HttpPost]
        [Route("api/Solution/IsNameUnique")]
        public bool IsNameUnique(Info info)
        {
            return !setupRepository.IsAlreadyDefined(info.Name, info.Id);
        }

        public class Info
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
