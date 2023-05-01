using BeautyControl.API.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyControl.API.Infra.Data.EntityTypeConfigurations
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(128)");

            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnType("VARCHAR(2048)");

            builder.OwnsOne(p => p.Image, ownerBuilder =>
            {
                ownerBuilder.Property(i => i.Url)
                    .IsRequired(false)
                    .HasColumnName("ImageUrl")
                    .HasColumnType("VARCHAR(2048)");

                ownerBuilder.Property(i => i.Name)
                    .IsRequired(false)
                    .HasColumnName("ImageName")
                    .HasColumnType("VARCHAR(256)");
            });

            builder.Property(p => p.Quantity)
                .IsRequired()
                .HasColumnType("INT");

            builder.Property(p => p.RunningOutOfStock)
                .IsRequired()
                .HasColumnType("INT");

            builder.Property(p => p.Status)
                .IsRequired()
                .HasColumnName("StatusStock")
                .HasColumnType("TINYINT");

            builder.Property(p => p.Category)
                .IsRequired()
                .HasColumnType("SMALLINT");
            
            builder.Property(p => p.CreationDate)
                .IsRequired()
                .HasColumnType("DATETIME");
        }
    }
}
