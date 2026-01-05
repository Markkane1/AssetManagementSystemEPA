

namespace AssetManagement.Application.DTOs
{
    public class OverstockReportDto
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public int CurrentQuantity { get; set; }
        public int Threshold { get; set; }
        public int Id { get; }
        public string Name { get; } = string.Empty;
        public int Quantity { get; }
    }
}