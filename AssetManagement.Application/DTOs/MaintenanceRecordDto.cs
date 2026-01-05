namespace AssetManagement.Application.DTOs
{
    public class MaintenanceRecordDto
    {
        public int Id { get; set; }
        public int AssetItemId { get; set; }
        public string AssetItemSerialNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal? Cost { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}