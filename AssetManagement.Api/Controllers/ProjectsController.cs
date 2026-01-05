using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var project = await _mediator.Send(new GetProjectByIdQuery(id));
            return Ok(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] ProjectDto projectDto)
        {
            var id = await _mediator.Send(new CreateProjectCommand(projectDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProjectDto projectDto)
        {
            await _mediator.Send(new UpdateProjectCommand(projectDto));
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProjectCommand(id));
            return NoContent();
        }
    }
}