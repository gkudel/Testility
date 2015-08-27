using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity;
using System;
using Microsoft.Owin.Security.Facebook;
using System.Threading.Tasks;
using System.Security.Claims;

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
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));


            var facebookOptions = new FacebookAuthenticationOptions()
            {
                AppId = "964715893591610",
                AppSecret = "6499cb17a6dfc09abdad757e4e760385"
            };

            facebookOptions.Scope.Add("email");

            app.UseFacebookAuthentication(facebookOptions);

        }
    }
}
