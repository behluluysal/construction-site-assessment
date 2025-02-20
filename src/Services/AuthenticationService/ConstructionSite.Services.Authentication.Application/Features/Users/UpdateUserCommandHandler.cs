using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Contracts;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Users;

public class UpdateUserHandler(UserManager<ApplicationUser> userManager, ILogger<UpdateUserHandler> logger)
    : IRequestHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing user update: {UserId}", request.Id);

        var user = await userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            logger.LogWarning("User update failed: User '{UserId}' not found.", request.Id);
            return Result.Failure($"User with ID '{request.Id}' not found.");
        }

        var existingUserByUsername = await userManager.FindByNameAsync(request.UserName);
        if (existingUserByUsername != null && existingUserByUsername.Id.ToString() != request.Id)
        {
            logger.LogWarning("User update failed: Username '{UserName}' is already in use.", request.UserName);
            return Result.Failure($"Username '{request.UserName}' is already taken.");
        }

        var existingUserByEmail = await userManager.FindByEmailAsync(request.Email);
        if (existingUserByEmail != null && existingUserByEmail.Id.ToString() != request.Id)
        {
            logger.LogWarning("User update failed: Email '{Email}' is already in use.", request.Email);
            return Result.Failure($"Email '{request.Email}' is already taken.");
        }

        user.UserName = request.UserName;
        user.Email = request.Email;
        user.Name = request.Name;
        user.Surname = request.Surname;

        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            logger.LogInformation("Successfully updated user '{UserId}'.", request.Id);
            return Result.Success();
        }

        logger.LogError("Failed to update user '{UserId}': {Errors}",
            request.Id, string.Join(", ", result.Errors.Select(e => e.Description)));
        return Result.Failure(result.Errors.Select(e => e.Description).ToArray());
    }
}