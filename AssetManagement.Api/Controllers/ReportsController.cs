using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Api.Constants;
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
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<AssetSummaryDto>>> GetAssetSummaryByLocation()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var summary = await _mediator.Send(new GetAssetSummaryByLocationQuery(userId));
            return Ok(summary);
        }

        [HttpGet("asset-summary/category")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<CategorySummaryDto>>> GetAssetSummaryByCategory()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var summary = await _mediator.Send(new GetAssetSummaryByCategoryQuery(userId));
            return Ok(summary);
        }

        [HttpGet("assignment-summary/directorate")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<DirectorateAssignmentSummaryDto>>> GetAssignmentSummaryByDirectorate()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var summary = await _mediator.Send(new GetAssignmentSummaryByDirectorateQuery(userId));
            return Ok(summary);
        }

        [HttpGet("asset-status")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<AssetStatusReportDto>>> GetAssetStatusReport()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var report = await _mediator.Send(new GetAssetStatusReportQuery(userId));
            return Ok(report);
        }

        [HttpGet("maintenance-history/{assetItemId}")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<MaintenanceRecordDto>>> GetMaintenanceHistoryForAsset(int assetItemId)
        {
            var history = await _mediator.Send(new GetMaintenanceHistoryForAssetQuery(assetItemId));
            return Ok(history);
        }

        [HttpGet("transfer-history/{assetItemId}")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<TransferHistoryDto>>> GetTransferHistoryForAsset(int assetItemId)
        {
            var history = await _mediator.Send(new GetTransferHistoryForAssetQuery(assetItemId));
            return Ok(history);
        }

        [HttpGet("asset-utilization")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<UtilizationReportDto>>> GetAssetUtilizationReport()
        {
            var report = await _mediator.Send(new GetAssetUtilizationReportQuery());
            return Ok(report);
        }

        [HttpGet("maintenance-cost")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<MaintenanceCostSummaryDto>>> GetMaintenanceCostSummary()
        {
            var summary = await _mediator.Send(new GetMaintenanceCostSummaryQuery());
            return Ok(summary);
        }

        [HttpGet("depreciation")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<DepreciationReportDto>>> GetAssetDepreciationReport()
        {
            var report = await _mediator.Send(new GetAssetDepreciationReportQuery());
            return Ok(report);
        }

        [HttpGet("location-transfer")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<LocationTransferSummaryDto>>> GetLocationTransferSummary()
        {
            var summary = await _mediator.Send(new GetLocationTransferSummaryQuery());
            return Ok(summary);
        }

        [HttpGet("employee-asset-value")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<EmployeeAssetValueSummaryDto>>> GetEmployeeAssetValueSummary()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var summary = await _mediator.Send(new GetEmployeeAssetValueSummaryQuery(userId));
            return Ok(summary);
        }

        [HttpGet("audit-trail/{assetId}")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<AuditTrailDto>>> GetAuditTrailForAsset(int assetId)
        {
            var auditTrail = await _mediator.Send(new GetAuditTrailForAssetQuery(assetId));
            return Ok(auditTrail);
        }

        [HttpGet("overstock/{threshold}")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<OverstockReportDto>>> GetOverstockAssetsReport(int threshold)
        {
            var report = await _mediator.Send(new GetOverstockAssetsReportQuery(threshold));
            return Ok(report);
        }

        [HttpGet("maintenance-due")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<MaintenanceDueReportDto>>> GetAssetMaintenanceDueReport([FromQuery] DateTime dueDate)
        {
            var report = await _mediator.Send(new GetAssetMaintenanceDueReportQuery(dueDate));
            return Ok(report);
        }
        [HttpGet("asset-assignment")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<AssetAssignmentReportDto>>> GetAssetAssignmentReport()
        {
            var report = await _mediator.Send(new GetAssetAssignmentReportQuery());
            return Ok(report);
        }

        [HttpGet("non-repairable")]
        [Authorize(Policy = Permissions.Reports.View)]
        public async Task<ActionResult<IEnumerable<NonRepairableAssetReportDto>>> GetAllAssetsThatAreNonRepairable([FromQuery] int? locationId = null)
        {
            var report = await _mediator.Send(new GetNonRepairableAssetsReportQuery(locationId));
            return Ok(report);
        }
    }
}