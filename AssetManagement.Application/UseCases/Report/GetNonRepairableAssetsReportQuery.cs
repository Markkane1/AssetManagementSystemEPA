using System.Collections.Generic;
using MediatR;
using AssetManagement.Application.DTOs;

namespace AssetManagement.Application.UseCases.Report;

public record GetNonRepairableAssetsReportQuery(int? LocationId = null) : IRequest<IEnumerable<NonRepairableAssetReportDto>>;