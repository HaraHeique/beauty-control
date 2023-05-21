using BeautyControl.API.Infra.Data;
using BeautyControl.API.Infra.Data.SeedData;
using BeautyControl.API.Infra.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Configurations
{
    public static class DatabaseRunnerConfig
    {
        /// <summary>
        /// Generate migrations before running this method, you can use the following command:
        /// Nuget package manager: Add-Migration {desired_migration_name} -context {desired_context_class_name}
        /// Dotnet CLI: dotnet ef migrations add {desired_migration_name} -c {desired_context_class_name}
        /// </summary>
        /// <param name="app"></param>
        public static void EnsureRunMigration(this WebApplication app)
        {
            if (ShouldAutoRunMigration(app.Configuration) == false) return;

            using var scope = app.Services.CreateScope();

            var dataContext = scope.ServiceProvider.GetRequiredService<AppDataContext>();
            var identityContext = scope.ServiceProvider.GetRequiredService<AppIdentityContext>();

            RunMigrations(identityContext, app.Configuration);
            
            RunMigrations(dataContext, app.Configuration, new string[] {
                ProductSeedData.GetScriptSQL(), SupplierSeedData.GetScriptSQL()
            });
        }

        private static void RunMigrations(DbContext context, IConfiguration configuration, params string[] sqlScriptsForSeedData)
        {
            if (HasNoMigrationsApplied(context, configuration))
            {
                context.Database.Migrate();
                EnsureSeedData(context, sqlScriptsForSeedData);
            }
            else if (HasPendingMigrations(context, configuration))
                context.Database.Migrate(); // Esta é a linha mais importante e de fato executa as migrations
        }

        private static void EnsureSeedData(DbContext context, params string[] sqlScripts)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                foreach (var sql in sqlScripts)
                    context.Database.ExecuteSqlRaw(sql);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        private static bool HasPendingMigrations(DbContext context, IConfiguration configuration) 
            => ShouldAutoRunMigration(configuration) && context.Database.GetPendingMigrations().Any();

        private static bool HasNoMigrationsApplied(DbContext context, IConfiguration configuration) 
            => configuration.GetValue<bool>("Migrations:SeedData") && !context.Database.GetAppliedMigrations().Any();

        private static bool ShouldAutoRunMigration(IConfiguration configuration)
            => configuration.GetValue<bool>("Migrations:AutoRun");
    }
}
