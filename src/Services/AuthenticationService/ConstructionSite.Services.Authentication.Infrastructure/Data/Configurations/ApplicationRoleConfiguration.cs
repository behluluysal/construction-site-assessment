using ConstructionSite.Services.Authentication.Domain.Entities;
using ConstructionSite.Services.Authentication.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConstructionSite.Services.Authentication.Infrastructure.Data.Configurations;

/// <summary>
/// Configures the entity type ApplicationRoleConfiguration and seeds initial data.
/// </summary>
/// <remarks>
/// Initializes a new instance of the ApplicationRoleConfiguration class with the specified admin role ID.
/// </remarks>
/// <param name="adminRoleId">The ID of the admin role for seeding data.</param>
/// <param name="supervisorRoleId">The ID of the supervisor role for seeding data.</param>
/// <param name="workerRoleId">The ID of the worker role for seeding data.</param>

internal class ApplicationRoleConfiguration(Ulid adminRoleId, Ulid supervisorRoleId, Ulid workerRoleId)
    : IEntityTypeConfiguration<ApplicationRole>
{
    #region [ Fields ]

    private readonly Ulid _adminRoleId = adminRoleId;

    private readonly Ulid _supervisorRoleId = supervisorRoleId;

    private readonly Ulid _workerRoleId = workerRoleId;

    #endregion

    #region [ Public Methods ]

    /// <summary>
    /// Configures the ApplicationRole entity type and seeds data.
    /// </summary>
    /// <param name="builder">The builder being used to construct the entity type model.</param>
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        #region [ Configurations ]

        builder.Property(e => e.Id)
              .HasConversion(new UlidToStringConverter())
              .IsRequired()
              .HasMaxLength(26);

        builder.Property(x => x.Description).HasMaxLength(60);

        #endregion

        #region [ Seeds ]

        builder.HasData(new ApplicationRole
        {
            Id = _adminRoleId,
            Name = "Admin",
            Description = "RoleDescription",
            NormalizedName = "ADMIN",
        },
        new ApplicationRole
        {
            Id = _supervisorRoleId,
            Name = "Supervisor",
            Description = "Role for Supervisors",
            NormalizedName = "SUPERVISOR",
        },
        new ApplicationRole
        {
            Id = _workerRoleId,
            Name = "Worker",
            Description = "Role for Construction Site Workers",
            NormalizedName = "WORKER",
        });

        #endregion
    }

    #endregion
}