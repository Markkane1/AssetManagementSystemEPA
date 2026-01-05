using AssetManagement.Api.Constants;
using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Application.UseCases.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UserPermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserPermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userId}/permissions")]
    [Authorize(Policy = Permissions.UserManagement.ManagePermissions)]
    public async Task<ActionResult<List<UserPermissionDto>>> GetUserPermissions(string userId)
    {
        var permissions = await _mediator.Send(new GetUserPermissionsQuery(userId));
        return Ok(permissions);
    }

    [HttpPost("{userId}/permissions")]
    [Authorize(Policy = Permissions.UserManagement.ManagePermissions)]
    public async Task<IActionResult> AssignPermission(string userId, [FromBody] AssignPermissionRequest request)
    {
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();

        await _mediator.Send(new AssignPermissionCommand(userId, request.PermissionId, currentUserId));
        return NoContent();
    }

    [HttpDelete("{userId}/permissions/{permissionId}")]
    [Authorize(Policy = Permissions.UserManagement.ManagePermissions)]
    public async Task<IActionResult> RevokePermission(string userId, int permissionId)
    {
        await _mediator.Send(new RevokePermissionCommand(userId, permissionId));
        return NoContent();
    }
}

public record AssignPermissionRequest(int PermissionId);
