using ConstructionSite.Services.Authentication.Application.DTOs;
using ConstructionSite.Services.Authentication.Domain.Common;
using MediatR;

namespace ConstructionSite.Services.Authentication.Application.Features.Users;

public record CreateUserCommand(string UserName, string Email, string Name, string Surname, string Password) : IRequest<Result<Ulid>>;
public record UpdateUserCommand(string Id, string UserName, string Email, string Name, string Surname) : IRequest<Result>;
public record DeleteUserCommand(string Id) : IRequest<Result>;
public record AssignRoleCommand(string UserId, string RoleName) : IRequest<Result>;
public record RevokeRoleCommand(string UserId, string RoleName) : IRequest<Result>;
public record GetUserByIdQuery(string Id) : IRequest<Result<UserDto>>;


// Queries
public record GetAllUsersQuery(int PageNumber, int PageSize, string? OrderBy, string? Filter) : IRequest<Result<UserListResultDto>>;
public record GetUserRolesQuery(string UserId) : IRequest<Result<List<string>>>;
public record GetWorkersQuery(int PageNumber, int PageSize, string? OrderBy, string? Filter) : IRequest<Result<UserListResultDto>>;