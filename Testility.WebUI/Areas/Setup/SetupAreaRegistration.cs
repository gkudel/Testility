using System.Web.Mvc;

namespace Testility.WebUI.Areas.Setup
{
    public class SetupAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Setup";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Setup_default",
                "Setup/{controller}/{action}/{id}",
                new { action = "List", controller = "Solution", id = UrlParameter.Optional }
            );
        }
    }
}