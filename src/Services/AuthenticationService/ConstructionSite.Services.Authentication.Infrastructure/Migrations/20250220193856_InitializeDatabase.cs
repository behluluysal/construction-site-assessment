using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConstructionSite.Services.Authentication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "API.Permission", "Activities.Create", "01JMA8PAM2JJA5VNGT14KMXVKD" },
                    { 2, "API.Permission", "Activities.View", "01JMA8PAM2JJA5VNGT14KMXVKD" },
                    { 3, "API.Permission", "Activities.Edit", "01JMA8PAM2JJA5VNGT14KMXVKD" },
                    { 4, "API.Permission", "Activities.Delete", "01JMA8PAM2JJA5VNGT14KMXVKD" },
                    { 5, "API.Permission", "ActivityTypes.Create", "01JMA8PAM2JJA5VNGT14KMXVKD" },
                    { 6, "API.Permission", "ActivityTypes.View", "01JMA8PAM2JJA5VNGT14KMXVKD" },
                    { 7, "API.Permission", "ActivityTypes.Edit", "01JMA8PAM2JJA5VNGT14KMXVKD" },
                    { 8, "API.Permission", "ActivityTypes.Delete", "01JMA8PAM2JJA5VNGT14KMXVKD" },
                    { 9, "API.Permission", "Users.View.Workers", "01JMA8PAM2JJA5VNGT14KMXVKD" },
                    { 10, "API.Permission", "Activities.CreateForWorker", "01JMA8PFV7H9ZSGXGE9K4NERZQ" },
                    { 11, "API.Permission", "ActivityTypes.ViewForWorker", "01JMA8PFV7H9ZSGXGE9K4NERZQ" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01JMA8NY8VDH5JP200XJ8EZ4XK", null, "RoleDescription", "Admin", "ADMIN" },
                    { "01JMA8PAM2JJA5VNGT14KMXVKD", null, "Role for Supervisors", "Supervisor", "SUPERVISOR" },
                    { "01JMA8PFV7H9ZSGXGE9K4NERZQ", null, "Role for Construction Site Workers", "Worker", "WORKER" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsAdmin", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiration", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "01JMA8M65MZ4WP2GCF3ZDBV6K9", 0, "fadeea3d-92d6-4c2d-9b94-d8377dc9d476", "admin@gmail.com", true, true, false, null, "Name1", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAELalL5v/Qu2Bi8PQ7P/Biq8pCNBRrJt4VeHGTOXQkZxIJ8cMzSb/YlFSHWHeYET+sQ==", null, false, null, null, "7f3799f1-1251-41b1-9d51-cca055e5c53a", "Surname1", false, "admin" },
                    { "01JMA8MEWWPFKSTT17CPDB14KK", 0, "9469b421-38bc-45d0-a841-36187c7db4c9", "test1@gmail.com", true, false, false, null, "Name1", "TEST1@GMAIL.COM", "SUPERVISOR", "AQAAAAIAAYagAAAAELalL5v/Qu2Bi8PQ7P/Biq8pCNBRrJt4VeHGTOXQkZxIJ8cMzSb/YlFSHWHeYET+sQ==", null, false, null, null, "27c524da-dcc1-4d1f-a8ac-0f84f1da87a0", "Surname1", false, "Supervisor" },
                    { "01JMA8NKFH81EAAMJQA67XW3SV", 0, "c23d4354-15bc-400e-86e0-17bcb943655d", "test2@gmail.com", true, false, false, null, "Name2", "TEST2@GMAIL.COM", "TESTUSER", "AQAAAAIAAYagAAAAELalL5v/Qu2Bi8PQ7P/Biq8pCNBRrJt4VeHGTOXQkZxIJ8cMzSb/YlFSHWHeYET+sQ==", null, false, null, null, "18a24e19-f76e-4022-9f65-a0e673019a12", "Surname2", false, "TestUser" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "01JMA8NY8VDH5JP200XJ8EZ4XK", "01JMA8M65MZ4WP2GCF3ZDBV6K9" },
                    { "01JMA8PAM2JJA5VNGT14KMXVKD", "01JMA8MEWWPFKSTT17CPDB14KK" },
                    { "01JMA8PFV7H9ZSGXGE9K4NERZQ", "01JMA8NKFH81EAAMJQA67XW3SV" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
