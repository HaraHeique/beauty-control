using BeautyControl.API.Domain.StockMovements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyControl.API.Infra.Data.EntityTypeConfigurations
{
    public class StockMovimentsTypeConfiguration : IEntityTypeConfiguration<StockMovements>
    {
        public void Configure(EntityTypeBuilder<StockMovements> builder)
        {
            builder.ToTable(nameof(StockMovements));

            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.Id)
                .ValueGeneratedOnAdd();

            builder.Property(sm => sm.Process)
                .IsRequired()
                .HasColumnType("TINYINT");
            
            builder.Property(sm => sm.Quantity)
                .IsRequired()
                .HasColumnType("INT");

            builder.Property(sm => sm.Date)
                .IsRequired()
                .HasColumnType("DATETIME");

            builder.HasOne(sm => sm.Product)
                .WithMany()
                .HasForeignKey(sm => sm.ProductId);

            builder.HasOne(sm => sm.Supplier)
                .WithMany()
                .HasForeignKey(sm => sm.SupplierId);
            
            builder.HasOne(sm => sm.Employee)
                .WithMany()
                .HasForeignKey(sm => sm.EmployeeId);
        }
    }
}
