using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Testility.WebUI.Services.Abstract
{
    public interface IIdentityServices : IDisposable
    {
        #region Two_Factor_Cookies
        void SetTwoFactorAuthCookie(string id);

        Task<string> GetTwoFactorUserIdAsync();
        #endregion


        #region Create_IdentityUser_Data
        Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string type);
        Task<IdentityResult> CreateAsync(IdentityUser user, string password);
        Task<IdentityResult> CreateUserWithNoPassword(IdentityUser user);
        Task<IdentityResult> AssociateLoginWithUser(string id, ExternalLoginInfo log);
        #endregion


        #region Update_IdentityUser_Data
        void UpdateUserData(IdentityUser user);
        Task<IdentityResult> SetTwoFactorEnabledProtection(string id);
        #endregion


        #region Get_IdentityUser_Data
        Task<IdentityUser> GetUserAsync(string name, string password);
        string GetUserName(string email);
        IdentityUser GetUser(string Id);
        IdentityUser GetUserByEmail(string email);
        Task<IdentityUser> GetUserById(string id);

        Task<IdentityUser> FindUserByName(string name);

        Task<bool> IsEmailConfirmed(string Id);

        #endregion


        #region Email_confirmation
        Task SendConfirmationEMail(string callbackUrl, string Id);
        Task<string> GenerateEmailToken(string id);
        Task<IdentityResult> confirmEmail(string id, string token);
        #endregion


        #region Two_Factor_Token
        Task<IdentityResult> GenerateTokenToLogin(string id);
        Task<bool> VerifyTokenToLogin(string id, string token);
        #endregion


        #region Sign_IdentityUser
        Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo log);
        Task SignInAsync(IdentityUser user);

        void SignOut();
        #endregion


        #region Password_Reset()
        Task<string> GeneratePasswordResetToken(string id);

        Task<IdentityResult> ResetPassword(string id, string token, string newPassword);

        Task SendResetPasswordMail(string callbackUrl, string Id);
        #endregion



        #region Save
        void Save();
        #endregion
    }
}
