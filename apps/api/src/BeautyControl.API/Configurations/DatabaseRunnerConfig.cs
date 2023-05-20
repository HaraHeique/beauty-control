using BeautyControl.API.Infra.Data;
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
            using var scope = app.Services.CreateScope();

            // Posso verificar via HealthChecks se o banco tá Up e também verificar o ambiente que está sendo executado

            RunIdentityMigrations(scope);
            RunBusinessDataMigrations(scope);
        }

        private static void RunIdentityMigrations(IServiceScope scope)
        {
            var identityContext = scope.ServiceProvider.GetRequiredService<AppIdentityContext>();

            if (HasPendingMigrations(identityContext))
                identityContext.Database.Migrate();
        }
        
        private static void RunBusinessDataMigrations(IServiceScope scope)
        {
            var dataContext = scope.ServiceProvider.GetRequiredService<AppDataContext>();

            if (HasPendingMigrations(dataContext))
                dataContext.Database.Migrate();
        }

        private static bool HasPendingMigrations(DbContext context)
        {
            return context.Database.GetPendingMigrations().Any();
        }

        private static void EnsureSeedData(DbContext context)
        {
            // TODO: Fazer aqui a lógica para o Seed de dados em ambos contextos criados
        }
    }
}
