using BeautyControl.API.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyControl.API.Infra.Data.EntityTypeConfigurations
{
    public class SupplierRatingTypeConfiguration : IEntityTypeConfiguration<SupplierRating>
    {
        public void Configure(EntityTypeBuilder<SupplierRating> builder)
        {
            builder.ToTable("SuppliersRatings");

            builder.HasKey(sr => sr.Id);

            builder.Property(sr => sr.Id)
                .ValueGeneratedOnAdd();

            builder.Property(sr => sr.Date)
                .IsRequired()
                .HasColumnType("DATETIME");

            builder.Property(sr => sr.Rating)
                .IsRequired()
                .HasColumnType("DECIMAL(18,2)");

            builder.HasOne(sr => sr.Supplier)
                .WithMany(s => s.SupplierRatings)
                .HasForeignKey("SupplierId");

            builder.HasOne(sr => sr.Employee)
                .WithMany()
                .HasForeignKey("EmployeeId");
        }
    }
}
