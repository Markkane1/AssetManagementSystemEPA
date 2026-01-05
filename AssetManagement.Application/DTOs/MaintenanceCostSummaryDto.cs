namespace AssetManagement.Application.DTOs;

public class MaintenanceCostSummaryDto
{
    public int? AssetId { get; set; }
    public int? CategoryId { get; set; }
    public int? LocationId { get; set; }
    public decimal TotalCost { get; set; }
    public object Value { get; } =string.Empty;
    public decimal V { get; }
}