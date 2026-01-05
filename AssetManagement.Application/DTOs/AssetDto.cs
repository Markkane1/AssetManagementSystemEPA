using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.DTOs
{
    public class AssetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public AssetSource Source { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
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
    }
}