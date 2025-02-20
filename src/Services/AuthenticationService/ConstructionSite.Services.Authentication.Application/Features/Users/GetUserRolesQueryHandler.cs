using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Users;

public class GetUserRolesQueryHandler(UserManager<ApplicationUser> userManager, ILogger<GetUserRolesQueryHandler> logger)
    : IRequestHandler<GetUserRolesQuery, Result<List<string>>>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<GetUserRolesQueryHandler> _logger = logger;

    public async Task<Result<List<string>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching roles for user: {UserId}", request.UserId);

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found.", request.UserId);
            return Result<List<string>>.FailureResult(["User not found."]);
        }

        var roles = await _userManager.GetRolesAsync(user);
        _logger.LogInformation("User {UserId} has roles: {Roles}", request.UserId, string.Join(", ", roles));

        return Result<List<string>>.SuccessResult([.. roles]);
    }
}

