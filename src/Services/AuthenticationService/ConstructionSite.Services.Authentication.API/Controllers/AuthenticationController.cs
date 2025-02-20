using ConstructionSite.Services.Authentication.Application.Common.Interfaces;
using ConstructionSite.Services.Authentication.Application.Features.Authentication;
using ConstructionSite.Services.Authentication.Application.Security.Models;
using ConstructionSite.Services.Authentication.Domain.Entities;
using ConstructionSite.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionSite.Services.Authentication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthenticationController(
    IMediator mediator,
    ILogger<AuthenticationController> logger,
    UserManager<ApplicationUser> userManager,
    IJwtTokenService jwtTokenService) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<AuthenticationController> _logger = logger;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

    [HttpPost("connect")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var command = new LoginCommand(model);
        var result = await _mediator.Send(command);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        var command = new RegisterCommand(model);
        var result = await _mediator.Send(command);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        _logger.LogInformation("Received refresh token request");

        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            return Unauthorized(new { error = "Refresh token is required" });
        }

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiration < DateTime.UtcNow)
        {
            _logger.LogWarning("Invalid or expired refresh token for user {UserId}", request.UserId);
            return Unauthorized(new { error = "Invalid or expired refresh token" });
        }

        // Generate new JWT & Refresh Token
        var (newToken, newTokenExpiration) = await _jwtTokenService.GenerateJwtTokenAsync(user);
        var (newRefreshToken, newRefreshExpiration) = await _jwtTokenService.GenerateAndStoreRefreshTokenAsync(user);

        return Ok(new RefreshTokenResponse
        {
            Id = user.Id,
            Token = newToken,
            TokenExpiration = newTokenExpiration,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiration = newRefreshExpiration
        });
    }



}
