using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Roles;

public class DeleteRoleCommandHandler(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<DeleteRoleCommandHandler> logger)
    : IRequestHandler<DeleteRoleCommand, Result>
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<DeleteRoleCommandHandler> _logger = logger;

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing role deletion: {RoleName}", request.RoleName);

        var role = await _roleManager.FindByNameAsync(request.RoleName);
        if (role == null)
        {
            _logger.LogWarning("Role deletion failed: {RoleName} does not exist.", request.RoleName);
            return Result.Failure($"Role '{request.RoleName}' not found.");
        }

        // Check if any users are assigned to this role
        var usersInRole = await _userManager.GetUsersInRoleAsync(request.RoleName);
        if (usersInRole.Any())
        {
            _logger.LogWarning("Attempted to delete role '{RoleName}' but it is assigned to users.", request.RoleName);
            return Result.Failure($"Cannot delete role '{request.RoleName}' because it is assigned to users.");
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to delete role '{RoleName}': {Errors}", request.RoleName, errors);
            return Result.Failure($"Failed to delete role '{request.RoleName}': {errors}");
        }

        _logger.LogInformation("Deleted role: {RoleName}", request.RoleName);
        return Result.Success();
    }
}