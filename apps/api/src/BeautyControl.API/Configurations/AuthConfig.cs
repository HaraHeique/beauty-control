using BeautyControl.API.Infra.Identity;
using BeautyControl.API.Infra.Identity.Models;
using BeautyControl.API.Infra.Identity.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using NetDevPack.Security.PasswordHasher.Core;

namespace BeautyControl.API.Configurations
{
    public static class AuthConfig
    {
        public static void AddAuthConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMemoryCache()
                .AddJwksManager()
                .PersistKeysInMemory();

            builder.Services.AddIdentity<AppUser, AppRole>(SetupIdentityOptions)
                .AddEntityFrameworkStores<AppIdentityContext>()
                .AddErrorDescriber<IdentityTranslateErrorsDescriber>()
                .AddDefaultTokenProviders();

            builder.Services.UpgradePasswordSecurity()
                .WithStrengthen(PasswordHasherStrength.Moderate) // Sensitive é o mais forte
                .UseArgon2<AppUser>();

            builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection(AuthSettings.Key));

            builder.Services.AddDbContext<AppIdentityContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"))
            );

            AddJwtValidationConfig();

            #region Métodos locais

            void AddJwtValidationConfig()
            {
                using var scope = builder.Services.BuildServiceProvider().CreateScope();

                var authSettings = scope.ServiceProvider
                    .GetRequiredService<IOptions<AuthSettings>>()
                    .Value;

                var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

                builder.Services.AddAuthentication(authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(async jwtOptions =>
                {
                    jwtOptions.RequireHttpsMetadata = true;
                    jwtOptions.SaveToken = true;
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = authSettings.Issuer,
                        ValidAudiences = authSettings.ValidOn,
                        IssuerSigningKey = await jwtService.GetCurrentSecurityKey()
                    };
                });

                builder.Services.AddAuthorization();
            }

            void SetupIdentityOptions(IdentityOptions options)
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;
            }

            #endregion
        }

        public static void UseAuthConfiguration(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
