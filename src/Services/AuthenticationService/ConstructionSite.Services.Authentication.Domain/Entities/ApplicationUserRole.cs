using Microsoft.AspNetCore.Identity;

namespace ConstructionSite.Services.Authentication.Domain.Entities;

public class ApplicationUserRole : IdentityUserRole<Ulid>
{
    public virtual ApplicationUser User { get; set; } = default!;
    public virtual ApplicationRole Role { get; set; } = default!;
}
