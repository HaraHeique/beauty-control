using BeautyControl.API.Features._Common.Contracts;
using BeautyControl.API.Features._Common.PipelineBehaviors;
using BeautyControl.API.Features._Common.Users;
using BeautyControl.API.Features.Account._Common;
using BeautyControl.API.Features.Products._Common.Uploads;
using BeautyControl.API.Infra.Data;
using BeautyControl.API.Infra.Identity;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BeautyControl.API.Configurations
{
    public static class DependecyInjectionConfig
    {
        public static void RegisterDependencies(this WebApplicationBuilder builder)
        {
            RegisterContextsDependencies(builder);

            RegisterMediatRDependencies(builder);

            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            RegisterUserDependencies(builder);

            RegisterFeaturesServices(builder);
        }

        private static void RegisterContextsDependencies(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppIdentityContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"),
                sqlOptions => sqlOptions.MigrationsHistoryTable(AppIdentityContext.HistoryTableName, AppIdentityContext.Schema))
            );
            
            builder.Services.AddDbContext<AppDataContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"),
                sqlOptions => sqlOptions.MigrationsHistoryTable(AppDataContext.HistoryTableName, AppDataContext.Schema))
            );
        }

        private static void RegisterMediatRDependencies(WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(FluentResultRequestValidationBehavior<,>));
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(FluentValidationRequestValidationBehavior<,>));

                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ImageUploadBehavior<,>));
            });
        }

        private static void RegisterUserDependencies(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<CurrentUser>();
        }

        private static void RegisterFeaturesServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<JwtGenerator>();

            builder.Services.AddTransient<ImageUploadManager>();

            RegisterDynamicServices();

            void RegisterDynamicServices()
            {
                var serviceTypes = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(type => type.IsClass && !type.IsAbstract && type.IsAssignableTo(typeof(IInjectableService)));

                foreach (var type in serviceTypes)
                    builder.Services.AddTransient(type);
            }
        }
    }
}
