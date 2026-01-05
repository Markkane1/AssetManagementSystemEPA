using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.DTOs;

namespace AssetManagement.Application.UseCases.Report;

public class GetNonRepairableAssetsReportQueryHandler : IRequestHandler<GetNonRepairableAssetsReportQuery, IEnumerable<NonRepairableAssetReportDto>>
{
    private readonly IAssetAssignmentReportRepository _repository;

    public GetNonRepairableAssetsReportQueryHandler(IAssetAssignmentReportRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<NonRepairableAssetReportDto>> Handle(GetNonRepairableAssetsReportQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetNonRepairableAssetsReportAsync(request.LocationId, cancellationToken);
    }
}