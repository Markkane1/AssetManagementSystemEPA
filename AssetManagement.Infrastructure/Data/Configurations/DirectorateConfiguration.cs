using AssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations
{
    public class DirectorateConfiguration : IEntityTypeConfiguration<Directorate>
    {
        public void Configure(EntityTypeBuilder<Directorate> builder)
        {
            builder.HasOne(d => d.Location)
                .WithMany(l => l.Directorates)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
