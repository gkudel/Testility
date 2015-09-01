using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;

namespace Testility.WebUI.Areas.WebApi.Controllers
{
    public class UnitTestController : ApiController
    {
        private readonly ISetupRepository setupRepository;

        public UnitTestController(ISetupRepository setupRepository)
        {
            this.setupRepository = setupRepository;
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
            SolutionApi r = Mapper.Map<SolutionApi>(s);
            return Request.CreateResponse<SolutionApi>(HttpStatusCode.OK, r);
        }
    }
}