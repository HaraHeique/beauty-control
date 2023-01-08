using BeautyControl.API.Infra.Identity;
using BeautyControl.API.Infra.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace BeautyControl.API.Configurations
{
    public static class DependecyInjectionConfig
    {
        public static void RegisterDependecies(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

            builder.Services.AddHttpContextAccessor();
        }
    }
}
