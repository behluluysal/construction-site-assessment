using Microsoft.AspNetCore.Identity;

namespace ConstructionSite.Services.Authentication.Domain.Entities;

public class ApplicationUser : IdentityUser<Ulid>
{
    #region [ Properties ]

    public override Ulid Id { get; set; } = Ulid.NewUlid();
    public bool IsAdmin { get; set; } = false;

    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }

    #endregion

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
}