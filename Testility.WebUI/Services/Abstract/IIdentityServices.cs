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
        Task<IdentityUser> GetUserAsync(string name, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string type);
        Task<IdentityResult> CreateAsync(IdentityUser user, string password);

        Task<IdentityResult> CreateUserWithNoPassword(IdentityUser user);
        Task<IdentityResult> AssociateLoginWithUser(string id, ExternalLoginInfo log);

        string GetUserName(string email);
        IdentityUser GetUser(string Id);
        void UpdateUserData(IdentityUser user);
        Task SendConfirmationEMail(string id);


        Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo log);
        Task SignInAsync(IdentityUser user);

        void Save();
    }
}
