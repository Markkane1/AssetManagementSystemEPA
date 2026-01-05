namespace AssetManagement.Application.DTOs;

public class DirectorateAssignmentSummaryDto
{
    public int DirectorateId { get; set; }
    public string DirectorateName { get; set; } = string.Empty;
    public int TotalAssignments { get; set; }
    public int TotalAssetItems { get; set; }
}