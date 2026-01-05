using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.Directorate;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectoratesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DirectoratesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DirectorateDto>>> GetAll()
        {
            var directorates = await _mediator.Send(new GetAllDirectoratesQuery());
            return Ok(directorates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DirectorateDto>> GetById(int id)
        {
            var directorate = await _mediator.Send(new GetDirectorateByIdQuery(id));
            return Ok(directorate);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] DirectorateDto directorateDto)
        {
            var id = await _mediator.Send(new CreateDirectorateCommand(directorateDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DirectorateDto directorateDto)
        {
            await _mediator.Send(new UpdateDirectorateCommand(directorateDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteDirectorateCommand(id));
            return NoContent();
        }
    }
}