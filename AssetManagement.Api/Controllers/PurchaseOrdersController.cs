using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Api.Constants;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.PurchaseOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchaseOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.PurchaseOrders.Read)]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> GetAll()
        {
            var purchaseOrders = await _mediator.Send(new GetAllPurchaseOrdersQuery());
            return Ok(purchaseOrders);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.PurchaseOrders.Read)]
        public async Task<ActionResult<PurchaseOrderDto>> GetById(int id)
        {
            var purchaseOrder = await _mediator.Send(new GetPurchaseOrderByIdQuery(id));
            return Ok(purchaseOrder);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.PurchaseOrders.Create)]
        public async Task<ActionResult<int>> Create([FromBody] PurchaseOrderDto purchaseOrderDto)
        {
            var id = await _mediator.Send(new CreatePurchaseOrderCommand(purchaseOrderDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        [Authorize(Policy = Permissions.PurchaseOrders.Update)]
        public async Task<IActionResult> Update([FromBody] PurchaseOrderDto purchaseOrderDto)
        {
            await _mediator.Send(new UpdatePurchaseOrderCommand(purchaseOrderDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.PurchaseOrders.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeletePurchaseOrderCommand(id));
            return NoContent();
        }
    }
}