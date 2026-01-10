using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Application.UseCases.Auth;
using AssetManagement.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api.Controllers;

[Route("api/roles")]
[ApiController]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IIdentityService _identityService;

    public RolesController(IMediator mediator, IIdentityService identityService)
    {
        _mediator = mediator;
        _identityService = identityService;
    }

    [HttpGet]
    public async Task<ActionResult<List<string>>> GetRoles()
    {
        return Ok(await _identityService.GetRolesAsync());
    }

    [HttpGet("{roleId}/permissions")]
    public async Task<ActionResult<List<RolePermissionDto>>> GetRolePermissions(string roleId)
    {
        var permissions = await _mediator.Send(new GetRolePermissionsQuery(roleId));
        return Ok(permissions);
    }

    [HttpPost("{roleId}/permissions")]
    public async Task<IActionResult> AssignPermission(string roleId, [FromBody] AssignRolePermissionRequest request)
    {
        await _mediator.Send(new AssignRolePermissionCommand(roleId, request.PermissionId));
        return NoContent();
    }

    [HttpDelete("{roleId}/permissions/{permissionId}")]
    public async Task<IActionResult> RevokePermission(string roleId, int permissionId)
    {
        await _mediator.Send(new RevokeRolePermissionCommand(roleId, permissionId));
        return NoContent();
    }

    [HttpPost("users/{userId}/roles")]
    public async Task<IActionResult> AddUserToRole(string userId, [FromBody] string roleName)
    {
        var result = await _identityService.AddToRoleAsync(userId, roleName);
        return result ? NoContent() : BadRequest();
    }

    [HttpDelete("users/{userId}/roles/{roleName}")]
    public async Task<IActionResult> RemoveUserFromRole(string userId, string roleName)
    {
        var result = await _identityService.RemoveFromRoleAsync(userId, roleName);
        return result ? NoContent() : BadRequest();
    }

    [HttpGet("users/{userId}/roles")]
    public async Task<ActionResult<IList<string>>> GetUserRoles(string userId)
    {
        return Ok(await _identityService.GetUserRolesAsync(userId));
    }
}

public record AssignRolePermissionRequest(int PermissionId);
