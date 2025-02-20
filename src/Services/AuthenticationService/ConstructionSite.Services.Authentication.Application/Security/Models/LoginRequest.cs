namespace ConstructionSite.Services.Authentication.Application.Security.Models;

public record LoginRequest
{
    public string UserName { get; init; } = default!;
    public string Password { get; init; } = default!;
}
