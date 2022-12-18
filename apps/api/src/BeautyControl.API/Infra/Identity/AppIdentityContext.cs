using BeautyControl.API.Infra.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Infra.Identity
{
    public class AppIdentityContext : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public AppIdentityContext(DbContextOptions<AppIdentityContext> options) : base(options) { }
    }
}
