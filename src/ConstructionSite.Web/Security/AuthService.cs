using ConstructionSite.Shared;
using ConstructionSite.Web.Client;

namespace ConstructionSite.Web.Security;

public class AuthService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<TokenResponse?> ConnectAsync(string username, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/authentication/connect")
        {
            Content = JsonContent.Create(new { Username = username, Password = password })
        };

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return null; // Handle authentication failure
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TokenResponse>>();
        return apiResponse?.Data;
    }

    public async Task<RefreshTokenResponse?> RefreshTokenAsync(string refreshToken, string userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/authentication/refresh")
        {
            Content = JsonContent.Create(new RefreshTokenRequest
            {
                RefreshToken = refreshToken,
                UserId = userId
            })
        };

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return null; // Refresh failed
        }

        return await response.Content.ReadFromJsonAsync<RefreshTokenResponse>();
    }
}


