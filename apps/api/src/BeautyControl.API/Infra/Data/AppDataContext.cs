using BeautyControl.API.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace BeautyControl.API.Infra.Data
{
    public class AppDataContext : DbContext
    {
        public const string Schema = "Business";
        public const string HistoryTableName = "__EFMigrationsHistory";

        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetDefaultModelColumnsType(modelBuilder);
            SetDefaultBehaviorForeignKeys(modelBuilder);

            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            #region Método locais

            void SetDefaultModelColumnsType(ModelBuilder modelBuilder)
            {
                IEnumerable<IMutableProperty> stringColumnsType = GetAllPropertiesByType(modelBuilder, typeof(string));

                foreach (var property in stringColumnsType)
                    property.SetColumnType("varchar(128)");

                IEnumerable<IMutableProperty> decimalColumnsType = GetAllPropertiesByType(modelBuilder, typeof(decimal));

                foreach (var property in decimalColumnsType)
                    property.SetColumnType("decimal(18,2)");

                IEnumerable<IMutableProperty> GetAllPropertiesByType(ModelBuilder modelBuilder, Type type)
                {
                    return modelBuilder.Model.GetEntityTypes()
                        .SelectMany(e => e.GetProperties().Where(p => p.ClrType == type));
                }
            }

            void SetDefaultBehaviorForeignKeys(ModelBuilder modelBuilder)
            {
                var foreignKeys = modelBuilder.Model.GetEntityTypes()
                    .SelectMany(e => e.GetForeignKeys());

                foreach (var relationship in foreignKeys)
                    relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            #endregion
        }
    }
}
