using BeautyControl.API.Features.Common.PipelineBehaviors;
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

            builder.Services.AddHttpContextAccessor();

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

        private static void RegisterFeaturesServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<JwtGenerator>();
        }
    }
}
