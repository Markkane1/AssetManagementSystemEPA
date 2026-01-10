using AssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations
{
    public class UserLocationAccessConfiguration : IEntityTypeConfiguration<UserLocationAccess>
    {
        public void Configure(EntityTypeBuilder<UserLocationAccess> builder)
        {
            builder.HasKey(ula => new { ula.UserId, ula.LocationId });

            builder.Property(ula => ula.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(ula => ula.LocationId)
                .IsRequired();

            // Foreign key to Location
            builder.HasOne(ula => ula.Location)
                .WithMany()
                .HasForeignKey(ula => ula.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for faster lookups
            builder.HasIndex(ula => ula.UserId);
        }
    }
}
