using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AttendanceSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogType = table.Column<int>(type: "int", nullable: false),
                    LogTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendance", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Attendance",
                columns: new[] { "Id", "LogTime", "LogType", "StaffId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 16, 0, 0, 0, DateTimeKind.Utc), 0, 4 },
                    { 2, new DateTime(2025, 1, 1, 20, 0, 0, 0, DateTimeKind.Utc), 1, 4 },
                    { 3, new DateTime(2025, 1, 2, 1, 0, 0, 0, DateTimeKind.Utc), 1, 4 },
                    { 4, new DateTime(2025, 1, 1, 16, 0, 0, 0, DateTimeKind.Utc), 0, 5 },
                    { 5, new DateTime(2025, 1, 1, 20, 0, 0, 0, DateTimeKind.Utc), 1, 5 },
                    { 6, new DateTime(2025, 1, 2, 1, 0, 0, 0, DateTimeKind.Utc), 1, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendance");
        }
    }
}
