using System.Security.Claims;

namespace BeautyControl.API.Features._Common.Users
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            ValidateClaimsPrincipal(principal);

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

            return claim?.Value;
        }

        public static string? GetUserEmail(this ClaimsPrincipal principal)
        {
            ValidateClaimsPrincipal(principal);

            var claim = principal.FindFirst(ClaimTypes.Email);

            return claim?.Value;
        }
        
        public static string? GetUserName(this ClaimsPrincipal principal)
        {
            ValidateClaimsPrincipal(principal);

            var claim = principal.FindFirst(ClaimTypes.Name);

            return claim?.Value;
        }

        public static IEnumerable<string> GetUserRoles(this ClaimsPrincipal principal)
        {
            ValidateClaimsPrincipal(principal);

            var claims = principal.FindAll(c => c.Type == ClaimTypes.Role);

            return claims.Select(c => c.Value);
        }

        public static IEnumerable<Claim> GetUserCustomClaims(this ClaimsPrincipal principal, string[] claimTypes)
        {
            ValidateClaimsPrincipal(principal);

            var claims = principal.FindAll(c => claimTypes.Contains(c.Type));

            return claims;
        }

        private static void ValidateClaimsPrincipal(ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentException(null, nameof(principal));
        }
    }
}
