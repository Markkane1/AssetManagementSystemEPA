using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.Vendor;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendorDto>>> GetAll()
        {
            var vendors = await _mediator.Send(new GetAllVendorsQuery());
            return Ok(vendors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VendorDto>> GetById(int id)
        {
            var vendor = await _mediator.Send(new GetVendorByIdQuery(id));
            return Ok(vendor);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] VendorDto vendorDto)
        {
            var id = await _mediator.Send(new CreateVendorCommand(vendorDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] VendorDto vendorDto)
        {
            await _mediator.Send(new UpdateVendorCommand(vendorDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteVendorCommand(id));
            return NoContent();
        }
    }
}