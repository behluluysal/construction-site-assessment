using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Users;

public class AssignRoleCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<AssignRoleCommandHandler> logger)
    : IRequestHandler<AssignRoleCommand, Result>
{
    public async Task<Result> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing role assignment: UserId = {UserId}, Role = {RoleName}", request.UserId, request.RoleName);

        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            logger.LogWarning("Role assignment failed: User {UserId} not found.", request.UserId);
            return Result.Failure($"User with ID '{request.UserId}' not found.");
        }

        if (!await roleManager.RoleExistsAsync(request.RoleName))
        {
            logger.LogWarning("Role assignment failed: Role {RoleName} does not exist.", request.RoleName);
            return Result.Failure($"Role '{request.RoleName}' does not exist.");
        }

        var result = await userManager.AddToRoleAsync(user, request.RoleName);
        if (result.Succeeded)
        {
            logger.LogInformation("Successfully assigned role '{RoleName}' to user '{UserId}'.", request.RoleName, request.UserId);
            return Result.Success();
        }

        logger.LogError("Failed to assign role '{RoleName}' to user '{UserId}': {Errors}",
            request.RoleName, request.UserId, string.Join(", ", result.Errors.Select(e => e.Description)));
        return Result.Failure(result.Errors.Select(e => e.Description).ToArray());
    }
}
