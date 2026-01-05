namespace AssetManagement.Application.DTOs
{
    public class UtilizationReportDto
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public double UtilizationRate { get; set; }
    }
}