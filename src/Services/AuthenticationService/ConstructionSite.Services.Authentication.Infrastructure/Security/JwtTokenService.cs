using ConstructionSite.Services.Authentication.Application.Common.Interfaces;
using ConstructionSite.Services.Authentication.Domain.Common;
using ConstructionSite.Services.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConstructionSite.Services.Authentication.Infrastructure.Security;

public class JwtTokenService(UserManager<ApplicationUser> userManager,
                         RoleManager<ApplicationRole> roleManager,
                         IStringLocalizer<JwtTokenService> localizer,
                         ILogger<JwtTokenService> logger,
                         JwtSettings jwtSettings) : IJwtTokenService
{

    #region [ Fields ]

    private readonly UserManager<ApplicationUser> _userManager = userManager;

    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    private readonly JwtSettings _jwtSettings = jwtSettings;

    private readonly IStringLocalizer<JwtTokenService> _localizer = localizer;

    private readonly ILogger<JwtTokenService> _logger = logger;

    #endregion

    /// <summary>
    /// Creates a JWT token for a specified user.
    /// </summary>
    /// <param name="user">User.</param>
    /// <returns>A JWT token string and expiration date.</returns>
    public async Task<(string token, DateTime expiration)> GenerateJwtTokenAsync(ApplicationUser user)
    {
        _logger.LogInformation("Generating JWT token for user ID: {UserId}", user.Id);

        var roles = await _userManager.GetRolesAsync(user) ?? [];
        if (roles.Count == 0)
        {
            _logger.LogWarning("No roles found for user: {UserName}", user.UserName);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName ?? throw new Exception(_localizer["WrongCredentials"])),
            new("full_name", $"{user.Name} {user.Surname}"),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        if (user.IsAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "admin"));
        }

        var userClaims = new List<Claim>();
        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName)
                ?? throw new Exception( _localizer["WrongCredentials"]);

            userClaims.AddRange(await _roleManager.GetClaimsAsync(role));
        }

        claims.AddRange(userClaims.Select(rc => new Claim(rc.Type, rc.Value)));

        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.UtcNow,
            Expires = expiresAt,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_jwtSettings.GetSecretKeyBytes()),
                SecurityAlgorithms.HmacSha256)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        var jwt = _tokenHandler.WriteToken(token);

        _logger.LogInformation("JWT token generated successfully for {UserName}", user.UserName);

        return (jwt, expiresAt);
    }

    public async Task<(string refreshToken, DateTime expiration)> GenerateAndStoreRefreshTokenAsync(ApplicationUser user)
    {
        var refreshToken = Guid.NewGuid().ToString();
        var expiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiration = expiration;

        await _userManager.UpdateAsync(user);

        return (refreshToken, expiration);
    }

}