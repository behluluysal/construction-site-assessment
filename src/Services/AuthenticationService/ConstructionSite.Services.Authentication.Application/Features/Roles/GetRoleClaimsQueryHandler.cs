using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ConstructionSite.Services.Authentication.Application.Features.Roles;

public class GetRoleClaimsQueryHandler(RoleManager<ApplicationRole> roleManager)
    : IRequestHandler<GetRoleClaimsQuery, Result<List<string>>>
{
    public async Task<Result<List<string>>> Handle(GetRoleClaimsQuery request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByNameAsync(request.RoleName);
        if (role == null)
        {
            return Result<List<string>>.FailureResult($"Role '{request.RoleName}' not found.");
        }

        var claims = await roleManager.GetClaimsAsync(role);
        var claimValues = claims.Select(c => c.Value).ToList();
        return Result<List<string>>.SuccessResult(claimValues);
    }
}
