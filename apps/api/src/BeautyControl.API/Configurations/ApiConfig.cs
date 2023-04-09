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

            builder.Services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
            });

            builder.Services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });

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
            app.UseHsts();

            app.UseApiVersioning();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("TotalAccess");

            app.UseAuthConfiguration();

            app.MapControllers();
        }
    }
}
