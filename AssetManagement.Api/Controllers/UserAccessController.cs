using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Application.UseCases.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers;

[Route("api/users/{userId}/access")]
[ApiController]
[Authorize]
public class UserAccessController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserAccessController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("locations")]
    public async Task<ActionResult<List<UserLocationAccessDto>>> GetUserLocations(string userId)
    {
        var access = await _mediator.Send(new GetUserLocationAccessQuery(userId));
        return Ok(access);
    }

    [HttpPost("locations")]
    public async Task<IActionResult> SetUserLocations(string userId, [FromBody] List<int> locationIds)
    {
        await _mediator.Send(new SetUserLocationAccessCommand(userId, locationIds));
        return NoContent();
    }
}
