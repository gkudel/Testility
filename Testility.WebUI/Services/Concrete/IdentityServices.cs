using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Testility.Domain.Concrete;
using Testility.WebUI.App_Start;
using Testility.WebUI.Services.Abstract;
using Microsoft.Owin.Security;

namespace Testility.WebUI.Services.Concrete
{
    public class IdentityServices : IIdentityServices, IDisposable
    {
        private readonly UserManager<IdentityUser> userMenager;
        private readonly UserStore<IdentityUser> userStore;
        private readonly EFDbContext db;
        private readonly SignInManager<IdentityUser, string> signInMenager;

        private readonly IAuthenticationManager authenticationManager;

        public IdentityServices(EFDbContext context, IAuthenticationManager authenticationMana)
        {
            db = context;
            authenticationManager = authenticationMana;
            userStore = new UserStore<IdentityUser>(context);
            userMenager = new UserManager<IdentityUser>(userStore);
            userMenager.UserValidator = new UserValidator<IdentityUser>(userMenager) { RequireUniqueEmail = true, AllowOnlyAlphanumericUserNames = false };
            userMenager.PasswordValidator = new PasswordValidator() { RequiredLength = 6, RequireLowercase = true, RequireUppercase = true, RequireDigit = true };

            signInMenager = new SignInManager<IdentityUser, string>(userMenager, authenticationManager);
            
            userMenager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<IdentityUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            //userMenager.EmailService = new EmailService();

            var dataProtectionProvider = Startup.dataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                IDataProtector dataProtector = dataProtectionProvider.Create("ASP.NET Identity");
                userMenager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(dataProtector);
            }
        }
        public Task<IdentityUser> GetUserAsync(string name, string password)
        {
            return userMenager.FindAsync(name, password);

        }

        public Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string type)
        {
            return userMenager.CreateIdentityAsync(user, type);
        }

        public Task<IdentityResult> CreateAsync(IdentityUser user, string password)
        {
            return userMenager.CreateAsync(user, password);
        }

        public string GetUserName(string email)
        {
            return userMenager.FindByEmail(email)?.UserName ?? string.Empty;
        }

        public IdentityUser GetUser(string Id)
        {
            return userMenager.FindById(Id);
        }

        public void UpdateUserData(IdentityUser user)
        {
            userMenager.Update(user);
            Save();
        }

        public async Task SendConfirmationEMail(string id)
        {
            string code = await userMenager.GenerateEmailConfirmationTokenAsync(id);
            var callbackUrl = ""; //Url.Action("ConfirmEmail", "Account", new { userId = id, code = code }, protocol: Request.Url.Scheme);
            await userMenager.SendEmailAsync(id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
        }

        public Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo log)
        {
            return signInMenager.ExternalSignInAsync(log , false);
        }

        public Task SignInAsync(IdentityUser user)
        {
            return signInMenager.SignInAsync(user, false, false);
        }


        public Task<IdentityResult> CreateUserWithNoPassword(IdentityUser user)
        {
            return userMenager.CreateAsync(user);
        }

        public Task<IdentityResult> AssociateLoginWithUser(string id, ExternalLoginInfo log)
        {
            return userMenager.AddLoginAsync(id,log.Login);
        }

        
        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }




}
