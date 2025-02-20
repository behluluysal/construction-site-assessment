using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Users;

public class CreateUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<CreateUserCommandHandler> logger)
    : IRequestHandler<CreateUserCommand, Result<Ulid>>
{
    public async Task<Result<Ulid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing user creation: {UserName}", request.UserName);

        if (await userManager.FindByNameAsync(request.UserName) != null)
        {
            logger.LogWarning("User creation failed: User '{UserName}' already exists.", request.UserName);
            return Result<Ulid>.FailureResult($"User '{request.UserName}' already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            logger.LogInformation("Successfully created user '{UserName}' with ID {UserId}.", user.UserName, user.Id);
            return Result<Ulid>.SuccessResult(user.Id);
        }

        logger.LogError("Failed to create user '{UserName}': {Errors}",
            user.UserName, string.Join(", ", result.Errors.Select(e => e.Description)));
        return Result<Ulid>.FailureResult(result.Errors.Select(e => e.Description).ToArray());
    }
}