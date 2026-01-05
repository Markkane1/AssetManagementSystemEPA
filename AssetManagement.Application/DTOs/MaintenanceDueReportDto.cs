namespace AssetManagement.Application.DTOs;

public class MaintenanceDueReportDto
{
    public int AssetItemId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
}