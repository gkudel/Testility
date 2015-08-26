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

        public IdentityServices(EFDbContext context, IAuthenticationManager authenticationMana, UserStore<IdentityUser> userStor, UserManager<IdentityUser> userMena)
        {
            db = context;

            //userStore = userStor;
            //userMenager = userMena;

            authenticationManager = authenticationMana;
            userStore = new UserStore<IdentityUser>(db);
            userMenager = new UserManager<IdentityUser>(userStore);
            userMenager.UserValidator = new UserValidator<IdentityUser>(userMenager) { RequireUniqueEmail = true, AllowOnlyAlphanumericUserNames = false };
            userMenager.PasswordValidator = new PasswordValidator() { RequiredLength = 6, RequireLowercase = true, RequireUppercase = true, RequireDigit = true };

            signInMenager = new SignInManager<IdentityUser, string>(userMenager, authenticationManager);
            
            userMenager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<IdentityUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            userMenager.EmailService = new EmailService();

            var dataProtectionProvider = Startup.dataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                IDataProtector dataProtector = dataProtectionProvider.Create("ASP.NET Identity");
                userMenager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(dataProtector);
            }
        }



        #region Two_Factor_Cookies
        public void SetTwoFactorAuthCookie(string id)
        {
            ClaimsIdentity identity = new ClaimsIdentity(DefaultAuthenticationTypes.TwoFactorCookie);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id));
            authenticationManager.SignIn(identity);
        }


        public async Task<string> GetTwoFactorUserIdAsync()
        {
            var result = await authenticationManager.AuthenticateAsync(DefaultAuthenticationTypes.TwoFactorCookie);

            if (result != null && result.Identity != null && !String.IsNullOrEmpty(result.Identity.GetUserId()))
            {
                return result.Identity.GetUserId();
            }
            return null;
        }

        #endregion


        #region Create_IdentityUser_Data
        public Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string type)
        {
            return userMenager.CreateIdentityAsync(user, type);
        }
        public Task<IdentityResult> CreateAsync(IdentityUser user, string password)
        {
            return userMenager.CreateAsync(user, password);
        }
        public Task<IdentityResult> CreateUserWithNoPassword(IdentityUser user)
        {
            return userMenager.CreateAsync(user);
        }
        public Task<IdentityResult> AssociateLoginWithUser(string id, ExternalLoginInfo log)
        {
            return userMenager.AddLoginAsync(id, log.Login);
        }
        #endregion


        #region Update_IdentityUser_Data
        public void UpdateUserData(IdentityUser user)
        {
            userMenager.Update(user);
            Save();
        }

        public async Task<IdentityResult> SetTwoFactorEnabledProtection(string id)
        {
            var result = await userMenager.SetTwoFactorEnabledAsync(id, true);
            return result;
        }
        #endregion


        #region Get_IdentityUser_Data
        public IdentityUser GetUser(string Id)
        {
            return userMenager.FindById(Id);
        }

        public IdentityUser GetUserByEmail(string email)
        {
            return userMenager.FindByEmail(email);
        }

        public Task<IdentityUser> GetUserAsync(string name, string password)
        {
            return userMenager.FindAsync(name, password);

        }

        public string GetUserName(string email)
        {
            return userMenager.FindByEmail(email)?.UserName ?? string.Empty;
        }

        public async Task<IdentityUser> GetUserById(string id)
        {
            var user = await userMenager.FindByIdAsync(id);
            return user;
        }

        public async Task<IdentityUser> FindUserByName(string name)
        {
            var user = await userMenager.FindByNameAsync(name);
            return user;
        }

        public async Task<bool> IsEmailConfirmed(string Id)
        {
            var result = await userMenager.IsEmailConfirmedAsync(Id);
            return result;
        }

        #endregion


        #region Email_Confirmation
        public async Task SendConfirmationEMail(string callbackUrl, string Id)
        {
            await userMenager.SendEmailAsync(Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
        }


        public async Task<string> GenerateEmailToken(string id)
        {
            string code = await userMenager.GenerateEmailConfirmationTokenAsync(id);
            return code;
        }

        public async Task<IdentityResult> confirmEmail(string id, string token)
        {
           var result = await userMenager.ConfirmEmailAsync(id, token);
           return result;
        }

        #endregion


        #region Two_Factor_Token
        public async Task<IdentityResult> GenerateTokenToLogin(string id)
        {
           var token = await userMenager.GenerateTwoFactorTokenAsync(id, "EmailCode");
           var result = await userMenager.NotifyTwoFactorTokenAsync(id, "EmailCode", token);
           return result;
        }
        public async Task<bool> VerifyTokenToLogin(string id, string token)
        {
           var result = await userMenager.VerifyTwoFactorTokenAsync(id, "EmailCode", token);
           return result;
        }

        #endregion


        #region Sign_IdentityUser
        public Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo log)
        {
            return signInMenager.ExternalSignInAsync(log , false);
        }

        public Task SignInAsync(IdentityUser user)
        {
            return signInMenager.SignInAsync(user, false, false);
        }

        public void SignOut()
        {
            authenticationManager.SignOut("ApplicationCookie");
        }

        #endregion


        #region Password_Reset()
        public async Task<string> GeneratePasswordResetToken(string id)
        {
           var token = await userMenager.GeneratePasswordResetTokenAsync(id);
           return token;
        }

        public async Task<IdentityResult> ResetPassword (string id, string token, string newPassword)
        {
            var result = await userMenager.ResetPasswordAsync(id, token, newPassword);
            return result;
        }

        public async Task SendResetPasswordMail(string callbackUrl, string Id)
        {
            await userMenager.SendEmailAsync(Id, "Reset your password", "You will reset your password by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
        }

        #endregion


        #region Save
        public void Save()
        {
            db.SaveChanges();
        }
        #endregion

        public void Dispose()
        {
            db.Dispose();
        }

    }




}
