using BeautyControl.API.Infra.Logging;
using FluentResults;
using Serilog;

namespace BeautyControl.API.Configurations
{
    // Configuração via código: https://henriquemauri.net/coletando-logs-com-o-serilog-no-net-6/
    public static class LoggingConfig
    {
        public static void AddLogConfiguration(this WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            Result.Setup(config =>
            {
                config.Logger = new CustomFluentValidationLogger(logger);
            });
        }
    }
}
