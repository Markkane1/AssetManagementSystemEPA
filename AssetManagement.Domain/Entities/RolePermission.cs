using AssetManagement.Domain.Common;

namespace AssetManagement.Domain.Entities;

public class RolePermission : Entity
{
    public string RoleId { get; private set; }
    public int PermissionId { get; private set; }
    public Permission Permission { get; private set; } = null!;

    // Required for EF Core
    protected RolePermission()
    {
        RoleId = null!;
    }

    public RolePermission(string roleId, int permissionId)
    {
        if (string.IsNullOrWhiteSpace(roleId))
            throw new ArgumentException("Role ID cannot be empty.", nameof(roleId));
        if (permissionId <= 0)
            throw new ArgumentException("Permission ID must be valid.", nameof(permissionId));

        RoleId = roleId;
        PermissionId = permissionId;
    }
}
