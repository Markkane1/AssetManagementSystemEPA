using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Domain.Entities;
using AssetManagement.Application.DTOs;

namespace AssetManagement.Application.Interfaces
{
    public interface IAssetRepository : IRepository<Asset>
    {
        Task<IEnumerable<Asset>> GetAllAssetsAsync(IEnumerable<int>? allowedLocationIds = null);
        Task<IEnumerable<AssetItem>> GetAllAssetItemsAsync(IEnumerable<int>? allowedLocationIds = null);
        Task<AssetItem> GetAssetItemByIdAsync(int id);
        Task AddAssetItemAsync(AssetItem assetItem);
        Task UpdateAssetItemAsync(AssetItem assetItem);
        Task DeleteAssetItemAsync(int id);
        Task<IEnumerable<Asset>> GetAssetsByLocationAsync(int locationId);
        Task<IEnumerable<Asset>> GetAssetsByCategoryAsync(int categoryId);
        Task<IEnumerable<Asset>> GetAssetsBySourceAsync(int sourceId, string sourceType);
        Task<IEnumerable<AssetItem>> GetUnassignedAssetItemsAsync(IEnumerable<int>? allowedLocationIds = null);
        Task<IEnumerable<Asset>> GetAssetsByVendorAsync(int vendorId);
        Task<IEnumerable<Asset>> GetAssetsByAcquisitionDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Asset>> GetLowStockAssetsAsync(int threshold);
        Task<IEnumerable<AssetSummaryDto>> GetAssetSummaryByLocationAsync(IEnumerable<int>? allowedLocationIds = null);
        Task<IEnumerable<CategorySummaryDto>> GetAssetSummaryByCategoryAsync(IEnumerable<int>? allowedLocationIds = null);
        Task<IEnumerable<AssetStatusReportDto>> GetAssetStatusReportAsync(IEnumerable<int>? allowedLocationIds = null);
        Task<IEnumerable<MaintenanceRecord>> GetMaintenanceHistoryForAssetAsync(int assetItemId);
        Task<IEnumerable<MaintenanceRecord>> GetAllMaintenanceRecordsAsync(IEnumerable<int>? allowedLocationIds = null);
        Task<MaintenanceRecord> GetMaintenanceRecordByIdAsync(int id);
        Task<IEnumerable<TransferHistory>> GetTransferHistoryForAssetAsync(int assetItemId);
        Task<IEnumerable<UtilizationReportDto>> GetAssetUtilizationReportAsync();
        Task<IEnumerable<MaintenanceCostSummaryDto>> GetMaintenanceCostSummaryAsync();
        Task<IEnumerable<DepreciationReportDto>> GetAssetDepreciationReportAsync();
        Task<IEnumerable<LocationTransferSummaryDto>> GetLocationTransferSummaryAsync();
        Task<IEnumerable<OverstockReportDto>> GetOverstockAssetsReportAsync(int threshold);
        Task<IEnumerable<MaintenanceDueReportDto>> GetAssetMaintenanceDueReportAsync(DateTime dueDate);
        Task<IEnumerable<AuditTrailDto>> GetAuditTrailForAssetAsync(int assetId);
    }
}