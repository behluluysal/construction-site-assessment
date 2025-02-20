using ConstructionSite.Services.Authentication.Application.Common.Interfaces;
using ConstructionSite.Services.Authentication.Application.Security.Responses;
using ConstructionSite.Services.Authentication.Application.Services;
using ConstructionSite.Services.Authentication.Domain.Common;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Features.Authentication;

public class LoginCommandHandler(
    IUserService userService,
    IJwtTokenService jwtTokenService,
    ILogger<LoginCommandHandler> logger,
    IStringLocalizer<LoginCommandHandler> localizer) : IRequestHandler<LoginCommand, Result<AuthenticationResponse>>
{

    public async Task<Result<AuthenticationResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Processing login request for: {UserName}", request.Model.UserName);

            var user = await userService.AuthenticateUserAsync(request.Model);

            var (token, tokenExpiration) = await jwtTokenService.GenerateJwtTokenAsync(user);
            var (refreshToken, refreshExpiration) = await jwtTokenService.GenerateAndStoreRefreshTokenAsync(user);

            var response = new AuthenticationResponse
            {
                Id = user.Id,
                Token = token,
                TokenExpiration = tokenExpiration,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshExpiration
            };

            logger.LogInformation("User {UserName} logged in successfully", request.Model.UserName);
            return Result<AuthenticationResponse>.SuccessResult(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Authentication failed for user: {UserName}", request.Model.UserName);
            return Result<AuthenticationResponse>.FailureResult(localizer["InvalidCredentials"]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during login for: {UserName}", request.Model.UserName);
            return Result<AuthenticationResponse>.FailureResult(localizer["UnexpectedError"]);
        }
    }
}