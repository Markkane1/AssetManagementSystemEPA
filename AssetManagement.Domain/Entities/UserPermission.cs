using AssetManagement.Domain.Common;

namespace AssetManagement.Domain.Entities;

public class UserPermission : Entity
{
    public string UserId { get; private set; }
    public int PermissionId { get; private set; }
    public Permission Permission { get; private set; } = null!;
    public DateTime GrantedAt { get; private set; }
    public string GrantedBy { get; private set; }

    // Required for EF Core
    protected UserPermission()
    {
        UserId = null!;
        GrantedBy = null!;
    }

    public UserPermission(string userId, int permissionId, string grantedBy)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        if (permissionId <= 0)
            throw new ArgumentException("Permission ID must be valid.", nameof(permissionId));
        if (string.IsNullOrWhiteSpace(grantedBy))
            throw new ArgumentException("Granted by cannot be empty.", nameof(grantedBy));

        UserId = userId;
        PermissionId = permissionId;
        GrantedBy = grantedBy;
        GrantedAt = DateTime.UtcNow;
    }
}
