using ConstructionSite.Services.Authentication.Application.DTOs;
using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Roles;

public class GetRoleByIdQueryHandler(RoleManager<ApplicationRole> roleManager, ILogger<GetRoleByIdQueryHandler> logger)
    : IRequestHandler<GetRoleByIdQuery, Result<RoleDto>>
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ILogger<GetRoleByIdQueryHandler> _logger = logger;

    public async Task<Result<RoleDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
        {
            _logger.LogWarning("Role with ID {Id} not found", request.Id);
            return Result<RoleDto>.FailureResult($"Role with ID {request.Id} not found.");
        }
        var data = new RoleDto(role.Id.ToString(), role.Name ?? "");
        return Result<RoleDto>.SuccessResult(data);
    }
}
