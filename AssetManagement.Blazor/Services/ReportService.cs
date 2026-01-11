using System.Net.Http.Json;
using AssetManagement.Application.DTOs;

namespace AssetManagement.Blazor.Services;

public class ReportService
{
    private readonly HttpClient _httpClient;

    public ReportService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AssetSummaryDto>> GetAssetSummaryByLocationAsync()
        => await _httpClient.GetFromJsonAsync<List<AssetSummaryDto>>("api/reports/asset-summary/location") ?? [];

    public async Task<List<CategorySummaryDto>> GetAssetSummaryByCategoryAsync()
        => await _httpClient.GetFromJsonAsync<List<CategorySummaryDto>>("api/reports/asset-summary/category") ?? [];

    public async Task<List<DirectorateAssignmentSummaryDto>> GetAssignmentSummaryByDirectorateAsync()
        => await _httpClient.GetFromJsonAsync<List<DirectorateAssignmentSummaryDto>>("api/reports/assignment-summary/directorate") ?? [];

    public async Task<List<AssetStatusReportDto>> GetAssetStatusReportAsync()
        => await _httpClient.GetFromJsonAsync<List<AssetStatusReportDto>>("api/reports/asset-status") ?? [];

    public async Task<List<MaintenanceRecordDto>> GetMaintenanceHistoryAsync(int assetItemId)
        => await _httpClient.GetFromJsonAsync<List<MaintenanceRecordDto>>($"api/reports/maintenance-history/{assetItemId}") ?? [];

    public async Task<List<TransferHistoryDto>> GetTransferHistoryAsync(int assetItemId)
        => await _httpClient.GetFromJsonAsync<List<TransferHistoryDto>>($"api/reports/transfer-history/{assetItemId}") ?? [];

    public async Task<List<UtilizationReportDto>> GetUtilizationAsync()
        => await _httpClient.GetFromJsonAsync<List<UtilizationReportDto>>("api/reports/asset-utilization") ?? [];

    public async Task<List<MaintenanceCostSummaryDto>> GetMaintenanceCostAsync()
        => await _httpClient.GetFromJsonAsync<List<MaintenanceCostSummaryDto>>("api/reports/maintenance-cost") ?? [];

    public async Task<List<DepreciationReportDto>> GetDepreciationAsync()
        => await _httpClient.GetFromJsonAsync<List<DepreciationReportDto>>("api/reports/depreciation") ?? [];

    public async Task<List<LocationTransferSummaryDto>> GetLocationTransferSummaryAsync()
        => await _httpClient.GetFromJsonAsync<List<LocationTransferSummaryDto>>("api/reports/location-transfer") ?? [];

    public async Task<List<EmployeeAssetValueSummaryDto>> GetEmployeeAssetValueSummaryAsync()
        => await _httpClient.GetFromJsonAsync<List<EmployeeAssetValueSummaryDto>>("api/reports/employee-asset-value") ?? [];

    public async Task<List<AuditTrailDto>> GetAuditTrailAsync(int assetId)
        => await _httpClient.GetFromJsonAsync<List<AuditTrailDto>>($"api/reports/audit-trail/{assetId}") ?? [];

    public async Task<List<OverstockReportDto>> GetOverstockAsync(int threshold)
        => await _httpClient.GetFromJsonAsync<List<OverstockReportDto>>($"api/reports/overstock/{threshold}") ?? [];

    public async Task<List<MaintenanceDueReportDto>> GetMaintenanceDueAsync(DateTime dueDate)
        => await _httpClient.GetFromJsonAsync<List<MaintenanceDueReportDto>>($"api/reports/maintenance-due?dueDate={dueDate:O}") ?? [];

    public async Task<List<AssetAssignmentReportDto>> GetAssetAssignmentAsync()
        => await _httpClient.GetFromJsonAsync<List<AssetAssignmentReportDto>>("api/reports/asset-assignment") ?? [];

    public async Task<List<NonRepairableAssetReportDto>> GetNonRepairableAsync(int? locationId = null)
    {
        var query = locationId.HasValue ? $"?locationId={locationId}" : string.Empty;
        return await _httpClient.GetFromJsonAsync<List<NonRepairableAssetReportDto>>($"api/reports/non-repairable{query}") ?? [];
    }
}
