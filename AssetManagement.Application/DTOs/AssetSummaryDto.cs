namespace AssetManagement.Application.DTOs;

public class AssetSummaryDto
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public int TotalAssets { get; set; }
    public decimal TotalValue { get; set; }
}