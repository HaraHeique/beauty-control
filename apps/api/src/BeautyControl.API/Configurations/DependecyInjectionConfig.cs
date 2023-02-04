using MediatR;
using System.Reflection;

namespace BeautyControl.API.Configurations
{
    public static class DependecyInjectionConfig
    {
        public static void RegisterDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

            builder.Services.AddHttpContextAccessor();
        }
    }
}
