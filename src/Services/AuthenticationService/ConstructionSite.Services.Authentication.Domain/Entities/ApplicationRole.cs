using Microsoft.AspNetCore.Identity;

namespace ConstructionSite.Services.Authentication.Domain.Entities;

public class ApplicationRole : IdentityRole<Ulid>
{
    public override Ulid Id { get; set; } = Ulid.NewUlid();
    // TODO not used yet
    public string? Description { get; set; }

    public virtual ICollection<ApplicationUserRole> RoleUsers { get; set; } = [];
}