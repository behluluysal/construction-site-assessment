namespace ConstructionSite.Services.Authentication.Application.Security.Models;

public class RegisterRequest
{
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
}
