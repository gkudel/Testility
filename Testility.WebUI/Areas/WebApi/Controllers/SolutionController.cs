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

namespace Testility.WebUI.Areas.WebApi.Controllers
{
    public class SolutionController : ApiController
    {
        private readonly ISetupRepository setupRepository;
        private readonly ICompilerService compilerService;

        public SolutionController(ISetupRepository setupRepository, ICompilerService compilerService)
        {
            this.setupRepository = setupRepository;
            this.compilerService = compilerService;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse<IEnumerable<SolutionIndexItemViewModel>>(HttpStatusCode.OK, setupRepository.GetSolutions().ToList().Select(s => Mapper.Map<SolutionApi, SolutionIndexItemViewModel>(s)).ToArray());
        }

        public HttpResponseMessage Get(int id)
        {
            SolutionApi s = setupRepository.GetSolution(id);
            if(s == null)
            {
                var message = string.Format("Solution with id = {0} not found", id);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
            return Request.CreateResponse<SolutionViewModel>(HttpStatusCode.OK, Mapper.Map<SolutionViewModel>(s));
        }

        [ArgumentNullExceptionFilter]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public HttpResponseMessage Post(SolutionViewModel model)
        {
            if (model == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Solution can't be null");
            SolutionApi solution = new SolutionApi();
            if (ModelState.IsValid)
            {
                if (model?.Id > 0)
                {
                    solution = setupRepository.GetSolution(model.Id);
                    if (solution == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Solution doesn't exist");
                }

                Mapper.Map(model, solution);
                List<object> ret = new List<object>();
                IList<Error> errors = compilerService.Compile(solution, model.References);
                if (errors.Count > 0)
                {
                    if(solution.Classes?.Count() > 0) solution.Classes.Clear();
                    foreach (Error e in errors)
                    {
                        ret.Add(new { Message = e.ToString(), Alert = e.IsWarning ? "warning" : "danger" });
                    }
                }
                setupRepository.Save(solution, model.References);
                return Request.CreateResponse(HttpStatusCode.OK, new {
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
        [HttpPost]
        [Route("api/Solution/Compile")]
        public HttpResponseMessage Compile(SolutionViewModel solution)
        {
            if (solution == null) return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Solution can't be null");
            IList<Error> errors = compilerService.Compile(Mapper.Map<SolutionApi>(solution), solution.References);
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
