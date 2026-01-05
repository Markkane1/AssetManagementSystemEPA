using AssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasOne(e => e.Location)
                .WithMany(l => l.Employees)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Directorate)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DirectorateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
