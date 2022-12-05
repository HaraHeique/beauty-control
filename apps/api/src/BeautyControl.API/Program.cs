using BeautyControl.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

ConfigurePipeline(app);

app.Run();

#region Métodos locais

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.AddApiConfiguration();
    builder.AddAuthConfiguration();
    builder.AddSwaggerConfiguration();
}

static void ConfigurePipeline(WebApplication app)
{
    app.UseApiConfiguration();
    app.UseAuthConfiguration();
    app.UseSwaggerConfiguration();
}

#endregion