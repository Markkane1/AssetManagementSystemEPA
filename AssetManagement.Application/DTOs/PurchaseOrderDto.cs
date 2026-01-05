namespace AssetManagement.Application.DTOs
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
    }
}