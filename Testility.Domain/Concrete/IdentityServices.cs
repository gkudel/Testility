using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Testility.Domain.Abstract;

namespace Testility.Domain.Concrete
{
    public class IdentityServices : IIdentityServices, IDisposable
    {
        private readonly UserManager<IdentityUser> userMenager;
        private readonly UserStore<IdentityUser> userStore;
        private readonly EFDbContext db;

        public IdentityServices(EFDbContext context)
        {
            db = context;

            userStore = new UserStore<IdentityUser>(context);
            userMenager = new UserManager<IdentityUser>(userStore);
            userMenager.UserValidator = new UserValidator<IdentityUser>(userMenager) {RequireUniqueEmail = true, AllowOnlyAlphanumericUserNames = false };
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
