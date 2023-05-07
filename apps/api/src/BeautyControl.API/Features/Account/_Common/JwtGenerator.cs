using BeautyControl.API.Infra.Identity.Models;
using BeautyControl.API.Infra.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BeautyControl.API.Features.Account._Common
{
    public class JwtGenerator
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthSettings _authSettings;
        private readonly IJwtService _jwtService;

        public JwtGenerator(
            UserManager<AppUser> userManager,
            IOptions<AuthSettings> authSettings,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _authSettings = authSettings.Value;
            _jwtService = jwtService;
        }

        public async Task<LoggedUserResponse> GenerateToken(string email)
        {
            var user = await GetUserByEmail(email);
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await BuildClaimsIdentity(user, claims);
            var encodedToken = await EncodeToken(identityClaims);

            return CreateTokenResponse();

            async Task<AppUser> GetUserByEmail(string email)
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null) throw new ArgumentException("Email de usuário inválido");

                return user;
            }

            LoggedUserResponse CreateTokenResponse()
            {
                return new LoggedUserResponse
                {
                    AccessToken = encodedToken,
                    ExpiresInSeconds = TimeSpan.FromHours(_authSettings.ExpiresInHours).TotalSeconds,
                    TokenInfo = new UserTokenResponse
                    {
                        Id = user!.Id,
                        Email = user!.Email,
                        UserName = user!.UserName,
                        Claims = claims!.Select(c => new UserClaimResponse
                        {
                            Type = c.Type,
                            Value = c.Value
                        })
                    }
                };
            }
        }

        private async Task<ClaimsIdentity> BuildClaimsIdentity(AppUser user, IList<Claim> claims)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.UserName!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            AddRolesAsClaims(claims, userRoles);

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;


            static long ToUnixEpochDate(DateTime date)
                => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

            static void AddRolesAsClaims(IList<Claim> claims, IList<string> roles)
            {
                foreach (string userRole in roles)
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
        }

        private async Task<string> EncodeToken(ClaimsIdentity claimsIdentity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _authSettings.Issuer,
                Claims = new Dictionary<string, object>
                {
                    { JwtRegisteredClaimNames.Aud, _authSettings.ValidOn }
                },
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(_authSettings.ExpiresInHours),
                SigningCredentials = await _jwtService.GetCurrentSigningCredentials()
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
