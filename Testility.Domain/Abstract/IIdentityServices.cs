using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Testility.Domain.Abstract
{
    public interface IIdentityServices : IDisposable
    {
        Task<IdentityUser> GetUserAsync(string name, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string type);
        Task<IdentityResult> CreateAsync(IdentityUser user, string password);
        string GetUserName(string email);
        IdentityUser GetUser(string Id);
        void UpdateUserData(IdentityUser user);
        void Save();
    }
}
