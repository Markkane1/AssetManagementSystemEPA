using AssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations
{
    public class AssetItemConfiguration : IEntityTypeConfiguration<AssetItem>
    {
        public void Configure(EntityTypeBuilder<AssetItem> builder)
        {
            builder.HasIndex(ai => ai.Tag)
                .IsUnique();

            builder.HasIndex(ai => ai.SerialNumber)
                .IsUnique();

            // AssignmentStatus removed


            builder.Property(ai => ai.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(ai => ai.Source)
                .HasConversion<string>()
                .IsRequired();

            builder.HasOne(ai => ai.Location)
                .WithMany(l => l.AssetItems)
                .HasForeignKey(ai => ai.LocationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
