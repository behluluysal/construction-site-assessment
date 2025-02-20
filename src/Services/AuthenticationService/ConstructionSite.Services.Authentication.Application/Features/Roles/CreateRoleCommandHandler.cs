using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Roles;

public class CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager, ILogger<CreateRoleCommandHandler> logger)
    : IRequestHandler<CreateRoleCommand, Result<Ulid>>
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ILogger<CreateRoleCommandHandler> _logger = logger;

    public async Task<Result<Ulid>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RoleName))
        {
            _logger.LogWarning("Role creation failed: Role name is empty or null.");
            return Result<Ulid>.FailureResult("Role name cannot be empty.");
        }

        if (await _roleManager.RoleExistsAsync(request.RoleName))
        {
            _logger.LogWarning("Role creation failed: {RoleName} already exists.", request.RoleName);
            return Result<Ulid>.FailureResult($"Role '{request.RoleName}' already exists.");
        }

        var role = new ApplicationRole {Id = Ulid.NewUlid(), Name = request.RoleName };
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to create role '{RoleName}': {Errors}", request.RoleName, errors);
            return Result<Ulid>.FailureResult($"Failed to create role '{request.RoleName}': {errors}");
        }

        _logger.LogInformation("Created Role: {RoleName}", request.RoleName);
        return Result<Ulid>.SuccessResult(role.Id);
    }
}

