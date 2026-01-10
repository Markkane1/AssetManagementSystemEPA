using AssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.OwnsOne(a => a.Price, money =>
            {
                money.Property(p => p.Amount).HasColumnName("Price").HasColumnType("decimal(18,2)");
                money.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3);
            });

            builder.Ignore(a => a.TrackedQuantity);

            builder.Property(a => a.AssetCode).IsRequired();
            builder.Property(a => a.Name).IsRequired();

            // Soft delete query filter
            builder.HasQueryFilter(a => !a.IsDeleted);

            builder.HasOne(a => a.PurchaseOrder)
                .WithMany(po => po.Assets)
                .HasForeignKey(a => a.PurchaseOrderId)
                .IsRequired(false);

            builder.HasOne(a => a.Project)
                .WithMany(p => p.Assets)
                .HasForeignKey(a => a.ProjectId)
                .IsRequired(false);
        }
    }
}
