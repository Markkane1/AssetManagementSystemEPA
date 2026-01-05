using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.DTOs;

public class AssetStatusReportDto
{
    public AssignmentStatus AssignmentStatus { get; set; }
    public int Count { get; set; }
    public List<AssetItemDto> AssetItems { get; set; } = new List<AssetItemDto>();
    public AssignmentStatus Key { get; }
    public List<AssetItemDto> AssetItemDtos { get; } = new List<AssetItemDto>();
}