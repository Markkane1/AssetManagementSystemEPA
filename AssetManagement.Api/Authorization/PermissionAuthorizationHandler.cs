using Microsoft.AspNetCore.Authorization;

namespace AssetManagement.Api.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Get permissions from claims
        var permissions = context.User
            .FindAll(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();

        // Check if user has the required permission
        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
