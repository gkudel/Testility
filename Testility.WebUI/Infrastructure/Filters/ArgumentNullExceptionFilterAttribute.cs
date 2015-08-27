using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Testility.WebUI.Infrastructure.Filters
{
    public class ArgumentNullExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ArgumentNullException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "ArgumentNullException occurred");
            }
        }
    }
}