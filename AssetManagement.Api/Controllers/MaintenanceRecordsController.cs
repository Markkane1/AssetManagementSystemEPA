using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.MaintenanceRecord;
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
        public async Task<ActionResult<IEnumerable<MaintenanceRecordDto>>> GetAll()
        {
            var maintenanceRecords = await _mediator.Send(new GetAllMaintenanceRecordsQuery());
            return Ok(maintenanceRecords);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceRecordDto>> GetById(int id)
        {
            var maintenanceRecord = await _mediator.Send(new GetMaintenanceRecordByIdQuery(id));
            return Ok(maintenanceRecord);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] MaintenanceRecordDto maintenanceRecordDto)
        {
            var id = await _mediator.Send(new CreateMaintenanceRecordCommand(maintenanceRecordDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MaintenanceRecordDto maintenanceRecordDto)
        {
            await _mediator.Send(new UpdateMaintenanceRecordCommand(maintenanceRecordDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteMaintenanceRecordCommand(id));
            return NoContent();
        }
    }
}