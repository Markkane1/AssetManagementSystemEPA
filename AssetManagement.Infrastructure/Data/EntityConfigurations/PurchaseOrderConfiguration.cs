using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AssetManagement.Domain.Entities;

namespace AssetManagement.Infrastructure.Data.EntityConfigurations
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            // Configure Money value object as an owned type so EF Core knows how to map it
            builder.OwnsOne(p => p.TotalAmount, ta =>
            {
                ta.Property(m => m.Amount)
                  .HasColumnName("TotalAmount")
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

                ta.Property(m => m.Currency)
                  .HasColumnName("TotalCurrency")
                  .HasMaxLength(3)
                  .IsRequired();
            });

            // If PurchaseOrder uses a private backing field for the Assets collection,
            // ensure EF uses field access so the collection is mapped correctly.
            var nav = builder.Metadata.FindNavigation(nameof(PurchaseOrder.Assets));
            if (nav != null)
            {
                nav.SetPropertyAccessMode(PropertyAccessMode.Field);
            }
        }
    }
}