using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AssetManagement.Domain.Entities;
using AssetManagement.Domain.Enums;
using AssetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Infrastructure.Repositories
{
    public class AssetRepository : Repository<Asset>, IAssetRepository
    {
        private readonly AppDbContext _context;

        public AssetRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssetItem>> GetAllAssetItemsAsync(string userRole, int? userLocationId)
        {
            var query = _context.AssetItems.AsQueryable();
            if (userRole != "Admin" && userLocationId.HasValue)
                query = query.Where(ai => ai.LocationId == userLocationId.Value);
            return await query.ToListAsync();
        }

        public async Task<AssetItem> GetAssetItemByIdAsync(int id)
        {
            return await _context.AssetItems.FindAsync(id) ?? throw new KeyNotFoundException($"AssetItem with ID {id} not found.");
        }

        public async Task AddAssetItemAsync(AssetItem assetItem)
        {
            await _context.AssetItems.AddAsync(assetItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAssetItemAsync(AssetItem assetItem)
        {
            _context.AssetItems.Update(assetItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAssetItemAsync(int id)
        {
            var assetItem = await GetAssetItemByIdAsync(id);
            _context.AssetItems.Remove(assetItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Asset>> GetAssetsByLocationAsync(int locationId)
        {
            return await _context.Assets
                .Include(a => a.AssetItems)
                .Where(a => a.AssetItems.Any(ai => ai.LocationId == locationId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Asset>> GetAssetsByCategoryAsync(int categoryId)
        {
            return await _context.Assets
                .Where(a => a.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Asset>> GetAssetsBySourceAsync(int sourceId, string sourceType)
        {
            return sourceType switch
            {
                "PurchaseOrder" => await _context.Assets.Where(a => a.PurchaseOrderId == sourceId).ToListAsync(),
                "Project" => await _context.Assets.Where(a => a.ProjectId == sourceId).ToListAsync(),
                _ => new List<Asset>()
            };
        }

        public async Task<IEnumerable<AssetItem>> GetUnassignedAssetItemsAsync()
        {
            return await _context.AssetItems
                .Where(ai => ai.AssignmentStatus == AssignmentStatus.Returned)
                .ToListAsync();
        }

        public async Task<IEnumerable<Asset>> GetAssetsByVendorAsync(int vendorId)
        {
            return await _context.Assets
                .Where(a => a.VendorId == vendorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Asset>> GetAssetsByAcquisitionDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Assets
                .Where(a => a.AcquisitionDate >= startDate && a.AcquisitionDate <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Asset>> GetLowStockAssetsAsync(int threshold)
        {
            return await _context.Assets
                .Where(a => a.Quantity <= threshold)
                .ToListAsync();
        }

        public async Task<IEnumerable<AssetSummaryDto>> GetAssetSummaryByLocationAsync()
        {
            return await _context.Locations
                .Select(l => new AssetSummaryDto
                {
                    LocationId = l.Id,
                    LocationName = l.Name,
                    TotalAssets = l.AssetItems.Count,
                    TotalValue = l.AssetItems.Sum(ai => ai.Asset.Price.Amount)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CategorySummaryDto>> GetAssetSummaryByCategoryAsync()
        {
            return await _context.Categories
                .Select(c => new CategorySummaryDto
                {
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    TotalAssets = c.Assets.Sum(a => a.Quantity),
                    TotalValue = c.Assets.Sum(a => a.Price.Amount * a.Quantity)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AssetStatusReportDto>> GetAssetStatusReportAsync()
        {
            return await _context.AssetItems
                .GroupBy(ai => ai.AssignmentStatus)
                .Select(g => new AssetStatusReportDto
                {
                    AssignmentStatus = g.Key,
                    Count = g.Count(),
                    AssetItems = g.Select(ai => new AssetItemDto(
                        ai.Id,
                        ai.AssetId,
                        ai.LocationId,
                        ai.SerialNumber,
                        ai.Tag,
                        ai.AssignmentStatus)).ToList()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRecord>> GetMaintenanceHistoryForAssetAsync(int assetItemId)
        {
            return await _context.MaintenanceRecords
                .Where(mr => mr.AssetItemId == assetItemId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TransferHistory>> GetTransferHistoryForAssetAsync(int assetItemId)
        {
            return await _context.TransferHistories
                .Where(th => th.AssetItemId == assetItemId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UtilizationReportDto>> GetAssetUtilizationReportAsync()
        {
            var reports = await _context.Assets
                .Select(a => new
                {
                    AssetId = a.Id,
                    AssetName = a.Name,
                    AcquisitionDate = a.AcquisitionDate,
                    Items = a.AssetItems.Select(ai => new
                    {
                        Assignments = ai.Assignments.Where(asg => asg.ReturnDate.HasValue).Select(asg => new
                        {
                            AssignmentDate = asg.AssignmentDate,
                            ReturnDate = asg.ReturnDate
                        })
                    })
                })
                .ToListAsync();

            var result = new List<UtilizationReportDto>();
            var now = DateTime.UtcNow;

            foreach (var asset in reports)
            {
                var totalDays = (now - asset.AcquisitionDate).TotalDays;
                foreach (var item in asset.Items)
                {
                    var assignedDays = item.Assignments.Sum(a => (a.ReturnDate!.Value - a.AssignmentDate).TotalDays);
                    var utilizationRate = totalDays > 0 ? (assignedDays / totalDays) * 100 : 0;

                    result.Add(new UtilizationReportDto
                    {
                        AssetId = asset.AssetId,
                        AssetName = asset.AssetName,
                        UtilizationRate = utilizationRate
                    });
                }
            }
            return result;
        }

        public async Task<IEnumerable<MaintenanceCostSummaryDto>> GetMaintenanceCostSummaryAsync()
        {
            return await _context.MaintenanceRecords
                .GroupBy(mr => new { mr.AssetItemId, mr.AssetItem.AssetId, mr.AssetItem.LocationId })
                .Select(g => new MaintenanceCostSummaryDto
                {
                    AssetId = g.Key.AssetId,
                    CategoryId = null,
                    LocationId = g.Key.LocationId,
                    TotalCost = g.Sum(mr => mr.Cost)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<DepreciationReportDto>> GetAssetDepreciationReportAsync()
        {
            var assets = await _context.Assets
                .Select(a => new { a.Id, a.Name, a.Price, a.AcquisitionDate })
                .ToListAsync();

            return assets.Select(asset =>
            {
                var years = (DateTime.UtcNow - asset.AcquisitionDate).TotalDays / 365.25;
                var depreciationRate = 0.2; // 20% per year
                var currentValue = asset.Price.Amount * Convert.ToDecimal(Math.Max(0, 1 - depreciationRate * years));

                return new DepreciationReportDto
                {
                    AssetId = asset.Id,
                    AssetName = asset.Name,
                    OriginalPrice = asset.Price.Amount,
                    CurrentValue = currentValue,
                    AcquisitionDate = asset.AcquisitionDate
                };
            }).ToList();
        }

        public async Task<IEnumerable<LocationTransferSummaryDto>> GetLocationTransferSummaryAsync()
        {
            return await _context.TransferHistories
                .GroupBy(th => new { th.FromLocationId, th.ToLocationId })
                .Select(g => new LocationTransferSummaryDto
                {
                    FromLocationId = g.Key.FromLocationId,
                    ToLocationId = g.Key.ToLocationId,
                    TransferCount = g.Count(),
                    TotalValue = g.Sum(th => th.AssetItem.Asset.Price.Amount)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<OverstockReportDto>> GetOverstockAssetsReportAsync(int threshold)
        {
            return await _context.Assets
                .Where(a => a.Quantity > threshold)
                .Select(a => new OverstockReportDto
                {
                    AssetId = a.Id,
                    AssetName = a.Name,
                    CurrentQuantity = a.Quantity,
                    Threshold = threshold
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceDueReportDto>> GetAssetMaintenanceDueReportAsync(DateTime dueDate)
        {
            return await _context.MaintenanceRecords
                .Where(mr => mr.Status == MaintenanceStatus.Scheduled && mr.MaintenanceDate <= dueDate)
                .Select(mr => new MaintenanceDueReportDto
                {
                    AssetItemId = mr.AssetItemId,
                    AssetName = mr.AssetItem.Asset.Name,
                    DueDate = mr.MaintenanceDate
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditTrailDto>> GetAuditTrailForAssetAsync(int assetId)
        {
            var assignmentsTask = _context.Assignments
                .Where(a => a.AssetItem.AssetId == assetId)
                .Select(a => new AuditTrailDto
                {
                    AssetId = assetId,
                    ActionType = "Assignment",
                    ActionDate = a.AssignmentDate,
                    Details = $"Assigned to Employee ID {a.EmployeeId}"
                })
                .ToListAsync();

            var transfersTask = _context.TransferHistories
                .Where(th => th.AssetItem.AssetId == assetId)
                .Select(th => new AuditTrailDto
                {
                    AssetId = assetId,
                    ActionType = "Transfer",
                    ActionDate = th.TransferDate,
                    Details = $"Transferred from Location ID {th.FromLocationId} to {th.ToLocationId}"
                })
                .ToListAsync();

            var maintenanceTask = _context.MaintenanceRecords
                .Where(mr => mr.AssetItem.AssetId == assetId)
                .Select(mr => new AuditTrailDto
                {
                    AssetId = assetId,
                    ActionType = "Maintenance",
                    ActionDate = mr.MaintenanceDate,
                    Details = mr.Description
                })
                .ToListAsync();

            await Task.WhenAll(assignmentsTask, transfersTask, maintenanceTask);

            return assignmentsTask.Result
                .Concat(transfersTask.Result)
                .Concat(maintenanceTask.Result)
                .OrderBy(a => a.ActionDate);
        }
    }
}