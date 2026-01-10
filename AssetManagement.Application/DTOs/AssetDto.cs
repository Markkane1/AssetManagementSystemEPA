using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.DTOs
{
    public class AssetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AssetCode { get; set; } = string.Empty;
        public AssetSource Source { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public int UntrackedQuantity { get; set; }
        public int TrackedQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
        public string? VendorAddress { get; set; }
        public string? VendorContactNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? PurchaseOrderId { get; set; }
        public string? PurchaseOrderNumber { get; set; }

        // New Business Fields
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Specification { get; set; } = string.Empty;
        public DateTime? WarrantyEndDate { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}