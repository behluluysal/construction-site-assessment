using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Users;

public class DeleteUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<DeleteUserCommandHandler> logger)
    : IRequestHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing user deletion: {UserId}", request.Id);

        var user = await userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            logger.LogWarning("User deletion failed: User '{UserId}' not found.", request.Id);
            return Result.Failure($"User with ID '{request.Id}' not found.");
        }

        var result = await userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            logger.LogInformation("Successfully deleted user '{UserId}'.", request.Id);
            return Result.Success();
        }

        logger.LogError("Failed to delete user '{UserId}': {Errors}",
            request.Id, string.Join(", ", result.Errors.Select(e => e.Description)));
        return Result.Failure(result.Errors.Select(e => e.Description).ToArray());
    }
}