using AssetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AssetManagement.Api.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IIdentityService _identityService;

    public PermissionAuthorizationHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return;
        }

        // Fetch permissions dynamically from the database via IdentityService
        var permissions = await _identityService.GetUserPermissionsAsync(userId);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}
