using AssetManagement.Domain.Common;
using AssetManagement.Domain.Enums;
using AssetManagement.Domain.ValueObjects;

namespace AssetManagement.Domain.Entities;

public class PurchaseOrder : Entity, IAggregateRoot
{
    public int VendorId { get; private set; }
    public Vendor Vendor { get; private set; } = null!;
    public DateTime OrderDate { get; private set; }
    public Money TotalAmount { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }
    public string PONumber { get; private set; }
    public DateTime PurchaseDate { get; private set; }

    private readonly List<Asset> _assets = new();
    public IReadOnlyCollection<Asset> Assets => _assets.AsReadOnly();

    protected PurchaseOrder()
    {
        PONumber = null!;
        TotalAmount = null!;
    }

    public PurchaseOrder(int vendorId, DateTime orderDate, Money totalAmount, string poNumber, DateTime purchaseDate)
    {
        if (vendorId <= 0) throw new ArgumentException("Invalid Vendor ID.", nameof(vendorId));
        if (string.IsNullOrWhiteSpace(poNumber)) throw new ArgumentException("PO Number is required.", nameof(poNumber));

        VendorId = vendorId;
        OrderDate = orderDate;
        TotalAmount = totalAmount ?? throw new ArgumentNullException(nameof(totalAmount));
        PONumber = poNumber;
        PurchaseDate = purchaseDate;
        Status = PurchaseOrderStatus.Draft; // Default status
    }

    public void UpdateStatus(PurchaseOrderStatus newStatus)
    {
        // Add valid transition logic here if needed
        Status = newStatus;
    }

    public void AddAsset(Asset asset)
    {
        ArgumentNullException.ThrowIfNull(asset);
        _assets.Add(asset);
    }
}