using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Api.Constants;
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
        [Authorize(Policy = Permissions.Projects.Read)]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            return Ok(projects);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.Projects.Read)]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var project = await _mediator.Send(new GetProjectByIdQuery(id));
            return Ok(project);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Projects.Create)]
        public async Task<ActionResult<int>> Create([FromBody] ProjectDto projectDto)
        {
            var id = await _mediator.Send(new CreateProjectCommand(projectDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        [Authorize(Policy = Permissions.Projects.Update)]
        public async Task<IActionResult> Update([FromBody] ProjectDto projectDto)
        {
            await _mediator.Send(new UpdateProjectCommand(projectDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Projects.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProjectCommand(id));
            return NoContent();
        }
    }
}