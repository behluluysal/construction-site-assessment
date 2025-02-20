namespace ConstructionSite.Shared;

public class RefreshTokenRequest
{
    public required string UserId { get; set; }
    public required string RefreshToken { get; set; }
}

public class RefreshTokenResponse
{
    public required Ulid Id { get; set; }
    public required string Token { get; set; }
    public DateTime TokenExpiration { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}