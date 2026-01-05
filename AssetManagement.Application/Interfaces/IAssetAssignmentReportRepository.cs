using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;

namespace AssetManagement.Application.Interfaces;

public interface IAssetAssignmentReportRepository
{
    Task<IEnumerable<AssetAssignmentReportDto>> GetAssetAssignmentReportAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<NonRepairableAssetReportDto>> GetNonRepairableAssetsReportAsync(int? locationId = null, CancellationToken cancellationToken = default);
}