using ConstructionSite.Services.Authentication.Application.DTOs;
using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Users;

public class GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager, ILogger<GetUserByIdQueryHandler> logger)
    : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching user details for ID: {UserId}", request.Id);

        var user = await userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            logger.LogWarning("User not found: {UserId}", request.Id);
            return Result<UserDto>.FailureResult($"User with ID '{request.Id}' not found.");
        }

        var userDto = new UserDto(
            user.Id,
            user.UserName ?? throw new Exception("Username was null."),
            user.Email ?? throw new Exception("Email was null."),
            user.Name,
            user.Surname
        );

        logger.LogInformation("Successfully retrieved user '{UserId}'", request.Id);
        return Result<UserDto>.SuccessResult(userDto);
    }
}
