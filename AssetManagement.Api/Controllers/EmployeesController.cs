using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Api.Constants;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Employees.Read)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var employees = await _mediator.Send(new GetAllEmployeesQuery(userId));
            return Ok(employees);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.Employees.Read)]
        public async Task<ActionResult<EmployeeDto>> GetById(int id)
        {
            var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));
            return Ok(employee);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Employees.Create)]
        public async Task<ActionResult<int>> Create([FromBody] EmployeeDto employeeDto)
        {
            var id = await _mediator.Send(new CreateEmployeeCommand(employeeDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        [Authorize(Policy = Permissions.Employees.Update)]
        public async Task<IActionResult> Update([FromBody] EmployeeDto employeeDto)
        {
            await _mediator.Send(new UpdateEmployeeCommand(employeeDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Employees.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteEmployeeCommand(id));
            return NoContent();
        }
    }
}