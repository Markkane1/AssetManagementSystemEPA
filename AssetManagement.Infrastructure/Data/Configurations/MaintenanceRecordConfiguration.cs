using AssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations
{
    public class MaintenanceRecordConfiguration : IEntityTypeConfiguration<MaintenanceRecord>
    {
        public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
        {
            builder.Property(m => m.Status)
                .HasConversion<string>();

            builder.Property(m => m.Cost)
                .HasColumnType("decimal(18,2)");

            // Matching query filter to avoid warnings with required AssetItem/Asset relationship
            builder.HasQueryFilter(m => !m.AssetItem.Asset.IsDeleted);
        }
    }
}
