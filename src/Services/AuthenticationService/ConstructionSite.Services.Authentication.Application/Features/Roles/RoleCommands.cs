using ConstructionSite.Services.Authentication.Application.DTOs;
using ConstructionSite.Services.Authentication.Domain.Common;
using MediatR;

namespace ConstructionSite.Services.Authentication.Application.Features.Roles;

public record CreateRoleCommand(string RoleName) : IRequest<Result<Ulid>>;
public record GetRoleByIdQuery(Ulid Id) : IRequest<Result<RoleDto>>;
public record DeleteRoleCommand(string RoleName) : IRequest<Result>;
public record GetAllRolesQuery() : IRequest<Result<IEnumerable<RoleDto>>>;

public record AssignClaimCommand(string RoleName, string ClaimName) : IRequest<Result>;
public record RevokeClaimCommand(string RoleName, string ClaimName) : IRequest<Result>;
public record GetRoleClaimsQuery(string RoleName) : IRequest<Result<List<string>>>;
