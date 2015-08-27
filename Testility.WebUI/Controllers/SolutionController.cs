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

namespace Testility.WebUI.Controllers
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
            return Request.CreateResponse<IEnumerable<SolutionViewModel>>(HttpStatusCode.OK, setupRepository.GetSolutions().Project().To<SolutionViewModel>().ToArray());
        }

        public HttpResponseMessage Get(int id)
        {
            Solution s = setupRepository.GetSolution(id);
            if(s == null)
            {
                var message = string.Format("Solution with id = {0} not found", id);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
            return Request.CreateResponse<SolutionViewModel>(HttpStatusCode.OK, Mapper.Map<SolutionViewModel>(s));
        }

        [System.Web.Mvc.ValidateAntiForgeryToken]
        public HttpResponseMessage Post(SolutionViewModel model)
        {
            if (model == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Solution can't be null");
            Solution solution = new Solution();
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
                ret.Insert(0, new { Message = "Saved!!!", Alert = "success" });
                return Request.CreateResponse(HttpStatusCode.OK, ret.ToArray());
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

    }
}
