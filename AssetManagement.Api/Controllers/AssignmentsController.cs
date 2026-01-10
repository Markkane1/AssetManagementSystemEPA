using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Api.Constants;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssignmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Assignments.Read)]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAll()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var assignments = await _mediator.Send(new GetAllAssignmentsQuery(userId));
            return Ok(assignments);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.Assignments.Read)]
        public async Task<ActionResult<AssignmentDto>> GetById(int id)
        {
            var query = new GetAssignmentByIdQuery(id); // Assume this exists
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
            //var assignment = await _mediator.Send(new GetAssignmentByIdQuery(id));
            //return Ok(assignment);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Assignments.Create)]
        public async Task<ActionResult<int>> Create([FromBody] AssignmentDto dto)
        {
            var command = new CreateAssignmentCommand(dto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result }, result);
            //var id = await _mediator.Send(new CreateAssignmentCommand(dto));
            //return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        [Authorize(Policy = Permissions.Assignments.Update)]
        public async Task<IActionResult> Update([FromBody] AssignmentDto assignmentDto)
        {
            await _mediator.Send(new UpdateAssignmentCommand(assignmentDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Assignments.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAssignmentCommand(id));
            return NoContent();
        }
        [HttpPost("return")]
        [Authorize(Policy = Permissions.Assignments.Update)]
        public async Task<IActionResult> ReturnAssetItem([FromBody] ReturnAssetItemRequest request)
        {
            await _mediator.Send(new ReturnAssetItemCommand(request.AssetItemId, request.ReturnDate));
            return NoContent();
        }

        [HttpPost("reassign")]
        [Authorize(Policy = Permissions.Assignments.Update)]
        public async Task<IActionResult> ReassignAssetItem([FromBody] ReassignAssetItemRequest request)
        {
            await _mediator.Send(new ReassignAssetItemCommand(request.AssignmentId, request.NewEmployeeId, request.AssignmentDate));
            return NoContent();
        }

        [HttpGet("by-employee/{employeeId}")]
        [Authorize(Policy = Permissions.Assignments.Read)]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAssignmentsByEmployee(int employeeId)
        {
            var assignments = await _mediator.Send(new GetAssignmentsByEmployeeQuery(employeeId));
            return Ok(assignments);
        }

        [HttpGet("by-asset-item/{assetItemId}")]
        [Authorize(Policy = Permissions.Assignments.Read)]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAssignmentsByAssetItem(int assetItemId)
        {
            var assignments = await _mediator.Send(new GetAssignmentsByAssetItemQuery(assetItemId));
            return Ok(assignments);
        }

        [HttpGet("history/employee/{employeeId}")]
        [Authorize(Policy = Permissions.Assignments.Read)]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAllAssignmentHistoryForEmployee(int employeeId)
        {
            var assignments = await _mediator.Send(new GetAllAssignmentHistoryForEmployeeQuery(employeeId));
            return Ok(assignments);
        }

        [HttpGet("history/asset/{assetItemId}")]
        [Authorize(Policy = Permissions.Assignments.Read)]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAssignmentHistoryForAsset(int assetItemId)
        {
            var assignments = await _mediator.Send(new GetAssignmentHistoryForAssetQuery(assetItemId));
            return Ok(assignments);
        }

        [HttpGet("by-date-range")]
        [Authorize(Policy = Permissions.Assignments.Read)]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAssignmentsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var assignments = await _mediator.Send(new GetAssignmentsByDateRangeQuery(startDate, endDate));
            return Ok(assignments);
        }

        [HttpGet("overdue")]
        [Authorize(Policy = Permissions.Assignments.Read)]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetOverdueAssignments()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var assignments = await _mediator.Send(new GetOverdueAssignmentsQuery(userId));
            return Ok(assignments);
        }
    }

    public record ReturnAssetItemRequest(int AssetItemId, DateTime ReturnDate);
    public record ReassignAssetItemRequest(int AssignmentId, int NewEmployeeId, DateTime AssignmentDate);
}