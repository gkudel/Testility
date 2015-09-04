using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Testility.Domain.Abstract;
using AutoMapper.QueryableExtensions;
using Testility.WebUI.Model;

namespace Testility.WebUI.Areas.WebApi
{
    public class ReferenceController : ApiController
    {
        private readonly IDbRepository setupRepository;

        public ReferenceController(IDbRepository setupRepository)
        {
            this.setupRepository = setupRepository;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse<IEnumerable<ReferenceViewModel>>(HttpStatusCode.OK, setupRepository.GetReferences().Project().To<ReferenceViewModel>().ToArray());
        }
    }
}