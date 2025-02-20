namespace ConstructionSite.Web.Security;

using ConstructionSite.Shared;
using ConstructionSite.Web;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {

        if (context.User.HasClaim(c => c.Type == "unique_name"))
        {
            if (context.User.HasClaim(c => c.Type == requirement.Type && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}