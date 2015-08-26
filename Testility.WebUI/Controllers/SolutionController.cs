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
            return Request.CreateResponse<IEnumerable<SolutionApi>>(HttpStatusCode.OK, setupRepository.GetSolutions().Project().To<SolutionApi>().ToArray());
        }

        public HttpResponseMessage Get(int id)
        {
            Solution s = setupRepository.GetSolution(id);
            if(s == null)
            {
                var message = string.Format("Solution with id = {0} not found", id);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
            return Request.CreateResponse<SolutionApi>(HttpStatusCode.OK, Mapper.Map<SolutionApi>(s));
        }

        [HttpPost]
        public HttpResponseMessage Compile(SolutionApi solution)
        {
            if (solution == null) return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Solution can't be null");
            IList<Error> errors = compilerService.Compile(Mapper.Map<Solution>(solution), solution.References);
            if (errors.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new[] {
                    new { Message = "Succes", Alert = "success" }
                });
            }
            else
            {
                List<object> ret = new List<object>();
                foreach (Error e in errors)
                {
                    ret.Add(new { Message = e.Message, Alert = "danger" });
                }
                return Request.CreateResponse(HttpStatusCode.OK, ret.ToArray());
            }
        }
    }
}
