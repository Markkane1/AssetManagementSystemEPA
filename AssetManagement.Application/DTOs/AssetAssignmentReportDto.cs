using System;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.DTOs;

public class AssetAssignmentReportDto
{
    public int AssetItemId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public AssignmentStatus AssignmentStatus { get; set; }
    public ItemStatus ItemStatus { get; set; }
    public ItemSource ItemSource { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? AssignmentDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}