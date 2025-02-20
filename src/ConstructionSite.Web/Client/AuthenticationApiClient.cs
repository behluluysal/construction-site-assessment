using ConstructionSite.Web.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace ConstructionSite.Web.Client;

public class AuthenticationApiClient(HttpClient httpClient,
                                     AuthenticationStateProvider authStateProvider,
                                     NavigationManager navigationManager,
                                     NotificationService notificationService,
                                     AuthService authService)
    : ApiClientBase(httpClient, authStateProvider, navigationManager, notificationService, authService)
{
    public async Task<TokenResponse?> ConnectAsync(string username, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/authentication/connect")
        {
            Content = JsonContent.Create(new { Username = username, Password = password })
        };

        var response = await SendRequestAsync(request);
        var apiResponse = await HandleApiResponseAsync<TokenResponse>(response);
        return apiResponse?.Data;
    }

    public async Task<UsersResponseData?> GetUsersAsync(string queryString)
    {
        var response = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Get, $"api/users?{queryString}"));
        var apiResponse = await HandleApiResponseAsync<UsersResponseData>(response);
        return apiResponse?.Data;
    }

    public async Task<string?> CreateUserAsync(UserViewModel user)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/users")
        {
            Content = JsonContent.Create(new { user.UserName, user.Email, user.Name, user.Surname, user.Password })
        };

        var response = await SendRequestAsync(request,
            "User Created",
            $"User '{user.UserName}' was successfully created.");
        return (await HandleApiResponseAsync<string>(response))?.Data;
    }

    public async Task UpdateUserAsync(UserViewModel user)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "api/users")
        {
            Content = JsonContent.Create(new { user.Id, user.UserName, user.Email, user.Name, user.Surname })
        };

        _ = await SendRequestAsync(request,
            "User Updated",
            $"User '{user.UserName}' was successfully updated.");
    }

    public async Task DeleteUserAsync(string id)
    {
        _ = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Delete, $"api/users/{id}"),
            "User Deleted",
            $"The user with ID {id} has been deleted successfully.");
    }

    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var response = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Get, $"api/users/{userId}/roles"));
        var apiResponse = await HandleApiResponseAsync<List<string>>(response);

        return apiResponse?.Data ?? [];
    }

    public async Task UpdateUserRolesAsync(string userId, List<string> roles)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/users/{userId}/roles")
        {
            Content = JsonContent.Create(roles)
        };

        _ = await SendRequestAsync(request,
            "Roles Updated",
            $"User with Id '{userId}'s roles have been updated successfully.");

    }

    public async Task AssignRoleAsync(string userId, string roleName)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/users/{userId}/roles/{roleName}");
        _ = await SendRequestAsync(request,
            "Role Assigned", $"Role '{roleName}' has been assigned to the user '{userId}'.");
    }

    public async Task RevokeRoleAsync(string userId, string roleName)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/users/{userId}/roles/{roleName}");
        _ = await SendRequestAsync(request,
            "Role Revoked",
            $"Role '{roleName}' has been revoked from the user '{userId}'.");
    }


    public async Task<List<RoleData>> GetAllRolesAsync()
    {
        var response = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Get, "api/roles"));
        var apiResponse = await HandleApiResponseAsync<List<RoleData>>(response);

        return apiResponse?.Data ?? [];
    }

    public async Task<List<string>> GetRoleClaimsAsync(string roleName)
    {
        var response = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Get, $"api/roles/{roleName}/claims"));
        var apiResponse = await HandleApiResponseAsync<List<string>>(response);
        return apiResponse?.Data ?? [];
    }

    public async Task AssignClaimToRoleAsync(string roleName, string claimName)
    {
        _ = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Post, $"api/roles/{roleName}/claims/{claimName}"),
            "Claim Assigned",
            $"Claim '{claimName}' has been assigned to the role {roleName}.");
    }

    public async Task RemoveClaimFromRoleAsync(string roleName, string claimName)
    {
        _ = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Delete, $"api/roles/{roleName}/claims/{claimName}"),
            "Claim Revoked",
            $"Claim '{claimName}' has been revoked from the role {roleName}.");
    }

    public async Task<UsersResponseData?> GetWorkersAsync(string queryString)
    {
        var response = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Get, $"api/users/workers?{queryString}"));
        var apiResponse = await HandleApiResponseAsync<UsersResponseData>(response);
        return apiResponse?.Data;
    }
}



public class TokenResponse
{
    public string Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime TokenExpiration { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; set; }
}

public class UsersResponseData
{
    public List<UserViewModel> Users { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int ElementsInCurrentPage { get; set; }
}

public class UserViewModel
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName => $"{Name} {Surname}";
}

public class RoleData
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
