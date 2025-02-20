using ConstructionSite.Services.Authentication.Application.Security.Models;
using ConstructionSite.Services.Authentication.Application.Security.Responses;
using ConstructionSite.Services.Authentication.Domain.Common;
using MediatR;

namespace ConstructionSite.Services.Authentication.Application.Features.Authentication;

[Obsolete("Deprecated, register functionality is not implemented on the Web UI. Use CreateUserCommand instead.")]
public record RegisterCommand(RegisterRequest Model) : IRequest<Result<AuthenticationResponse>>;
public record LoginCommand(LoginRequest Model) : IRequest<Result<AuthenticationResponse>>;