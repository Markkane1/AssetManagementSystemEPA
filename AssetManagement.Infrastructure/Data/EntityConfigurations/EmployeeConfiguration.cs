using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AssetManagement.Domain.Entities;
using AssetManagement.Domain.ValueObjects;

namespace AssetManagement.Infrastructure.Data.EntityConfigurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Map Email value object as an owned type so EF Core does not treat it as a separate entity
            builder.OwnsOne(e => e.Email, eb =>
            {
                eb.Property(v => v.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            // If Employee uses a private backing field for collections or value objects,
            // ensure EF uses field access where appropriate (example shown for any collection named _items).
            // var nav = builder.Metadata.FindNavigation(nameof(Employee.SomeCollection));
            // if (nav != null) nav.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}