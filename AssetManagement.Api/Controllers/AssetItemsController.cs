using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.AssetItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using AssetManagement.Application.UseCases.Assignment;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssetItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetItemDto>>> GetAll([FromQuery] string userRole, [FromQuery] int? userLocationId)
        {
            var assetItems = await _mediator.Send(new GetAllAssetItemsQuery(userRole, userLocationId));
            return Ok(assetItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssetItemDto>> GetById(int id)
        {
            var assetItem = await _mediator.Send(new GetAssetItemByIdQuery(id));
            return Ok(assetItem);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] AssetItemDto assetItemDto)
        {
            var id = await _mediator.Send(new CreateAssetItemCommand(assetItemDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AssetItemDto assetItemDto)
        {
            await _mediator.Send(new UpdateAssetItemCommand(assetItemDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAssetItemCommand(id));
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("transfer")]
        public async Task<IActionResult> TransferAssets([FromBody] TransferAssetsRequest request)
        {
            await _mediator.Send(new TransferAssetsCommand(request.AssetItemIds, request.TargetLocationId));
            return NoContent();
        }

        [HttpGet("unassigned")]
        public async Task<ActionResult<IEnumerable<AssetItemDto>>> GetUnassignedAssetItems()
        {
            var assetItems = await _mediator.Send(new GetUnassignedAssetItemsQuery());
            return Ok(assetItems);
        }
    }
    public record TransferAssetsRequest(int[] AssetItemIds, int TargetLocationId);
}