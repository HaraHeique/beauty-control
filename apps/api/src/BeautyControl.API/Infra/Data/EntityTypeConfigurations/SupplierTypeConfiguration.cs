using BeautyControl.API.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyControl.API.Infra.Data.EntityTypeConfigurations
{
    public class SupplierTypeConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(128)");
            
            builder.Property(s => s.Observation)
                .IsRequired()
                .HasColumnType("VARCHAR(MAX)");

            builder.OwnsMany(s => s.Telephones, ownerBuilder => ownerBuilder.ToJson());

            builder.OwnsMany(s => s.Emails, ownerBuilder => ownerBuilder.ToJson());

            builder.Property(s => s.AverageRating)
                .IsRequired()
                .HasColumnType("DECIMAL(18,2)");
            
            builder.Property(s => s.CreationDate)
                .IsRequired()
                .HasColumnType("DATETIME");

            builder.HasMany(s => s.SupplierRatings)
                .WithOne(sr => sr.Supplier)
                .HasForeignKey("SupplierId");
        }
    }
}
