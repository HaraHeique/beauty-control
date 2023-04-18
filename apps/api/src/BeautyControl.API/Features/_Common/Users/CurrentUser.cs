using System.Security.Claims;

namespace BeautyControl.API.Features._Common.Users
{
    public class CurrentUser
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUser(IHttpContextAccessor acessor) => _accessor = acessor;

        public HttpContext? HttpContext => _accessor.HttpContext;

        public string? Name => HttpContext?.User.Identity?.Name;

        public int Id
        {
            get
            {
                var userId = HttpContext?.User.GetUserId();

                return IsAuthenticated() && !string.IsNullOrEmpty(userId) ? int.Parse(userId) : 0;
            }
        }
        
        public string? Email => IsAuthenticated() ? HttpContext?.User.GetUserEmail() : string.Empty;

        public bool IsAuthenticated()
        {
            if (HttpContext is null) return false;

            var identity = HttpContext.User.Identity;

            return identity?.IsAuthenticated ?? false;
        }

        public bool IsInRole(string role) 
            => HttpContext is not null && HttpContext.User.IsInRole(role);

        public IEnumerable<Claim> GetClaimsIdentity() 
            => HttpContext?.User.Claims ?? Array.Empty<Claim>();

        public IEnumerable<string> GetUserRoles() 
            => IsAuthenticated() ? HttpContext!.User.GetUserRoles() : Array.Empty<string>();

        public IEnumerable<Claim> GetCustomClaims(string[] claimsType) 
            => IsAuthenticated() ? HttpContext!.User.GetUserCustomClaims(claimsType) : Array.Empty<Claim>();
    }
}
