using AssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.HasKey(up => up.Id);

            builder.Property(up => up.UserId)
                .IsRequired()
                .HasMaxLength(450); // Match IdentityUser Id length

            builder.Property(up => up.PermissionId)
                .IsRequired();

            builder.Property(up => up.GrantedAt)
                .IsRequired();

            builder.Property(up => up.GrantedBy)
                .IsRequired()
                .HasMaxLength(450);

            // Composite unique index: One user can only have a permission once
            builder.HasIndex(up => new { up.UserId, up.PermissionId })
                .IsUnique();

            // Index for faster user permission lookups
            builder.HasIndex(up => up.UserId);

            // Foreign key to Permission
            builder.HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
