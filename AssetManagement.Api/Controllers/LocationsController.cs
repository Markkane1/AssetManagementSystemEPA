using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.Location;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetAll()
        {
            var locations = await _mediator.Send(new GetAllLocationsQuery());
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDto>> GetById(int id)
        {
            var location = await _mediator.Send(new GetLocationByIdQuery(id));
            return Ok(location);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] LocationDto locationDto)
        {
            var id = await _mediator.Send(new CreateLocationCommand(locationDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LocationDto locationDto)
        {
            await _mediator.Send(new UpdateLocationCommand(locationDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteLocationCommand(id));
            return NoContent();
        }
    }
}