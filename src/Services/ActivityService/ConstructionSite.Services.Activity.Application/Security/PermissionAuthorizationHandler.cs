using ConstructionSite.Shared;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ConstructionSite.Services.Activity.Application.Security;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var userPermissions = context.User.FindAll("API.Permission").Select(c => c.Value);

        if (context.User.HasClaim(c => c.Type == ClaimTypes.Name))
        {
            if (context.User.HasClaim(c => c.Type == requirement.Type && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
