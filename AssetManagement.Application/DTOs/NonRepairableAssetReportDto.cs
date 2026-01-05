using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.DTOs;

public class NonRepairableAssetReportDto
{
    public int AssetItemId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public AssignmentStatus AssignmentStatus { get; set; }
    public ItemStatus ItemStatus { get; set; }
    public ItemSource ItemSource { get; set; }
}