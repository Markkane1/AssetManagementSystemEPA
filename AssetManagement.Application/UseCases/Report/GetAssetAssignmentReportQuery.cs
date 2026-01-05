
using AssetManagement.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace AssetManagement.Application.UseCases.Report;

public record GetAssetAssignmentReportQuery : IRequest<IEnumerable<AssetAssignmentReportDto>>;