using BeautyControl.API.Features._Common.PipelineBehaviors;
using BeautyControl.API.Features._Common.Users;
using BeautyControl.API.Features.Account._Common;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace BeautyControl.API.Configurations
{
    public static class DependecyInjectionConfig
    {
        public static void RegisterDependencies(this WebApplicationBuilder builder)
        {
            RegisterMediatRDependencies(builder);

            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            RegisterFeaturesServices(builder);
        }

        private static void RegisterMediatRDependencies(WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(FluentResultRequestValidationBehavior<,>));
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(FluentValidationRequestValidationBehavior<,>));
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
        }
    }
}
