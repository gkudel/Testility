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
using Testility.WebUI.Infrastructure.Filters;
using Testility.WebUI.Model;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Areas.WebApi.Controllers
{
    public class UnitTestController : ApiController
    {
        private readonly ISetupRepository setupRepository;
        private readonly ITestGenarator testGenerator;
        private readonly ICompilerService compilerService;

        public UnitTestController(ISetupRepository setupRepository, ITestGenarator testGenerator, ICompilerService compilerService)
        {
            this.setupRepository = setupRepository;
            this.testGenerator = testGenerator;
            this.compilerService = compilerService;
        }

        [HttpPost]
        [Route("api/UnitTest/Create/{solutionId}")]
        public HttpResponseMessage Create(int solutionId)
        {
            SolutionApi s = setupRepository.GetSolution(solutionId);
            if (s == null)
            {
                var message = string.Format("Solution with id = {0} not found", solutionId);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }        
            SolutionViewModel ret = Mapper.Map<SolutionViewModel>(s);            
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


        [ArgumentNullExceptionFilter]
        [HttpPost]
        [Route("api/UnitTest/Compile")]
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
    }
}