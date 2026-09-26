using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provider = table.Column<int>(type: "int", nullable: false),
                    ProviderUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Stage = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshTokenHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthAccountId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_AuthAccount_AuthAccountId",
                        column: x => x.AuthAccountId,
                        principalTable: "AuthAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuthAccount",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "Email", "IsDeleted", "ModificationDate", "ModifiedBy", "Password", "Phone", "Provider", "ProviderUserId", "Stage", "Status", "UserId" },
                values: new object[,]
                {
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "manager@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0360000002", 3, "", 2, 1, 2 },
                    { 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0360000003", 3, "", 2, 1, 3 },
                    { 4, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "doctor@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0360000004", 3, "", 2, 1, 4 },
                    { 5, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "nurse@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0360000005", 3, "", 2, 1, 5 },
                    { 6, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "hr@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0360000006", 3, "", 2, 1, 6 },
                    { 7, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "pharmacist@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0360000007", 3, "", 2, 1, 7 },
                    { 8, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "accountant@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0360000008", 3, "", 2, 1, 8 },
                    { 9, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "marketer@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0360000009", 3, "", 2, 1, 9 },
                    { 11, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "receptionist@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0360000011", 3, "", 2, 1, 11 },
                    { 12, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "hotlineagent@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0360000012", 3, "", 2, 1, 12 },
                    { 13, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "inventoryclerk@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0360000013", 3, "", 2, 1, 13 },
                    { 14, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "doctor14@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0361000014", 3, "", 2, 1, 14 },
                    { 15, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "doctor15@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0361000015", 3, "", 2, 1, 15 },
                    { 16, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "doctor16@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0361000016", 3, "", 2, 1, 16 },
                    { 17, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "doctor17@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0361000017", 3, "", 2, 1, 17 },
                    { 18, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "doctor18@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0361000018", 3, "", 2, 1, 18 },
                    { 19, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "doctor19@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0361000019", 3, "", 2, 1, 19 },
                    { 20, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "doctor20@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0361000020", 3, "", 2, 1, 20 },
                    { 21, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "doctor21@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0361000021", 3, "", 2, 1, 21 },
                    { 22, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "doctor22@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0361000022", 3, "", 2, 1, 22 },
                    { 39, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient39@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000039", 3, "", 2, 1, 39 },
                    { 40, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient40@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000040", 3, "", 2, 1, 40 },
                    { 41, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient41@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000041", 3, "", 2, 1, 41 },
                    { 42, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient42@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000042", 3, "", 2, 1, 42 },
                    { 43, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient43@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000043", 3, "", 2, 1, 43 },
                    { 44, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient44@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000044", 3, "", 2, 1, 44 },
                    { 45, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient45@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000045", 3, "", 2, 1, 45 },
                    { 46, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient46@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000046", 3, "", 2, 1, 46 },
                    { 47, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient47@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000047", 3, "", 2, 1, 47 },
                    { 48, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient48@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000048", 3, "", 2, 1, 48 },
                    { 49, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient49@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000049", 3, "", 2, 1, 49 },
                    { 50, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient50@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000050", 3, "", 2, 1, 50 },
                    { 51, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient51@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000051", 3, "", 2, 1, 51 },
                    { 52, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient52@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000052", 3, "", 2, 1, 52 },
                    { 53, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient53@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000053", 3, "", 2, 1, 53 },
                    { 54, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient54@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000054", 3, "", 2, 1, 54 },
                    { 55, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient55@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000055", 3, "", 2, 1, 55 },
                    { 56, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient56@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000056", 3, "", 2, 1, 56 },
                    { 57, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient57@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000057", 3, "", 2, 1, 57 },
                    { 58, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient58@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000058", 3, "", 2, 1, 58 },
                    { 59, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient59@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000059", 3, "", 2, 1, 59 },
                    { 60, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient60@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000060", 3, "", 2, 1, 60 },
                    { 61, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient61@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000061", 3, "", 2, 1, 61 },
                    { 62, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient62@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000062", 3, "", 2, 1, 62 },
                    { 63, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "patient63@example.com", false, null, null, "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6", "0362000063", 3, "", 2, 1, 63 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthAccount_Email",
                table: "AuthAccount",
                column: "Email",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuthAccount_Phone",
                table: "AuthAccount",
                column: "Phone",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Phone\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuthAccount_UserId",
                table: "AuthAccount",
                column: "UserId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"UserId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_AuthAccountId",
                table: "RefreshToken",
                column: "AuthAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "AuthAccount");
        }
    }
}
