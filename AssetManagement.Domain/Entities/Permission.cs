using AssetManagement.Domain.Common;

namespace AssetManagement.Domain.Entities;

public class Permission : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<RolePermission> _rolePermissions = new();
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    // Required for EF Core
    protected Permission()
    {
        Name = null!;
        Description = null!;
        Category = null!;
    }

    public Permission(string name, string description, string category)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Permission name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Permission category cannot be empty.", nameof(category));

        Name = name;
        Description = description ?? string.Empty;
        Category = category;
        IsActive = true;
    }

    public void UpdateDetails(string description, string category)
    {
        Description = description ?? Description;
        Category = category ?? Category;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
