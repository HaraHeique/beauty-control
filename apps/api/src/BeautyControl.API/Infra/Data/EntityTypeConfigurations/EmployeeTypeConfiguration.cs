using BeautyControl.API.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautyControl.API.Infra.Data.EntityTypeConfigurations
{
    public class EmployeeTypeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedNever();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(256)");
            
            builder.Property(e => e.Position)
                .IsRequired()
                .HasColumnType("INT");

            builder.OwnsOne(e => e.Email, ownerBuilder =>
            {
                ownerBuilder.Property(e => e.Address)
                    .IsRequired(required: true)
                    .HasColumnName("Email")
                    .HasColumnType("VARCHAR(256)");
            });
        }
    }
}
