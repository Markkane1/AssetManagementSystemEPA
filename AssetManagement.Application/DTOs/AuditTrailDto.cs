namespace AssetManagement.Application.DTOs;

public class AuditTrailDto
{
    public int AssetId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public DateTime ActionDate { get; set; }
    public string Details { get; set; } = string.Empty;
    public DateTime AssignmentDate { get; }
}