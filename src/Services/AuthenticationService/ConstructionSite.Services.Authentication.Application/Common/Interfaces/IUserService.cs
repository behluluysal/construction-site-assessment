using ConstructionSite.Services.Authentication.Application.Security.Models;
using ConstructionSite.Services.Authentication.Domain.Entities;

namespace ConstructionSite.Services.Authentication.Application.Common.Interfaces;

public interface IUserService
{
    Task<ApplicationUser> RegisterUserAsync(RegisterRequest request);
    Task<ApplicationUser> AuthenticateUserAsync(LoginRequest request);
}
