using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Testility.WebUI.Areas.Authorization.Models;
using Testility.WebUI.Services.Abstract;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using Testility.WebUI.Infrastructure.ExternalLogin;

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
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LogIn(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await identityServices.GetUserAsync(identityServices.GetUserName(model.Email), model.Password);
                if (user != null)
                {

                    if (!await identityServices.IsEmailConfirmed(user.Id))
                    {
                        ModelState.AddModelError("", "You must have a confirmed email to log on.");
                        return View();
                    }

                    await identityServices.GenerateTokenToLogin(user.Id);
                    identityServices.SetTwoFactorAuthCookie(user.Id);
                    return RedirectToAction("VerifyCode");
                }
            }
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        public async Task<ActionResult> VerifyCode()
        {
            if (String.IsNullOrEmpty(await identityServices.GetTwoFactorUserIdAsync()))
            {
                return RedirectToAction("LogIn");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeVM model)
        {
            if (ModelState.IsValid)
            {
                string userId = await identityServices.GetTwoFactorUserIdAsync();
                if (userId == null || String.IsNullOrEmpty(model.token))
                {

                    return View("Error");
                }
                var user = await identityServices.GetUserById(userId);

                if (await identityServices.VerifyTokenToLogin(user.Id, model.token))
                {

                    var identity = await identityServices.CreateIdentityAsync(user, "ApplicationCookie");
                    await identityServices.SignInAsync(user);
                    return RedirectToAction("List", "Solution", new { area = "Setup" });
                }
                else
                {
                    ModelState.AddModelError("", "Invalid code");
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                            await identityServices.SetTwoFactorEnabledProtection(newUser.Id);
                            string token = await identityServices.GenerateEmailToken(newUser.Id);
                            await identityServices.SendConfirmationEMail(Url.Action("ConfirmEmail", "Auth", new { userId = newUser.Id, code = token }, protocol: Request.Url.Scheme), newUser.Id);
                            TempData["EmailToken"] = string.Format("Email Confirmation has been send");
                            return View();
                        }
                        result.Errors.ToList().ForEach(a => ModelState.AddModelError("", a));
                        return View();
                    }
                    catch (Exception /*e */)
                    {
                        ModelState.AddModelError("", "Error when reqistering a user");
                        return View();
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

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");       //ToDo
            }

            IdentityResult result = await identityServices.confirmEmail(userId, code);

            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                //AddErrors(result);        //ToDo
                return View();
            }
        }


        public ActionResult LogOut()
        {
            identityServices.SignOut();
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
            return new ExternalLogin(provider, Url.Action("ExternalLoginCallback", "Auth", new { ReturnUrl = returnUrl }));
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
                    IdentityUser existingUser = await identityServices.FindUserByName(loginInfo.DefaultUserName);
                    await identityServices.GenerateTokenToLogin(existingUser.Id); 
                    identityServices.SetTwoFactorAuthCookie(existingUser.Id);
                    return RedirectToAction("VerifyCode");
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
                    var res = await identityServices.AssociateLoginWithUser(newUser.Id, info);
                    if (res.Succeeded)
                    {
                        await identityServices.GenerateTokenToLogin(newUser.Id);
                        identityServices.SetTwoFactorAuthCookie(newUser.Id);
                        return RedirectToAction("VerifyCode");
                    }
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            IdentityUser user = identityServices.GetUserByEmail(model.Email);
            if (user != null)
            {
                var token = await identityServices.GeneratePasswordResetToken(user.Id);
                await identityServices.SendResetPasswordMail(Url.Action("ConfirmPasswordResset", "Auth", new { passToken = token , userId = user.Id}, protocol: Request.Url.Scheme), user.Id);

                TempData["PasswordConfirmation"] = string.Format("Password reset email has been send");
                return View();  
            }
            ModelState.AddModelError("", "There is no such user");
            return View();
        }



        [HttpGet]
        public ActionResult ConfirmPasswordResset(string passToken, string userId)
        {
            if (passToken == null || userId == null)
            {
                return View("Error");      
            }

            ConfirmPasswordResetVM model = new ConfirmPasswordResetVM() { Token = passToken, Id = userId };
            return View("ConfirmNewPassword", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmPasswordResset(ConfirmPasswordResetVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();        
            }
            IdentityResult result = await identityServices.ResetPassword(model.Id, model.Token, model.NewPassword);

            if (result.Succeeded)
                return RedirectToAction("LogIn");

            result.Errors.ToList().ForEach(x => ModelState.AddModelError("", x));
            return View();          
         }



    };
}