using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Testility.WebUI.Areas.Authorization.Models;
using Testility.WebUI.Services.Abstract;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;

namespace Testility.WebUI.Areas.Authorization.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {

        private readonly IIdentityServices identityServices;


        public AuthController(IIdentityServices identityService)
        {
            this.identityServices = identityService;
        }

        public ActionResult LogIn(string returnUrl)
        {
            var model = new LoginVM() { ReturnUrl = returnUrl };
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> LogIn(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await identityServices.GetUserAsync(identityServices.GetUserName(model.Email), model.Password);
                if (user != null)
                {
                    var identity = await identityServices.CreateIdentityAsync(user, "ApplicationCookie");
                    SignInToSite(identity);

                    return Redirect(GetRedirectUrl(model.ReturnUrl));
                }
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }


        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("List", "Solution", new { area = "Setup" });
            }

            return returnUrl;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser newUser = Mapper.Map<IdentityUser>(model);
                if (model.Id == null)                                               //New one
                {
                    try
                    {
                        var result = await identityServices.CreateAsync(newUser, model.Password);
                        if (result.Succeeded)
                        {
                            await identityServices.SendConfirmationEMail(newUser.Id);
                            var identity = await identityServices.CreateIdentityAsync(newUser, "ApplicationCookie");
                            SignInToSite(identity);
                            return RedirectToAction("List", "Solution", new { area = "Setup" });
                        }
                        result.Errors.ToList().ForEach(a => ModelState.AddModelError("", a));

                        return View();
                    }
                    catch (Exception e)
                    {

                    }
                }
                IdentityUser orgUser = identityServices.GetUser(User.Identity.GetUserId());
                if (orgUser != null) //Existing one
                {
                    Mapper.Map(model, orgUser);
                    identityServices.UpdateUserData(orgUser);
                    return RedirectToAction("List", "Solution", new { area = "Setup" });
                }
            }
            ModelState.AddModelError("", "Error when reqistering a user");
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }    


        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("LogIn");
        }


        public ActionResult Manage()
        {
            IdentityUser user = identityServices.GetUser(User.Identity.GetUserId());
            RegisterVM model = Mapper.Map<RegisterVM>(user);
            return View("Register", model);
        }


        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Auth", new { ReturnUrl = GetRedirectUrl(returnUrl) }));
        }

        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await Request.GetOwinContext().Authentication.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("LogIn");
            }

            var result = await identityServices.ExternalSignInAsync(loginInfo);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    //return View("Lockout");
                case SignInStatus.RequiresVerification:
                    //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                    //return (returnUrl);
                    default:
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationVM { Email = loginInfo.Email, Name= loginInfo.DefaultUserName });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationVM model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                var info = await Request.GetOwinContext().Authentication.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                IdentityUser newUser = Mapper.Map<IdentityUser>(model);
                var result = await identityServices.CreateUserWithNoPassword(newUser);
                if (result.Succeeded)
                {
                    result = await identityServices.AssociateLoginWithUser(newUser.Id, info);
                    if (result.Succeeded)
                    {
                        await identityServices.SignInAsync(newUser);
                        return RedirectToLocal(returnUrl);
                    }
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        private void SignInToSite(ClaimsIdentity identity)
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(identity);
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary["XsrfId"] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    };
}