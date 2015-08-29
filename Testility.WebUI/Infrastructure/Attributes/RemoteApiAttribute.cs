using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Testility.WebUI.Infrastructure.Attributes
{
    public class RemoteApiAttribute : RemoteAttribute
    {
        public RemoteApiAttribute(string action, string controller, string area) : base(action, controller, area)
        {
            this.RouteData["area"] = area;
        }
    }
}