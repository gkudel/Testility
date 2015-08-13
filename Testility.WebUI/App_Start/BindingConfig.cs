using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Testility.WebUI.Areas.Setup.Models;
using Testility.WebUI.Infrastructure.Binding;

namespace Testility.WebUI.App_Start
{
    public class BindingConfig
    {
        public static void RegisterBinding()
        {
            ModelBinders.Binders.Add(typeof(EditViewModel), new EditPostValueBinder());
        }
    }
}
