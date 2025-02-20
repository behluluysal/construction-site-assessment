using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ConstructionSite.Web.Security;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private static readonly AuthenticationState anonUser
        = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public ProtectedSessionStorage SessionStorage;
    private readonly byte[] jwtSecretKey;
    private string? currentToken;
    private bool connectionEstablished;
    private readonly int timeOut;
    private readonly AuthService _authService;
    public JwtAuthStateProvider
        (ProtectedSessionStorage browserStorage, IConfiguration config, AuthService authService)
    {
        connectionEstablished = false;
        currentToken = null;
        SessionStorage = browserStorage;
        string? strTimeOut = config["InactivityTimeOut"];
        if (strTimeOut == null || !int.TryParse(strTimeOut, out timeOut))
            timeOut = 300; // Default to five minutes (300 seconds)
        string? key = config["JWTSecretKey"]
            ?? throw new ArgumentException("Config contains no JWT secret key");
        jwtSecretKey = Encoding.ASCII.GetBytes(key);
        _authService = authService;
    }


    public int InactivityTimeOut => timeOut;

    public async Task<bool> Login(AuthenticationRequest? authRequest)
    {
        if (authRequest == null)
            return false;

        var result = await _authService.ConnectAsync(authRequest.UserName, authRequest.Password);

        if (result == null)
            return false;

        //currentToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwiZnVsbF9uYW1lIjoiTmFtZTEgU3VybmFtZTEiLCJuYW1laWQiOiIwMUpNQThNNjVNWjRXUDJHQ0YzWkRCVjZLOSIsInJvbGUiOiJhZG1pbiIsIm5iZiI6MTc0MDA2MjI1MiwiZXhwIjoxNzQwMDYzNDUyLCJpYXQiOjE3NDAwNjIyNTIsImlzcyI6IkNvbnN0cnVjdGlvblNpdGVJc3N1ZXIiLCJhdWQiOiJDb25zdHJ1Y3Rpb25TaXRlQXVkaWVuY2UifQ.caSXO1OVV6l7dGrX4vU41lEvBkSAiB76e86W-NhPOvs";
        currentToken = result.Token;
        if (connectionEstablished)
        {
            await SessionStorage.SetAsync("jwttoken", currentToken);
            await SessionStorage.SetAsync("refreshToken", result.RefreshToken);
            await SessionStorage.SetAsync("userId", result.Id);
        }
        await AuthenticateUser(currentToken);
        return true;
    }

    public async Task Logout()
    {
        currentToken = null;
        if (connectionEstablished)
        {
            await SessionStorage.DeleteAsync("jwttoken");
            await SessionStorage.DeleteAsync("refreshToken");
            await SessionStorage.DeleteAsync("userId");
        }
        await AuthenticateUser(string.Empty);
    }

    public async Task AuthenticateUser(string jwt)
    {
        AuthenticationState authState = await DecodeJwt(jwt);
        NotifyAuthenticationStateChanged
            (Task.FromResult(authState));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (connectionEstablished)
        {
            var tokenResult = await SessionStorage.GetAsync<string>("jwttoken");

            if (!tokenResult.Success || tokenResult.Value == null)
                return anonUser;

            return await DecodeJwt(tokenResult.Value);
        }
        else if (string.IsNullOrWhiteSpace(currentToken))
            return anonUser;
        else
            return await DecodeJwt(currentToken);
    }

    private async Task<AuthenticationState> DecodeJwt(string jwt)
    {
        JsonWebTokenHandler handler = new();
        TokenValidationParameters validationParams = new()
        {
            ValidIssuer = "ConstructionSiteIssuer",
            ValidAudience = "ConstructionSiteAudience",
            ValidateIssuer = true,
            ValidateAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtSecretKey),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(5)
        };
        TokenValidationResult result = await handler
            .ValidateTokenAsync(jwt, validationParams);
        if (result.IsValid)
        {
            var identity = new ClaimsIdentity(result.ClaimsIdentity.Claims, "jwt");

            // using session storage instead
            //identity.AddClaim(new Claim("access_token", jwt));

            ClaimsPrincipal principal = new(identity);
            return new AuthenticationState(principal);
        }
        return anonUser;
    }

    public async Task<string?> GetTokenAsync()
    {
        var refreshTokenResult = await SessionStorage.GetAsync<string>("jwttoken");

        return refreshTokenResult.Success ? refreshTokenResult.Value : null;
    }
    public async Task<string?> GetRefreshTokenAsync()
    {
        var refreshTokenResult = await SessionStorage.GetAsync<string>("refreshToken");

        return refreshTokenResult.Success ? refreshTokenResult.Value : null;
    }
    public async Task<string?> GetUserIdAsync()
    {
        var refreshTokenResult = await SessionStorage.GetAsync<string>("userId");

        return refreshTokenResult.Success ? refreshTokenResult.Value : null;
    }


    public void ConnectionEstablished()
        => connectionEstablished = true;
}
