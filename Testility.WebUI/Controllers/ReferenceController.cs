using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Testility.Domain.Abstract;
using AutoMapper.QueryableExtensions;
using Testility.WebUI.Model;

namespace Testility.WebUI.Controllers
{
    public class ReferenceController : ApiController
    {
        private readonly ISetupRepository setupRepository;

        public ReferenceController(ISetupRepository setupRepository)
        {
            this.setupRepository = setupRepository;
        }

        public IEnumerable<ReferenceApi> Get()
        {
            return setupRepository.GetReferences().Project().To<ReferenceApi>().ToArray();
        }
    }
}