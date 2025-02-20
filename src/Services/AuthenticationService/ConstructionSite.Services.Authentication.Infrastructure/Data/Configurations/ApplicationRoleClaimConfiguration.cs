using ConstructionSite.Services.Authentication.Infrastructure.Data.Converters;
using ConstructionSite.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConstructionSite.Services.Authentication.Infrastructure.Data.Configurations;

/// <summary>
/// Configures the entity type IdentityRoleClaim and seeds initial data.
/// </summary>
/// <remarks>
/// Initializes a new instance of the IdentityRoleClaimConfiguration class with the specified admin role ID.
/// </remarks>
/// <param name="adminRoleId">The ID of the admin role for seeding data.</param>
/// <param name="supervisorRoleId">The ID of the supervisor role for seeding data.</param>
/// <param name="workerRoleId">The ID of the worker role for seeding data.</param>
internal class ApplicationRoleClaimConfiguration(Ulid adminRoleId, Ulid supervisorRoleId, Ulid workerRoleId) : IEntityTypeConfiguration<IdentityRoleClaim<Ulid>>
{
    #region [ Fields ]

    private readonly Ulid _adminRoleId = adminRoleId;
    private readonly Ulid _supervisorRoleId = supervisorRoleId;
    private readonly Ulid _workerRoleId = workerRoleId;

    #endregion

    #region [ Public Methods ]

    /// <summary>
    /// Configures the IdentityRoleClaim entity type and seeds data.
    /// </summary>
    /// <param name="builder">The builder being used to construct the entity type model.</param>
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Ulid>> builder)
    {
        #region [ Configuration ]

        builder.Property(e => e.RoleId)
             .HasConversion(new UlidToStringConverter())
             .IsRequired()
             .HasMaxLength(26);

        #endregion

        #region [ Seeds ]

        builder.HasData(

            // Supervisor Role Permissions
            new IdentityRoleClaim<Ulid> { Id = 1, RoleId = _supervisorRoleId, ClaimType = "API.Permission", ClaimValue = Permissions.Activities.Create },
            new IdentityRoleClaim<Ulid> { Id = 2, RoleId = _supervisorRoleId, ClaimType = "API.Permission", ClaimValue = Permissions.Activities.View },
            new IdentityRoleClaim<Ulid> { Id = 3, RoleId = _supervisorRoleId, ClaimType = "API.Permission", ClaimValue = Permissions.Activities.Edit },
            new IdentityRoleClaim<Ulid> { Id = 4, RoleId = _supervisorRoleId, ClaimType = "API.Permission", ClaimValue = Permissions.Activities.Delete },
            new IdentityRoleClaim<Ulid> { Id = 5, RoleId = _supervisorRoleId, ClaimType = "API.Permission", ClaimValue = Permissions.ActivityTypes.Create},
            new IdentityRoleClaim<Ulid> { Id = 6, RoleId = _supervisorRoleId, ClaimType = "API.Permission", ClaimValue = Permissions.ActivityTypes.View},
            new IdentityRoleClaim<Ulid> { Id = 7, RoleId = _supervisorRoleId, ClaimType = "API.Permission", ClaimValue = Permissions.ActivityTypes.Edit},
            new IdentityRoleClaim<Ulid> { Id = 8, RoleId = _supervisorRoleId, ClaimType = "API.Permission", ClaimValue = Permissions.ActivityTypes.Delete},
            new IdentityRoleClaim<Ulid> { Id = 9, RoleId = _supervisorRoleId, ClaimType = "API.Permission", ClaimValue = Permissions.Users.ViewWorkers},

            // Worker Role Permissions
            new IdentityRoleClaim<Ulid> { Id = 10, RoleId = _workerRoleId, ClaimType = "API.Permission", ClaimValue = Permissions.Activities.CreateForWorker },
            new IdentityRoleClaim<Ulid> { Id = 11, RoleId = _workerRoleId, ClaimType = "API.Permission", ClaimValue = Permissions.ActivityTypes.ViewForWorker }
        );

        #endregion
    }

    #endregion
}