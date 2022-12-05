using Microsoft.AspNetCore.Mvc;

namespace BeautyControl.API.Configurations
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.Configure<ApiBehaviorOptions>(opt => opt.SuppressModelStateInvalidFilter = true);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("TotalAccess", builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        public static void UseApiConfiguration(this WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("TotalAccess");

            app.UseAuthConfiguration();

            app.MapControllers();
        }
    }
}
