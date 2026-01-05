namespace AssetManagement.Application.DTOs;

public class DepreciationReportDto
{
    public int AssetId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public decimal OriginalPrice { get; set; }
    public decimal CurrentValue { get; set; }
    public DateTime AcquisitionDate { get; set; }
}
