using ConstructionSite.Services.Authentication.Domain.Entities;
using ConstructionSite.Services.Authentication.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConstructionSite.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the ApplicationUserRole entity type and seeds initial data.
    /// </summary>
    internal class ApplicationUserRoleConfiguration(Ulid adminRoleId, Ulid adminUserId, Ulid supervisorRoleId, Ulid supervisorUserId, Ulid workerRoleId, Ulid workerUserId)
        : IEntityTypeConfiguration<ApplicationUserRole>
    {
        #region [ Fields ]

        private readonly Ulid _adminRoleId = adminRoleId;
        private readonly Ulid _adminUserId = adminUserId;
        private readonly Ulid _supervisorRoleId = supervisorRoleId;
        private readonly Ulid _supervisorUserId = supervisorUserId;
        private readonly Ulid _workerRoleId = workerRoleId;
        private readonly Ulid _workerUserId = workerUserId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the ApplicationUserRole entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            #region [ Configurations ]
            builder
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            builder
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder
                .HasOne(ur => ur.Role)
                .WithMany(r => r.RoleUsers)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            #endregion

            #region [ Seeds ]

            builder.HasData(
                new ApplicationUserRole { RoleId = _adminRoleId, UserId = _adminUserId },
                new ApplicationUserRole { RoleId = _supervisorRoleId, UserId = _supervisorUserId },
                new ApplicationUserRole { RoleId = _workerRoleId, UserId = _workerUserId }
            );

            #endregion
        }

        #endregion
    }
}
