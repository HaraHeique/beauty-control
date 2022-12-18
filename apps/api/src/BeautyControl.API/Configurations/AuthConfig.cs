﻿using BeautyControl.API.Infra.Identity;
using BeautyControl.API.Infra.Identity.Models;
using BeautyControl.API.Infra.Identity.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;

namespace BeautyControl.API.Configurations
{
    public static class AuthConfig
    {
        public static void AddAuthConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMemoryCache()
                .AddJwksManager()
                .PersistKeysInMemory();

            builder.Services.AddDbContext<AppIdentityContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"))
            );

            builder.Services.AddIdentityCore<AppUser>()
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<AppIdentityContext>()
                .AddErrorDescriber<IdentityTranslateErros>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection(AuthSettings.Key));

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

            #endregion
        }

        public static void UseAuthConfiguration(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
