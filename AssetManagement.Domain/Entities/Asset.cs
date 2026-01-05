using AssetManagement.Domain.Common;
using AssetManagement.Domain.ValueObjects;

namespace AssetManagement.Domain.Entities;

public class Asset : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public int? VendorId { get; private set; }
    public Vendor? Vendor { get; private set; }
    public int? PurchaseOrderId { get; private set; }
    public PurchaseOrder? PurchaseOrder { get; private set; }
    public int? ProjectId { get; private set; }
    public Project? Project { get; private set; }
    public DateTime AcquisitionDate { get; private set; }

    // Using Value Object for Price
    public Money Price { get; private set; }

    public int Quantity { get; private set; }

    private readonly List<AssetItem> _assetItems = new();
    public IReadOnlyCollection<AssetItem> AssetItems => _assetItems.AsReadOnly();

    // Required for EF Core
    protected Asset()
    {
        Name = null!;
        Price = null!;
    }

    public Asset(string name, int categoryId, DateTime acquisitionDate, Money price, int quantity)
    {
        // Enforce invariants
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (categoryId <= 0) throw new ArgumentException("Invalid category ID.", nameof(categoryId));
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));

        Name = name;
        CategoryId = categoryId;
        AcquisitionDate = acquisitionDate;
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Quantity = quantity;
    }

    public void UpdateDetails(string name, DateTime acquisitionDate)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));
        Name = name;
        AcquisitionDate = acquisitionDate;
    }

    public void SetVendor(int? vendorId)
    {
        VendorId = vendorId;
    }

    public void SetProject(int? projectId)
    {
        ProjectId = projectId;
    }

    public void SetPurchaseOrder(int? purchaseOrderId)
    {
        PurchaseOrderId = purchaseOrderId;
    }

    // Example logic to add items
    public void AddAssetItem(AssetItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _assetItems.Add(item);
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));
        Quantity = quantity;
    }
}