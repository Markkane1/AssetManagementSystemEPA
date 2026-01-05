namespace AssetManagement.Application.DTOs.Auth;

public class PermissionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class UserPermissionDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public DateTime GrantedAt { get; set; }
    public string GrantedBy { get; set; } = string.Empty;
}
