using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Api.Constants;
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
        [Authorize(Policy = Permissions.AssetItems.Read)]
        public async Task<ActionResult<IEnumerable<AssetItemDto>>> GetAll()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var assetItems = await _mediator.Send(new GetAllAssetItemsQuery(userId));
            return Ok(assetItems);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.AssetItems.Read)]
        public async Task<ActionResult<AssetItemDto>> GetById(int id)
        {
            var assetItem = await _mediator.Send(new GetAssetItemByIdQuery(id));
            return Ok(assetItem);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.AssetItems.Create)]
        public async Task<ActionResult<int>> Create([FromBody] AssetItemDto assetItemDto)
        {
            var id = await _mediator.Send(new CreateAssetItemCommand(assetItemDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        [Authorize(Policy = Permissions.AssetItems.Update)]
        public async Task<IActionResult> Update([FromBody] AssetItemDto assetItemDto)
        {
            await _mediator.Send(new UpdateAssetItemCommand(assetItemDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.AssetItems.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAssetItemCommand(id));
            return NoContent();
        }

        [HttpPost("transfer")]
        [Authorize(Policy = Permissions.AssetItems.Transfer)]
        public async Task<IActionResult> TransferAssets([FromBody] TransferAssetsRequest request)
        {
            await _mediator.Send(new TransferAssetsCommand(request.AssetItemIds, request.TargetLocationId));
            return NoContent();
        }

        [HttpGet("unassigned")]
        [Authorize(Policy = Permissions.AssetItems.Read)]
        public async Task<ActionResult<IEnumerable<AssetItemDto>>> GetUnassignedAssetItems()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var assetItems = await _mediator.Send(new GetUnassignedAssetItemsQuery(userId));
            return Ok(assetItems);
        }
    }
    public record TransferAssetsRequest(int[] AssetItemIds, int TargetLocationId);
}