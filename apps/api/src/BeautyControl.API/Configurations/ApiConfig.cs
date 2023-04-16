using BeautyControl.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace BeautyControl.API.Configurations
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            AddVersionConfig(builder);

            AddCorsConfig(builder);

            AddRateLimitingConfig(builder);

            #region Métodos Locais

            static void AddVersionConfig(WebApplicationBuilder builder)
            {
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
            }

            static void AddCorsConfig(WebApplicationBuilder builder)
            {
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

            static void AddRateLimitingConfig(WebApplicationBuilder builder)
            {
                builder.Services.AddRateLimiter(options =>
                {
                    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                    // Global para aplicação toda
                    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext => RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(), factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 10,
                        QueueLimit = 0,
                        Window = TimeSpan.FromSeconds(1)
                    }));

                    // Para um grupo de endpoints (tem que usar a annotation/decorator EnableRateLimiting) 
                    options.AddFixedWindowLimiter("Api", options =>
                    {
                        options.PermitLimit = 10;
                        options.Window = TimeSpan.FromSeconds(1);
                        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                        options.QueueLimit = 5;
                    });
                });
            }

            #endregion
        }

        public static void UseApiConfiguration(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseHsts();

            app.UseApiVersioning();

            app.UseRateLimiter();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("TotalAccess");

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthConfiguration();

            app.MapControllers();
        }
    }
}
