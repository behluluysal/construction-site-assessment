using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ConstructionSite.Services.Authentication.Application.Features.Roles;

public class RevokeClaimCommandHandler(RoleManager<ApplicationRole> roleManager)
    : IRequestHandler<RevokeClaimCommand, Result>
{
    public async Task<Result> Handle(RevokeClaimCommand request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByNameAsync(request.RoleName);
        if (role == null)
            return Result.Failure($"Role '{request.RoleName}' not found.");

        var existingClaims = await roleManager.GetClaimsAsync(role);
        var claimToRemove = existingClaims.FirstOrDefault(c => c.Type == "API.Permission" && c.Value == request.ClaimName);

        if (claimToRemove == null)
        {
            return Result.Failure($"Claim '{request.ClaimName}' not found for role '{request.RoleName}'.");
        }

        var result = await roleManager.RemoveClaimAsync(role, claimToRemove);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure($"Failed to revoke claim '{request.ClaimName}' from role '{request.RoleName}': {errors}");
        }

        return Result.Success();
    }
}

