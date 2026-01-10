using AssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.Property(a => a.AssignmentStatus)
                .HasConversion<string>()
                .IsRequired();

            // Matching query filter to avoid warnings with required AssetItem/Asset relationship
            builder.HasQueryFilter(a => !a.AssetItem.Asset.IsDeleted);
        }
    }
}
