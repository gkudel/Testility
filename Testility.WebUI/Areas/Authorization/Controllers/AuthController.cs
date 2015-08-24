using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Collections.Generic;

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Testility.Domain.Concrete;
using Testility.WebUI.Areas.Authorization.Models;

namespace Testility.WebUI.Areas.Authorization.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {

        private readonly IdentityServices identityServices;


        public AuthController(IdentityServices identityService)
        {
            this.identityServices = identityService;
        }

        public ActionResult LogIn(string returnUrl)
        {
                var model = new LoginVM() { ReturnUrl = returnUrl};
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
                            identityServices.SendConfirmationEMail(User.Identity.GetUserId());
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


        private void SignInToSite(ClaimsIdentity identity)
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(identity);
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
    }
}