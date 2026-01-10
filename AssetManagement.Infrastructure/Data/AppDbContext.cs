using Microsoft.EntityFrameworkCore;
using AssetManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AssetManagement.Application.Interfaces;

namespace AssetManagement.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetItem> AssetItems { get; set; }
        public DbSet<Directorate> Directorates { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<TransferHistory> TransferHistories { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<UserLocationAccess> UserLocationAccess { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}