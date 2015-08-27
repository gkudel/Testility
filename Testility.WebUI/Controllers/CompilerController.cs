using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.Engine.Model;
using Testility.WebUI.Model;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Controllers
{
    public class CompilerController : ApiController
    {
        private readonly ISetupRepository setupRepository;
        private readonly ICompilerService compilerService;

        public CompilerController(ISetupRepository setupRepository, ICompilerService compilerService)
        {
            this.setupRepository = setupRepository;
            this.compilerService = compilerService;
        }

        public HttpResponseMessage Post(SolutionViewModel solution)
        {
            if (solution == null) return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Solution can't be null");
            IList<Error> errors = compilerService.Compile(Mapper.Map<Solution>(solution), solution.References);
            if (errors.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new[] {
                    new { Message = "Succes!!!", Alert = "success" }
                });
            }
            else
            {
                List<object> ret = new List<object>();
                foreach (Error e in errors)
                {
                    ret.Add(new { Message = e.ToString(), Alert = e.IsWarning ? "warning" : "danger" });
                }
                return Request.CreateResponse(HttpStatusCode.OK, ret.ToArray());
            }
        }
    }
}
