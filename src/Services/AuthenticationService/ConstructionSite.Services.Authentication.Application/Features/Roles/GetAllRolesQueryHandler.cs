using ConstructionSite.Services.Authentication.Application.DTOs;
using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ConstructionSite.Services.Authentication.Application.Features.Roles;

public class GetAllRolesQueryHandler(RoleManager<ApplicationRole> roleManager)
    : IRequestHandler<GetAllRolesQuery, Result<IEnumerable<RoleDto>>>
{
    public async Task<Result<IEnumerable<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles.ToListAsync(cancellationToken);
        var result = roles.Select(r => new RoleDto(r.Id.ToString(), r.Name ?? throw new Exception("Role name was null."))).ToList();
        return Result<IEnumerable<RoleDto>>.SuccessResult(result);
    }
}