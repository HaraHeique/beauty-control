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
        }

        private static void RegisterMediatRDependencies(WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FluentResultRequestValidationBehavior<,>));
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FluentValidationRequestValidationBehavior<,>));
        }
    }
}
