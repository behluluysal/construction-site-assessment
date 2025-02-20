using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using ConstructionSite.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ConstructionSite.Services.Authentication.Application.Features.Roles;

public class AssignClaimCommandHandler(RoleManager<ApplicationRole> roleManager, ILogger<AssignClaimCommandHandler> logger)
    : IRequestHandler<AssignClaimCommand, Result>
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ILogger<AssignClaimCommandHandler> _logger = logger;

    public async Task<Result> Handle(AssignClaimCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Assigning claim '{ClaimName}' to role '{RoleName}'", request.ClaimName, request.RoleName);

        var role = await _roleManager.FindByNameAsync(request.RoleName);
        if (role == null)
        {
            _logger.LogWarning("Role '{RoleName}' not found", request.RoleName);
            return Result.Failure($"Role '{request.RoleName}' not found");
        }

        List<string> availableClaims = [.. Permissions.Users.Metrics, .. Permissions.ActivityTypes.Metrics, .. Permissions.Activities.Metrics];
        if (!availableClaims.Contains(request.ClaimName))
        {
            _logger.LogWarning("Attempted to assign invalid claim: {ClaimName}", request.ClaimName);
            return Result.Failure($"Invalid claim: {request.ClaimName}");
        }

        var existingClaims = await _roleManager.GetClaimsAsync(role);
        if (existingClaims.Any(c => c.Type == "API.Permission" && c.Value == request.ClaimName))
        {
            _logger.LogWarning("Role '{RoleName}' already has claim: {ClaimName}", request.RoleName, request.ClaimName);
            return Result.Failure($"Role '{request.RoleName}' already has claim '{request.ClaimName}'");
        }

        var result = await _roleManager.AddClaimAsync(role, new Claim("API.Permission", request.ClaimName));
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to assign claim '{ClaimName}' to role '{RoleName}': {Errors}", request.ClaimName, request.RoleName, errors);
            return Result.Failure($"Failed to assign claim '{request.ClaimName}' to role '{request.RoleName}': {errors}");
        }

        _logger.LogInformation("Successfully assigned claim '{ClaimName}' to role '{RoleName}'", request.ClaimName, request.RoleName);
        return Result.Success();
    }
}


