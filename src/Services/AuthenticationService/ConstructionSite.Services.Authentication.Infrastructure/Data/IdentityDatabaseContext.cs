using ConstructionSite.Services.Authentication.Domain.Entities;
using ConstructionSite.Services.Authentication.Infrastructure.Data.Configurations;
using ConstructionSite.Services.Authentication.Infrastructure.Data.Converters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConstructionSite.Services.Authentication.Infrastructure.Data;

public class IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Ulid,
    IdentityUserClaim<Ulid>, ApplicationUserRole, IdentityUserLogin<Ulid>,
    IdentityRoleClaim<Ulid>, IdentityUserToken<Ulid>>(options)
{
    #region [ Properties ]


    #endregion

    #region [ Protected ]

    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region [ Common Ids ]

        Ulid adminUserId = Ulid.Parse("01JMA8M65MZ4WP2GCF3ZDBV6K9");
        Ulid supervisorUserId = Ulid.Parse("01JMA8MEWWPFKSTT17CPDB14KK");
        Ulid workerUserId = Ulid.Parse("01JMA8NKFH81EAAMJQA67XW3SV");

        Ulid adminRoleId = Ulid.Parse("01JMA8NY8VDH5JP200XJ8EZ4XK");
        Ulid supervisorRoleId = Ulid.Parse("01JMA8PAM2JJA5VNGT14KMXVKD");
        Ulid workerRoleId = Ulid.Parse("01JMA8PFV7H9ZSGXGE9K4NERZQ");

        #endregion

        #region [ Ulid Converter ]

        builder.Entity<IdentityUserClaim<Ulid>>(entity =>
        {
            entity.Property(e => e.UserId)
                  .HasConversion(new UlidToStringConverter())
                  .IsRequired()
                  .HasMaxLength(26);
        });

        builder.Entity<IdentityUserLogin<Ulid>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        builder.Entity<IdentityUserLogin<Ulid>>(entity =>
        {
            entity.HasKey(e => new { e.UserId });
            entity.Property(e => e.UserId)
                  .HasConversion(new UlidToStringConverter())
                  .IsRequired()
                  .HasMaxLength(26);
        });

        builder.Entity<IdentityUserToken<Ulid>>(entity =>
        {
            entity.HasKey(e => new { e.UserId });

            entity.Property(e => e.UserId)
                  .HasConversion(new UlidToStringConverter())
                  .IsRequired()
                  .HasMaxLength(26);
        });

        #endregion

        #region [ Entity Configurations ]

        builder.ApplyConfiguration(new ApplicationRoleConfiguration(adminRoleId, supervisorRoleId, workerRoleId));
        builder.ApplyConfiguration(new ApplicationUserConfiguration(adminUserId, supervisorUserId, workerUserId));
        builder.ApplyConfiguration(new ApplicationRoleClaimConfiguration(adminRoleId, supervisorRoleId, workerRoleId));
        builder.ApplyConfiguration(new ApplicationUserRoleConfiguration(adminRoleId, adminUserId, supervisorRoleId, supervisorUserId, workerRoleId, workerUserId));

        #endregion
    }

    #endregion

    #region [ Internal Class ]



    #endregion
}
