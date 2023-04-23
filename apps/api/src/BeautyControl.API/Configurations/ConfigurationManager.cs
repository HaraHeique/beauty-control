namespace BeautyControl.API.Configurations
{
    public static class ConfigurationManager
    {
        // Provavelmente essa config é desnecessário, pois o .Net já faz nativamente, o qual fica repetido
        public static void Configure(this WebApplicationBuilder appBuilder)
        {
            appBuilder.Configuration
                .SetBasePath(appBuilder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{appBuilder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (appBuilder.Environment.IsDevelopment())
                appBuilder.Configuration.AddUserSecrets<Program>();
        }
    }
}
