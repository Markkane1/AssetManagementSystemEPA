using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Api.Constants;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.MaintenanceRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaintenanceRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Maintenance.Read)]
        public async Task<ActionResult<IEnumerable<MaintenanceRecordDto>>> GetAll()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var maintenanceRecords = await _mediator.Send(new GetAllMaintenanceRecordsQuery(userId));
            return Ok(maintenanceRecords);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.Maintenance.Read)]
        public async Task<ActionResult<MaintenanceRecordDto>> GetById(int id)
        {
            var maintenanceRecord = await _mediator.Send(new GetMaintenanceRecordByIdQuery(id));
            return Ok(maintenanceRecord);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Maintenance.Create)]
        public async Task<ActionResult<int>> Create([FromBody] MaintenanceRecordDto maintenanceRecordDto)
        {
            var id = await _mediator.Send(new CreateMaintenanceRecordCommand(maintenanceRecordDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        [Authorize(Policy = Permissions.Maintenance.Update)]
        public async Task<IActionResult> Update([FromBody] MaintenanceRecordDto maintenanceRecordDto)
        {
            await _mediator.Send(new UpdateMaintenanceRecordCommand(maintenanceRecordDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Maintenance.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteMaintenanceRecordCommand(id));
            return NoContent();
        }
    }
}