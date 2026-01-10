using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Api.Constants;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.UseCases.Asset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AssetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Assets.Read)]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAll()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var assets = await _mediator.Send(new GetAllAssetsQuery(userId));
            return Ok(assets);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.Assets.Read)]
        public async Task<ActionResult<AssetDto>> GetById(int id)
        {
            var asset = await _mediator.Send(new GetAssetByIdQuery(id));
            return Ok(asset);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Assets.Create)]
        public async Task<ActionResult<int>> Create([FromBody] AssetDto assetDto)
        {
            var id = await _mediator.Send(new CreateAssetCommand(assetDto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        [Authorize(Policy = Permissions.Assets.Update)]
        public async Task<IActionResult> Update([FromBody] AssetDto assetDto)
        {
            await _mediator.Send(new UpdateAssetCommand(assetDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Assets.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAssetCommand(id));
            return NoContent();
        }

        [HttpGet("by-location/{locationId}")]
        [Authorize(Policy = Permissions.Assets.Read)]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssetsByLocation(int locationId)
        {
            var assets = await _mediator.Send(new GetAssetsByLocationQuery(locationId));
            return Ok(assets);
        }

        [HttpGet("by-category/{categoryId}")]
        [Authorize(Policy = Permissions.Assets.Read)]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssetsByCategory(int categoryId)
        {
            var assets = await _mediator.Send(new GetAssetsByCategoryQuery(categoryId));
            return Ok(assets);
        }

        [HttpGet("by-source/{sourceId}/{sourceType}")]
        [Authorize(Policy = Permissions.Assets.Read)]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssetsBySource(int sourceId, string sourceType)
        {
            var assets = await _mediator.Send(new GetAssetsBySourceQuery(sourceId, sourceType));
            return Ok(assets);
        }

        [HttpGet("by-vendor/{vendorId}")]
        [Authorize(Policy = Permissions.Assets.Read)]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssetsByVendor(int vendorId)
        {
            var assets = await _mediator.Send(new GetAssetsByVendorQuery(vendorId));
            return Ok(assets);
        }

        [HttpGet("by-acquisition-date")]
        [Authorize(Policy = Permissions.Assets.Read)]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssetsByAcquisitionDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var assets = await _mediator.Send(new GetAssetsByAcquisitionDateRangeQuery(startDate, endDate));
            return Ok(assets);
        }

        [HttpGet("low-stock/{threshold}")]
        [Authorize(Policy = Permissions.Assets.Read)]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetLowStockAssets(int threshold)
        {
            var assets = await _mediator.Send(new GetLowStockAssetsQuery(threshold));
            return Ok(assets);
        }
    }
}