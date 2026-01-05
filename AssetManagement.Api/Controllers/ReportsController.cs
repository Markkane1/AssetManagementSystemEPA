using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("asset-summary/location")]
        public async Task<ActionResult<IEnumerable<AssetSummaryDto>>> GetAssetSummaryByLocation()
        {
            var summary = await _mediator.Send(new GetAssetSummaryByLocationQuery());
            return Ok(summary);
        }

        [HttpGet("asset-summary/category")]
        public async Task<ActionResult<IEnumerable<CategorySummaryDto>>> GetAssetSummaryByCategory()
        {
            var summary = await _mediator.Send(new GetAssetSummaryByCategoryQuery());
            return Ok(summary);
        }

        [HttpGet("assignment-summary/directorate")]
        public async Task<ActionResult<IEnumerable<DirectorateAssignmentSummaryDto>>> GetAssignmentSummaryByDirectorate()
        {
            var summary = await _mediator.Send(new GetAssignmentSummaryByDirectorateQuery());
            return Ok(summary);
        }

        [HttpGet("asset-status")]
        public async Task<ActionResult<IEnumerable<AssetStatusReportDto>>> GetAssetStatusReport()
        {
            var report = await _mediator.Send(new GetAssetStatusReportQuery());
            return Ok(report);
        }

        [HttpGet("maintenance-history/{assetItemId}")]
        public async Task<ActionResult<IEnumerable<MaintenanceRecordDto>>> GetMaintenanceHistoryForAsset(int assetItemId)
        {
            var history = await _mediator.Send(new GetMaintenanceHistoryForAssetQuery(assetItemId));
            return Ok(history);
        }

        [HttpGet("transfer-history/{assetItemId}")]
        public async Task<ActionResult<IEnumerable<TransferHistoryDto>>> GetTransferHistoryForAsset(int assetItemId)
        {
            var history = await _mediator.Send(new GetTransferHistoryForAssetQuery(assetItemId));
            return Ok(history);
        }

        [HttpGet("asset-utilization")]
        public async Task<ActionResult<IEnumerable<UtilizationReportDto>>> GetAssetUtilizationReport()
        {
            var report = await _mediator.Send(new GetAssetUtilizationReportQuery());
            return Ok(report);
        }

        [HttpGet("maintenance-cost")]
        public async Task<ActionResult<IEnumerable<MaintenanceCostSummaryDto>>> GetMaintenanceCostSummary()
        {
            var summary = await _mediator.Send(new GetMaintenanceCostSummaryQuery());
            return Ok(summary);
        }

        [HttpGet("depreciation")]
        public async Task<ActionResult<IEnumerable<DepreciationReportDto>>> GetAssetDepreciationReport()
        {
            var report = await _mediator.Send(new GetAssetDepreciationReportQuery());
            return Ok(report);
        }

        [HttpGet("location-transfer")]
        public async Task<ActionResult<IEnumerable<LocationTransferSummaryDto>>> GetLocationTransferSummary()
        {
            var summary = await _mediator.Send(new GetLocationTransferSummaryQuery());
            return Ok(summary);
        }

        [HttpGet("employee-asset-value")]
        public async Task<ActionResult<IEnumerable<EmployeeAssetValueSummaryDto>>> GetEmployeeAssetValueSummary()
        {
            var summary = await _mediator.Send(new GetEmployeeAssetValueSummaryQuery());
            return Ok(summary);
        }

        [HttpGet("audit-trail/{assetId}")]
        public async Task<ActionResult<IEnumerable<AuditTrailDto>>> GetAuditTrailForAsset(int assetId)
        {
            var auditTrail = await _mediator.Send(new GetAuditTrailForAssetQuery(assetId));
            return Ok(auditTrail);
        }

        [HttpGet("overstock/{threshold}")]
        public async Task<ActionResult<IEnumerable<OverstockReportDto>>> GetOverstockAssetsReport(int threshold)
        {
            var report = await _mediator.Send(new GetOverstockAssetsReportQuery(threshold));
            return Ok(report);
        }

        [HttpGet("maintenance-due")]
        public async Task<ActionResult<IEnumerable<MaintenanceDueReportDto>>> GetAssetMaintenanceDueReport([FromQuery] DateTime dueDate)
        {
            var report = await _mediator.Send(new GetAssetMaintenanceDueReportQuery(dueDate));
            return Ok(report);
        }
        [HttpGet("asset-assignment")]
        public async Task<ActionResult<IEnumerable<AssetAssignmentReportDto>>> GetAssetAssignmentReport()
        {
            var report = await _mediator.Send(new GetAssetAssignmentReportQuery());
            return Ok(report);
        }

        [HttpGet("non-repairable")]
        public async Task<ActionResult<IEnumerable<NonRepairableAssetReportDto>>> GetAllAssetsThatAreNonRepairable([FromQuery] int? locationId = null)
        {
            var report = await _mediator.Send(new GetNonRepairableAssetsReportQuery(locationId));
            return Ok(report);
        }
    }
}