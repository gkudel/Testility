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

namespace Testility.WebUI.Controllers
{
    public class SolutionController : ApiController
    {
        private ISetupRepository setupRepository;

        public SolutionController(ISetupRepository setupRepository)
        {
            this.setupRepository = setupRepository;
        }

        public IEnumerable<SolutionApi> Get()
        {
            return setupRepository.GetSolutions().Project().To<SolutionApi>().ToArray();
        }
    }
}
