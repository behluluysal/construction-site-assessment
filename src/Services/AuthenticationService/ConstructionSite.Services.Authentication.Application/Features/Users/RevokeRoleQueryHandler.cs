using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
namespace ConstructionSite.Services.Authentication.Application.Features.Users;

public class RevokeRoleQueryHandler(UserManager<ApplicationUser> userManager, ILogger<RevokeRoleQueryHandler> logger)
    : IRequestHandler<RevokeRoleCommand, Result>
{
    public async Task<Result> Handle(RevokeRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing role revocation: UserId = {UserId}, Role = {RoleName}", request.UserId, request.RoleName);

        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            logger.LogWarning("Role revocation failed: User '{UserId}' not found.", request.UserId);
            return Result.Failure($"User with ID '{request.UserId}' not found.");
        }

        var result = await userManager.RemoveFromRoleAsync(user, request.RoleName);
        if (result.Succeeded)
        {
            logger.LogInformation("Successfully revoked role '{RoleName}' from user '{UserId}'.", request.RoleName, request.UserId);
            return Result.Success();
        }

        logger.LogError("Failed to revoke role '{RoleName}' from user '{UserId}': {Errors}",
            request.RoleName, request.UserId, string.Join(", ", result.Errors.Select(e => e.Description)));
        return Result.Failure(result.Errors.Select(e => e.Description).ToArray());
    }
}