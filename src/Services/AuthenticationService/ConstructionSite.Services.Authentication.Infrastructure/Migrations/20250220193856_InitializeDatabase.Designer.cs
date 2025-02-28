﻿// <auto-generated />
using System;
using ConstructionSite.Services.Authentication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConstructionSite.Services.Authentication.Infrastructure.Migrations
{
    [DbContext(typeof(IdentityDatabaseContext))]
    [Migration("20250220193856_InitializeDatabase")]
    partial class InitializeDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ConstructionSite.Services.Authentication.Domain.Entities.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = "01JMA8NY8VDH5JP200XJ8EZ4XK",
                            Description = "RoleDescription",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "01JMA8PAM2JJA5VNGT14KMXVKD",
                            Description = "Role for Supervisors",
                            Name = "Supervisor",
                            NormalizedName = "SUPERVISOR"
                        },
                        new
                        {
                            Id = "01JMA8PFV7H9ZSGXGE9K4NERZQ",
                            Description = "Role for Construction Site Workers",
                            Name = "Worker",
                            NormalizedName = "WORKER"
                        });
                });

            modelBuilder.Entity("ConstructionSite.Services.Authentication.Domain.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "01JMA8M65MZ4WP2GCF3ZDBV6K9",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "fadeea3d-92d6-4c2d-9b94-d8377dc9d476",
                            Email = "admin@gmail.com",
                            EmailConfirmed = true,
                            IsAdmin = true,
                            LockoutEnabled = false,
                            Name = "Name1",
                            NormalizedEmail = "ADMIN@GMAIL.COM",
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAELalL5v/Qu2Bi8PQ7P/Biq8pCNBRrJt4VeHGTOXQkZxIJ8cMzSb/YlFSHWHeYET+sQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "7f3799f1-1251-41b1-9d51-cca055e5c53a",
                            Surname = "Surname1",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        },
                        new
                        {
                            Id = "01JMA8MEWWPFKSTT17CPDB14KK",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "9469b421-38bc-45d0-a841-36187c7db4c9",
                            Email = "test1@gmail.com",
                            EmailConfirmed = true,
                            IsAdmin = false,
                            LockoutEnabled = false,
                            Name = "Name1",
                            NormalizedEmail = "TEST1@GMAIL.COM",
                            NormalizedUserName = "SUPERVISOR",
                            PasswordHash = "AQAAAAIAAYagAAAAELalL5v/Qu2Bi8PQ7P/Biq8pCNBRrJt4VeHGTOXQkZxIJ8cMzSb/YlFSHWHeYET+sQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "27c524da-dcc1-4d1f-a8ac-0f84f1da87a0",
                            Surname = "Surname1",
                            TwoFactorEnabled = false,
                            UserName = "Supervisor"
                        },
                        new
                        {
                            Id = "01JMA8NKFH81EAAMJQA67XW3SV",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "c23d4354-15bc-400e-86e0-17bcb943655d",
                            Email = "test2@gmail.com",
                            EmailConfirmed = true,
                            IsAdmin = false,
                            LockoutEnabled = false,
                            Name = "Name2",
                            NormalizedEmail = "TEST2@GMAIL.COM",
                            NormalizedUserName = "TESTUSER",
                            PasswordHash = "AQAAAAIAAYagAAAAELalL5v/Qu2Bi8PQ7P/Biq8pCNBRrJt4VeHGTOXQkZxIJ8cMzSb/YlFSHWHeYET+sQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "18a24e19-f76e-4022-9f65-a0e673019a12",
                            Surname = "Surname2",
                            TwoFactorEnabled = false,
                            UserName = "TestUser"
                        });
                });

            modelBuilder.Entity("ConstructionSite.Services.Authentication.Domain.Entities.ApplicationUserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(26)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(26)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            UserId = "01JMA8M65MZ4WP2GCF3ZDBV6K9",
                            RoleId = "01JMA8NY8VDH5JP200XJ8EZ4XK"
                        },
                        new
                        {
                            UserId = "01JMA8MEWWPFKSTT17CPDB14KK",
                            RoleId = "01JMA8PAM2JJA5VNGT14KMXVKD"
                        },
                        new
                        {
                            UserId = "01JMA8NKFH81EAAMJQA67XW3SV",
                            RoleId = "01JMA8PFV7H9ZSGXGE9K4NERZQ"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Ulid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaimType = "API.Permission",
                            ClaimValue = "Activities.Create",
                            RoleId = "01JMA8PAM2JJA5VNGT14KMXVKD"
                        },
                        new
                        {
                            Id = 2,
                            ClaimType = "API.Permission",
                            ClaimValue = "Activities.View",
                            RoleId = "01JMA8PAM2JJA5VNGT14KMXVKD"
                        },
                        new
                        {
                            Id = 3,
                            ClaimType = "API.Permission",
                            ClaimValue = "Activities.Edit",
                            RoleId = "01JMA8PAM2JJA5VNGT14KMXVKD"
                        },
                        new
                        {
                            Id = 4,
                            ClaimType = "API.Permission",
                            ClaimValue = "Activities.Delete",
                            RoleId = "01JMA8PAM2JJA5VNGT14KMXVKD"
                        },
                        new
                        {
                            Id = 5,
                            ClaimType = "API.Permission",
                            ClaimValue = "ActivityTypes.Create",
                            RoleId = "01JMA8PAM2JJA5VNGT14KMXVKD"
                        },
                        new
                        {
                            Id = 6,
                            ClaimType = "API.Permission",
                            ClaimValue = "ActivityTypes.View",
                            RoleId = "01JMA8PAM2JJA5VNGT14KMXVKD"
                        },
                        new
                        {
                            Id = 7,
                            ClaimType = "API.Permission",
                            ClaimValue = "ActivityTypes.Edit",
                            RoleId = "01JMA8PAM2JJA5VNGT14KMXVKD"
                        },
                        new
                        {
                            Id = 8,
                            ClaimType = "API.Permission",
                            ClaimValue = "ActivityTypes.Delete",
                            RoleId = "01JMA8PAM2JJA5VNGT14KMXVKD"
                        },
                        new
                        {
                            Id = 9,
                            ClaimType = "API.Permission",
                            ClaimValue = "Users.View.Workers",
                            RoleId = "01JMA8PAM2JJA5VNGT14KMXVKD"
                        },
                        new
                        {
                            Id = 10,
                            ClaimType = "API.Permission",
                            ClaimValue = "Activities.CreateForWorker",
                            RoleId = "01JMA8PFV7H9ZSGXGE9K4NERZQ"
                        },
                        new
                        {
                            Id = 11,
                            ClaimType = "API.Permission",
                            ClaimValue = "ActivityTypes.ViewForWorker",
                            RoleId = "01JMA8PFV7H9ZSGXGE9K4NERZQ"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Ulid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)");

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Ulid>", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Ulid>", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)");

                    b.Property<string>("LoginProvider")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("ConstructionSite.Services.Authentication.Domain.Entities.ApplicationUserRole", b =>
                {
                    b.HasOne("ConstructionSite.Services.Authentication.Domain.Entities.ApplicationRole", "Role")
                        .WithMany("RoleUsers")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConstructionSite.Services.Authentication.Domain.Entities.ApplicationUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ConstructionSite.Services.Authentication.Domain.Entities.ApplicationRole", b =>
                {
                    b.Navigation("RoleUsers");
                });

            modelBuilder.Entity("ConstructionSite.Services.Authentication.Domain.Entities.ApplicationUser", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
