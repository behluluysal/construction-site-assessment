using ConstructionSite.Services.Authentication.Domain.Entities;
using ConstructionSite.Services.Authentication.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConstructionSite.Services.Authentication.Infrastructure.Data.Configurations;

/// <summary>
/// Configures the entity type ApplicationUser and seeds initial data.
/// </summary>
/// <remarks>
/// Initializes a new instance of the ApplicationUserConfiguration class with the specified admin user ID.
/// </remarks>
/// <param name="adminUserId">The ID of the admin user for seeding data.</param>
/// <param name="supervisorUserId">The ID of the supervisor for seeding data.</param>
/// <param name="workerUserId">The ID of the worker for seeding data.</param>
internal class ApplicationUserConfiguration(Ulid adminUserId, Ulid supervisorUserId, Ulid workerUserId)
    : IEntityTypeConfiguration<ApplicationUser>
{
    #region [ Fields ]

    private readonly Ulid _adminUserId = adminUserId;

    private readonly Ulid _supervisorUserId = supervisorUserId;

    private readonly Ulid _workerUserId = workerUserId;

    #endregion

    #region [ Public Methods ]

    /// <summary>
    /// Configures the ApplicationUser entity type and seeds data.
    /// </summary>
    /// <param name="builder">The builder being used to construct the entity type model.</param>
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        #region [ Configurations ]

        builder.Property(e => e.Id)
              .HasConversion(new UlidToStringConverter())
              .IsRequired()
              .HasMaxLength(26);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Surname).IsRequired().HasMaxLength(15);

        #endregion

        #region [ Seeds ]

        builder.HasData(new ApplicationUser
        {
            Id = _adminUserId,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAELalL5v/Qu2Bi8PQ7P/Biq8pCNBRrJt4VeHGTOXQkZxIJ8cMzSb/YlFSHWHeYET+sQ==",
            IsAdmin = true,
            SecurityStamp = "7f3799f1-1251-41b1-9d51-cca055e5c53a",
            ConcurrencyStamp = "fadeea3d-92d6-4c2d-9b94-d8377dc9d476",
            Name = "Name1",
            Surname = "Surname1",
        },
        new ApplicationUser
        {
            Id = _supervisorUserId,
            UserName = "Supervisor",
            NormalizedUserName = "SUPERVISOR",
            Email = "test1@gmail.com",
            NormalizedEmail = "TEST1@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAELalL5v/Qu2Bi8PQ7P/Biq8pCNBRrJt4VeHGTOXQkZxIJ8cMzSb/YlFSHWHeYET+sQ==",
            IsAdmin = false,
            SecurityStamp = "27c524da-dcc1-4d1f-a8ac-0f84f1da87a0",
            ConcurrencyStamp = "9469b421-38bc-45d0-a841-36187c7db4c9",
            Name = "Name1",
            Surname = "Surname1",
        },
        new ApplicationUser
        {
            Id = _workerUserId,
            UserName = "TestUser",
            NormalizedUserName = "TESTUSER",
            Email = "test2@gmail.com",
            NormalizedEmail = "TEST2@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAELalL5v/Qu2Bi8PQ7P/Biq8pCNBRrJt4VeHGTOXQkZxIJ8cMzSb/YlFSHWHeYET+sQ==",
            IsAdmin = false,
            SecurityStamp = "18a24e19-f76e-4022-9f65-a0e673019a12",
            ConcurrencyStamp = "c23d4354-15bc-400e-86e0-17bcb943655d",
            Name = "Name2",
            Surname = "Surname2",
        });

        #endregion
    }

    #endregion
}