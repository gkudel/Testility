using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity;
using System;

[assembly: OwinStartup(typeof(Testility.WebUI.App_Start.Startup))]

namespace Testility.WebUI.App_Start
{
    public class Startup
    {
        internal static IDataProtectionProvider dataProtectionProvider { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            dataProtectionProvider = app.GetDataProtectionProvider();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Authorization/Auth/LogIn")
            });
           
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseFacebookAuthentication(
               appId: "964715893591610",
               appSecret: "6499cb17a6dfc09abdad757e4e760385");

        }
    }
}
