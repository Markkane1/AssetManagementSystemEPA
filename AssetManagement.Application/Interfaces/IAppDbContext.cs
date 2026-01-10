using Microsoft.EntityFrameworkCore;
using AssetManagement.Domain.Entities;

namespace AssetManagement.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Category> Categories { get; set; }
    DbSet<Vendor> Vendors { get; set; }
    DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    DbSet<Project> Projects { get; set; }
    DbSet<Asset> Assets { get; set; }
    DbSet<AssetItem> AssetItems { get; set; }
    DbSet<Directorate> Directorates { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<Location> Locations { get; set; }
    DbSet<Assignment> Assignments { get; set; }
    DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    DbSet<Permission> Permissions { get; set; }
    DbSet<RolePermission> RolePermissions { get; set; }
    DbSet<UserLocationAccess> UserLocationAccess { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
