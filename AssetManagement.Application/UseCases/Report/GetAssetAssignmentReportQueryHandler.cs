using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.DTOs;

namespace AssetManagement.Application.UseCases.Report;

public class GetAssetAssignmentReportQueryHandler : IRequestHandler<GetAssetAssignmentReportQuery, IEnumerable<AssetAssignmentReportDto>>
{
    private readonly IAssetAssignmentReportRepository _repository;

    public GetAssetAssignmentReportQueryHandler(IAssetAssignmentReportRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AssetAssignmentReportDto>> Handle(GetAssetAssignmentReportQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAssetAssignmentReportAsync(cancellationToken);
    }
}