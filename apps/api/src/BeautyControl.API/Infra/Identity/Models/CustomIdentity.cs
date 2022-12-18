using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BeautyControl.API.Infra.Identity.Models
{
    public class AppUser : IdentityUser<int>
    {
        public bool Active { get; set; }
    }

    public class AppRole : IdentityRole<int> { }

    public class AppUserRole : IdentityUserRole<int> { }

    public class AppUserClaim : IdentityUserClaim<int> { }

    public class AppUserLogin : IdentityUserLogin<int> { }

    public class AppUserToken : IdentityUserToken<int> { }

    public class AppRoleClaim : IdentityRoleClaim<int> { }

    public class AppUserStore : UserStore<AppUser, AppRole, AppIdentityContext, int, AppUserClaim, AppUserRole, AppUserLogin, AppUserToken, AppRoleClaim>
    {
        public AppUserStore(AppIdentityContext context) : base(context) { }
    }

    public class AppRoleStore : RoleStore<AppRole, AppIdentityContext, int, AppUserRole, AppRoleClaim>
    {
        public AppRoleStore(AppIdentityContext context) : base(context) { }
    }
}
