using ConstructionSite.Services.Authentication.Application.Common.Interfaces;
using ConstructionSite.Services.Authentication.Application.Security.Models;
using ConstructionSite.Services.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ConstructionSite.Services.Authentication.Application.Services
{
    public class UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger, IStringLocalizer<UserService> localizer) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<UserService> _logger = logger;
        private readonly IStringLocalizer<UserService> _localizer = localizer;

        public async Task<ApplicationUser> RegisterUserAsync(RegisterRequest request)
        {
            _logger.LogInformation("Registration attempt for email: {Email}", request.Email);

            var userExists = await _userManager.Users.AnyAsync(u => u.Email == request.Email || u.UserName == request.UserName);
            if (userExists)
            {
                _logger.LogWarning("User already exists: {Email}", request.Email);
                throw new InvalidOperationException(_localizer["UserExists"]);
            }

            var newUser = new ApplicationUser
            {
                Id = Ulid.NewUlid(),
                UserName = request.UserName,
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"{string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            _logger.LogInformation("User registered successfully: {Email}", request.Email);
            return newUser;
        }
        public async Task<ApplicationUser> AuthenticateUserAsync(LoginRequest request)
        {
            _logger.LogInformation("Authenticating user: {UserName}", request.UserName);

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserName}", request.UserName);
                throw new UnauthorizedAccessException(_localizer["InvalidCredentials"]);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Invalid password for user: {UserName}", request.UserName);
                throw new UnauthorizedAccessException(_localizer["InvalidCredentials"]);
            }

            _logger.LogInformation("User authenticated successfully: {UserName}", request.UserName);
            return user;
        }
    }
}
