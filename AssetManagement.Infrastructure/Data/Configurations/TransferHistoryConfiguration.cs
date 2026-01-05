using AssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations
{
    public class TransferHistoryConfiguration : IEntityTypeConfiguration<TransferHistory>
    {
        public void Configure(EntityTypeBuilder<TransferHistory> builder)
        {
            builder.HasOne(th => th.AssetItem)
                .WithMany(ai => ai.TransferHistories)
                .HasForeignKey(th => th.AssetItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(th => th.FromLocation)
                .WithMany()
                .HasForeignKey(th => th.FromLocationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(th => th.ToLocation)
                .WithMany()
                .HasForeignKey(th => th.ToLocationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
