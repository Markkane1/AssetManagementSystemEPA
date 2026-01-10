using AssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(rp => rp.Id);

            builder.Property(rp => rp.RoleId)
                .IsRequired()
                .HasMaxLength(450); // Match IdentityRole Id length

            builder.Property(rp => rp.PermissionId)
                .IsRequired();

            // Composite unique index
            builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId })
                .IsUnique();

            // Foreign key to Permission
            builder.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Note: We don't strictly enforce a FK to AspNetRoles here if we want to avoid 
            // direct dependency on Identity tables in a specific way, but usually it's fine.
            // builder.HasOne<IdentityRole>() ... 
            // Since RoleId is just a string, it maps to the IdentityRole.Id.
        }
    }
}
