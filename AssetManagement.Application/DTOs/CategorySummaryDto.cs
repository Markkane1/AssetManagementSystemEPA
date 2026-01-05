namespace AssetManagement.Application.DTOs;

public class CategorySummaryDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int TotalAssets { get; set; }
    public decimal TotalValue { get; set; }
}