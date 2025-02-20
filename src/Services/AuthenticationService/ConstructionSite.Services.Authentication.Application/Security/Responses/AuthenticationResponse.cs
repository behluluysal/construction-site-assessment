namespace ConstructionSite.Services.Authentication.Application.Security.Responses;

public record AuthenticationResponse
{
    public Ulid Id { get; set; }
    public string Token { get; init; } = default!;
    public DateTime TokenExpiration { get; init; }
    public string RefreshToken { get; init; } = default!;
    public DateTime RefreshTokenExpiration { get; init; }
}
