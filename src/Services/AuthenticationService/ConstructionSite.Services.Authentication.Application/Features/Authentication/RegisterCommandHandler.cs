using ConstructionSite.Services.Authentication.Application.Common.Interfaces;
using ConstructionSite.Services.Authentication.Application.Security.Responses;
using ConstructionSite.Services.Authentication.Domain.Common;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Authentication;

public class RegisterCommandHandler(
    IUserService userService,
    IJwtTokenService jwtTokenService,
    ILogger<RegisterCommandHandler> logger,
    IStringLocalizer<RegisterCommandHandler> localizer
) : IRequestHandler<RegisterCommand, Result<AuthenticationResponse>>
{
    #region [ Fields ]

    private readonly IUserService _userService = userService;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;
    private readonly IStringLocalizer<RegisterCommandHandler> _localizer = localizer;

    #endregion

    public async Task<Result<AuthenticationResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.RegisterUserAsync(request.Model);

            var (token, tokenExpiration) = await jwtTokenService.GenerateJwtTokenAsync(user);
            var (refreshToken, refreshExpiration) = await jwtTokenService.GenerateAndStoreRefreshTokenAsync(user);

            var response = new AuthenticationResponse
            {
                Token = token,
                TokenExpiration = tokenExpiration,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshExpiration
            };

            _logger.LogInformation("User {Email} registered successfully.", request.Model.Email);
            return Result<AuthenticationResponse>.SuccessResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during registration for {Email}", request.Model.Email);
            return Result<AuthenticationResponse>.FailureResult(
                _localizer["UnexpectedError"]);
        }
    }
}