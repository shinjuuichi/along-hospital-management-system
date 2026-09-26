using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicalServiceSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalServiceRole",
                columns: table => new
                {
                    MedicalServiceId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalServiceRole", x => new { x.MedicalServiceId, x.Role });
                    table.ForeignKey(
                        name: "FK_MedicalServiceRole_MedicalService_MedicalServiceId",
                        column: x => x.MedicalServiceId,
                        principalTable: "MedicalService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MedicalService",
                columns: new[] { "Id", "Code", "CreatedBy", "CreationDate", "DeletionDate", "Description", "IsActive", "IsDeleted", "ModificationDate", "ModifiedBy", "Name", "Price", "SpecialtyId" },
                values: new object[,]
                {
                    { 1, "GHC101", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Comprehensive periodic health check-up", true, false, null, null, "General Health Check", 0.1m, 1 },
                    { 2, "CC102", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Heart and vascular health consultation", true, false, null, null, "Cardiology Consultation", 0.1m, 2 },
                    { 3, "DE103", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Skin care and dermatological check", true, false, null, null, "Dermatology Examination", 0.1m, 3 },
                    { 4, "INF001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Intravenous infusion therapy service", true, false, null, null, "Infusion Service", 0.15m, 1 },
                    { 5, "TH001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Online consultation service for telehealth appointments", true, false, null, null, "Telehealth Appointment", 0.1m, 2 },
                    { 6, "BED-STD-001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Daily inpatient charge for a standard hospital bed", true, false, null, null, "Standard Bed Daily Charge", 10m, 1 },
                    { 7, "BED-ELE-002", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Daily inpatient charge for an electric hospital bed", true, false, null, null, "Electric Bed Daily Charge", 5m, 1 },
                    { 8, "BED-ICU-003", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Daily inpatient charge for an ICU hospital bed", true, false, null, null, "ICU Bed Daily Charge", 20m, 1 }
                });

            migrationBuilder.InsertData(
                table: "MedicalServiceRole",
                columns: new[] { "MedicalServiceId", "Role" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 1, 4 },
                    { 2, 3 },
                    { 3, 3 },
                    { 4, 3 },
                    { 5, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalServiceRole");

            migrationBuilder.DropTable(
                name: "MedicalService");
        }
    }
}
