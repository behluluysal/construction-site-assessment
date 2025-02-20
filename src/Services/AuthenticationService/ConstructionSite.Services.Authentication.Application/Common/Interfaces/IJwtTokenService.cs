using ConstructionSite.Services.Authentication.Domain.Entities;

namespace ConstructionSite.Services.Authentication.Application.Common.Interfaces;

public interface IJwtTokenService
{
    Task<(string token, DateTime expiration)> GenerateJwtTokenAsync(ApplicationUser user);
    Task<(string refreshToken, DateTime expiration)> GenerateAndStoreRefreshTokenAsync(ApplicationUser user);
    //bool VerifyRefreshToken(string tokenToVerify, string storedToken);
}