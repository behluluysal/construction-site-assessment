using ConstructionSite.Web.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace ConstructionSite.Web.Client;

public abstract class ApiClientBase(HttpClient httpClient,
                        AuthenticationStateProvider authStateProvider,
                        NavigationManager navigationManager,
                        NotificationService notificationService,
                        AuthService authService)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;
    private readonly NavigationManager _navigationManager = navigationManager;
    protected readonly NotificationService _notificationService = notificationService;
    protected readonly AuthService _authService = authService;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, string? successTitle = null, string? successMessage = null)
    {
        try
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                bool tokenRefreshed = await TryRefreshTokenAsync();

                if (tokenRefreshed)
                {
                    await SetAuthorizationHeaderAsync();
                    var newRequest = new HttpRequestMessage(request.Method, request.RequestUri)
                    {
                        Content = request.Content 
                    };
                    response = await _httpClient.SendAsync(newRequest);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _navigationManager.NavigateTo("/login");
                    return response;
                }
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                _notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Access Denied",
                    Detail = "You do not have permission to perform this action.",
                    Duration = 4000
                });
                return response;
            }

            if (!response.IsSuccessStatusCode)
            {
                // Attempt to parse the response as ApiResponse<T>
                string responseContent = await response.Content.ReadAsStringAsync();

                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, _jsonOptions);

                    if (errorResponse?.Succeeded == false && errorResponse.Errors.Count != 0)
                    {
                        _notificationService.Notify(new NotificationMessage
                        {
                            Severity = NotificationSeverity.Error,
                            Summary = "Request Failed",
                            Detail = string.Join(", ", errorResponse.Errors),
                            Duration = 5000
                        });
                    }
                    else
                    {
                        _notificationService.Notify(new NotificationMessage
                        {
                            Severity = NotificationSeverity.Error,
                            Summary = "Request Failed",
                            Detail = response.ReasonPhrase ?? "An unknown error occurred.",
                            Duration = 5000
                        });
                    }
                }
                catch (JsonException)
                {
                    _notificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Request Failed",
                        Detail = $"{response.ReasonPhrase} - {responseContent}",
                        Duration = 5000
                    });
                }
            }
            else if (!string.IsNullOrEmpty(successTitle) && !string.IsNullOrEmpty(successMessage))
            {
                _notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = successTitle,
                    Detail = successMessage,
                    Duration = 4000
                });
            }

            return response;
        }
        catch (Exception ex)
        {
            _notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Warning,
                Summary = "Client Connection Error",
                Detail = ex.Message,
                Duration = 4000
            });

            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = "Client Error: " + ex.Message
            };
        }
    }


    protected async Task<ApiResponse<T>?> HandleApiResponseAsync<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            return null;

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();

        if (apiResponse == null)
        {
            _notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Unexpected Error",
                Detail = "Failed to parse the server response.",
                Duration = 4000
            });
            return null;
        }

        if (!apiResponse.Succeeded)
        {
            foreach (var error in apiResponse.Errors)
            {
                _notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = error,
                    Duration = 4000
                });
            }
            return null;
        }

        return apiResponse;
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        if (_authStateProvider is JwtAuthStateProvider jwtAuthStateProvider)
        {
            var token = await jwtAuthStateProvider.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }

    private async Task<bool> TryRefreshTokenAsync()
    {
        string? refreshToken = "";
        string? userId = "";
        if (_authStateProvider is JwtAuthStateProvider jwtAuthStateProvider)
        {
            refreshToken = await jwtAuthStateProvider.GetRefreshTokenAsync();
            userId = await jwtAuthStateProvider.GetUserIdAsync();
            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var newTokenResponse = await _authService.RefreshTokenAsync(refreshToken, userId);

            if (newTokenResponse == null || string.IsNullOrEmpty(newTokenResponse.Token))
            {
                return false;
            }

            await jwtAuthStateProvider.SessionStorage.SetAsync("jwttoken", newTokenResponse.Token);
            await jwtAuthStateProvider.SessionStorage.SetAsync("refreshToken", newTokenResponse.RefreshToken);
            await jwtAuthStateProvider.SessionStorage.SetAsync("userId", newTokenResponse.Id);
            await jwtAuthStateProvider.AuthenticateUser(newTokenResponse.Token);
            return true;

        }
        return false;
    }
}
