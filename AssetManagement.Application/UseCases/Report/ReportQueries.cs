using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Report
{
    public class GetAssetSummaryByLocationQuery : IRequest<IEnumerable<AssetSummaryDto>>;
    public class GetAssetSummaryByCategoryQuery : IRequest<IEnumerable<CategorySummaryDto>>;
    public class GetAssignmentSummaryByDirectorateQuery : IRequest<IEnumerable<DirectorateAssignmentSummaryDto>>;
    public class GetAssetStatusReportQuery : IRequest<IEnumerable<AssetStatusReportDto>>;
    public class GetMaintenanceHistoryForAssetQuery : IRequest<IEnumerable<MaintenanceRecordDto>>
    {
        public int AssetItemId { get; }
        public GetMaintenanceHistoryForAssetQuery(int assetItemId)
        {
            AssetItemId = assetItemId;
        }
    }
    public class GetTransferHistoryForAssetQuery : IRequest<IEnumerable<TransferHistoryDto>>
    {
        public int AssetItemId { get; }
        public GetTransferHistoryForAssetQuery(int assetItemId)
        {
            AssetItemId = assetItemId;
        }
    }
    public class GetAssetUtilizationReportQuery : IRequest<IEnumerable<UtilizationReportDto>>;
    public class GetMaintenanceCostSummaryQuery : IRequest<IEnumerable<MaintenanceCostSummaryDto>>;
    public class GetAssetDepreciationReportQuery : IRequest<IEnumerable<DepreciationReportDto>>;
    public class GetLocationTransferSummaryQuery : IRequest<IEnumerable<LocationTransferSummaryDto>>;
    public class GetEmployeeAssetValueSummaryQuery : IRequest<IEnumerable<EmployeeAssetValueSummaryDto>>;
    public class GetAuditTrailForAssetQuery : IRequest<IEnumerable<AuditTrailDto>>
    {
        public int AssetId { get; }
        public GetAuditTrailForAssetQuery(int assetId)
        {
            AssetId = assetId;
        }
    }
    public class GetOverstockAssetsReportQuery : IRequest<IEnumerable<OverstockReportDto>>
    {
        public int Threshold { get; }
        public GetOverstockAssetsReportQuery(int threshold)
        {
            Threshold = threshold;
        }
    }
    public class GetAssetMaintenanceDueReportQuery : IRequest<IEnumerable<MaintenanceDueReportDto>>
    {
        public DateTime DueDate { get; }
        public GetAssetMaintenanceDueReportQuery(DateTime dueDate)
        {
            DueDate = dueDate;
        }
    }

    public class ReportQueryHandler :
        IRequestHandler<GetAssetSummaryByLocationQuery, IEnumerable<AssetSummaryDto>>,
        IRequestHandler<GetAssetSummaryByCategoryQuery, IEnumerable<CategorySummaryDto>>,
        IRequestHandler<GetAssignmentSummaryByDirectorateQuery, IEnumerable<DirectorateAssignmentSummaryDto>>,
        IRequestHandler<GetAssetStatusReportQuery, IEnumerable<AssetStatusReportDto>>,
        IRequestHandler<GetMaintenanceHistoryForAssetQuery, IEnumerable<MaintenanceRecordDto>>,
        IRequestHandler<GetTransferHistoryForAssetQuery, IEnumerable<TransferHistoryDto>>,
        IRequestHandler<GetAssetUtilizationReportQuery, IEnumerable<UtilizationReportDto>>,
        IRequestHandler<GetMaintenanceCostSummaryQuery, IEnumerable<MaintenanceCostSummaryDto>>,
        IRequestHandler<GetAssetDepreciationReportQuery, IEnumerable<DepreciationReportDto>>,
        IRequestHandler<GetLocationTransferSummaryQuery, IEnumerable<LocationTransferSummaryDto>>,
        IRequestHandler<GetEmployeeAssetValueSummaryQuery, IEnumerable<EmployeeAssetValueSummaryDto>>,
        IRequestHandler<GetAuditTrailForAssetQuery, IEnumerable<AuditTrailDto>>,
        IRequestHandler<GetOverstockAssetsReportQuery, IEnumerable<OverstockReportDto>>,
        IRequestHandler<GetAssetMaintenanceDueReportQuery, IEnumerable<MaintenanceDueReportDto>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public ReportQueryHandler(IAssetRepository assetRepository, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _assetRepository = assetRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AssetSummaryDto>> Handle(GetAssetSummaryByLocationQuery request, CancellationToken cancellationToken)
        {
            return await _assetRepository.GetAssetSummaryByLocationAsync();
        }

        public async Task<IEnumerable<CategorySummaryDto>> Handle(GetAssetSummaryByCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _assetRepository.GetAssetSummaryByCategoryAsync();
        }

        public async Task<IEnumerable<DirectorateAssignmentSummaryDto>> Handle(GetAssignmentSummaryByDirectorateQuery request, CancellationToken cancellationToken)
        {
            return await _employeeRepository.GetAssignmentSummaryByDirectorateAsync();
        }

        public async Task<IEnumerable<AssetStatusReportDto>> Handle(GetAssetStatusReportQuery request, CancellationToken cancellationToken)
        {
            return await _assetRepository.GetAssetStatusReportAsync();
        }

        public async Task<IEnumerable<MaintenanceRecordDto>> Handle(GetMaintenanceHistoryForAssetQuery request, CancellationToken cancellationToken)
        {
            var maintenanceRecords = await _assetRepository.GetMaintenanceHistoryForAssetAsync(request.AssetItemId);
            return _mapper.Map<IEnumerable<MaintenanceRecordDto>>(maintenanceRecords);
        }

        public async Task<IEnumerable<TransferHistoryDto>> Handle(GetTransferHistoryForAssetQuery request, CancellationToken cancellationToken)
        {
            var transferHistory = await _assetRepository.GetTransferHistoryForAssetAsync(request.AssetItemId);
            return _mapper.Map<IEnumerable<TransferHistoryDto>>(transferHistory);
        }

        public async Task<IEnumerable<UtilizationReportDto>> Handle(GetAssetUtilizationReportQuery request, CancellationToken cancellationToken)
        {
            return await _assetRepository.GetAssetUtilizationReportAsync();
        }

        public async Task<IEnumerable<MaintenanceCostSummaryDto>> Handle(GetMaintenanceCostSummaryQuery request, CancellationToken cancellationToken)
        {
            return await _assetRepository.GetMaintenanceCostSummaryAsync();
        }

        public async Task<IEnumerable<DepreciationReportDto>> Handle(GetAssetDepreciationReportQuery request, CancellationToken cancellationToken)
        {
            return await _assetRepository.GetAssetDepreciationReportAsync();
        }

        public async Task<IEnumerable<LocationTransferSummaryDto>> Handle(GetLocationTransferSummaryQuery request, CancellationToken cancellationToken)
        {
            return await _assetRepository.GetLocationTransferSummaryAsync();
        }

        public async Task<IEnumerable<EmployeeAssetValueSummaryDto>> Handle(GetEmployeeAssetValueSummaryQuery request, CancellationToken cancellationToken)
        {
            return await _employeeRepository.GetEmployeeAssetValueSummaryAsync();
        }

        public async Task<IEnumerable<AuditTrailDto>> Handle(GetAuditTrailForAssetQuery request, CancellationToken cancellationToken)
        {
            return await _assetRepository.GetAuditTrailForAssetAsync(request.AssetId);
        }

        public async Task<IEnumerable<OverstockReportDto>> Handle(GetOverstockAssetsReportQuery request, CancellationToken cancellationToken)
        {
            return await _assetRepository.GetOverstockAssetsReportAsync(request.Threshold);
        }

        public async Task<IEnumerable<MaintenanceDueReportDto>> Handle(GetAssetMaintenanceDueReportQuery request, CancellationToken cancellationToken)
        {
            return await _assetRepository.GetAssetMaintenanceDueReportAsync(request.DueDate);
        }
    }
}