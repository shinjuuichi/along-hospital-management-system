using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeleHealthSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class A : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeleRoom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoomDisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeleRoom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeleSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CredentialMetadataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    TeleRoomId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeleSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeleSession_TeleRoom_TeleRoomId",
                        column: x => x.TeleRoomId,
                        principalTable: "TeleRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TeleRoom",
                columns: new[] { "Id", "RoomCode", "RoomDisplayName", "SpecialtyId" },
                values: new object[,]
                {
                    { 1, "TELEROOM_CARDIOLOGY", "Cardiology TeleRoom", 1 },
                    { 2, "TELEROOM_GENERAL_INTERNAL_MEDICINE", "General Internal Medicine TeleRoom", 2 },
                    { 3, "TELEROOM_OBSTETRICS_GYNECOLOGY", "Obstetrics and Gynecology TeleRoom", 3 },
                    { 4, "TELEROOM_ORTHOPEDICS", "Orthopedics TeleRoom", 4 },
                    { 5, "TELEROOM_PEDIATRICS", "Pediatrics TeleRoom", 5 },
                    { 6, "TELEROOM_DERMATOLOGY", "Dermatology TeleRoom", 6 },
                    { 7, "TELEROOM_NEUROLOGY", "Neurology TeleRoom", 7 },
                    { 8, "TELEROOM_OPHTHALMOLOGY", "Ophthalmology TeleRoom", 8 },
                    { 9, "TELEROOM_ENT", "Otolaryngology (ENT) TeleRoom", 9 },
                    { 10, "TELEROOM_UROLOGY", "Urology TeleRoom", 10 },
                    { 11, "TELEROOM_NEPHROLOGY", "Nephrology TeleRoom", 11 },
                    { 12, "TELEROOM_PULMONOLOGY", "Pulmonology TeleRoom", 12 },
                    { 13, "TELEROOM_GASTROENTEROLOGY", "Gastroenterology TeleRoom", 13 },
                    { 14, "TELEROOM_ENDOCRINOLOGY", "Endocrinology TeleRoom", 14 },
                    { 15, "TELEROOM_RHEUMATOLOGY", "Rheumatology TeleRoom", 15 },
                    { 16, "TELEROOM_HEMATOLOGY", "Hematology TeleRoom", 16 },
                    { 17, "TELEROOM_ONCOLOGY", "Oncology TeleRoom", 17 },
                    { 18, "TELEROOM_RADIOLOGY", "Radiology TeleRoom", 18 },
                    { 19, "TELEROOM_PSYCHIATRY", "Psychiatry TeleRoom", 19 },
                    { 20, "TELEROOM_REHABILITATION_MEDICINE", "Rehabilitation Medicine TeleRoom", 20 },
                    { 21, "TELEROOM_ANESTHESIOLOGY", "Anesthesiology TeleRoom", 21 },
                    { 22, "TELEROOM_EMERGENCY_MEDICINE", "Emergency Medicine TeleRoom", 22 },
                    { 23, "TELEROOM_INFECTIOUS_DISEASES", "Infectious Diseases TeleRoom", 23 },
                    { 24, "TELEROOM_FAMILY_MEDICINE", "Family Medicine TeleRoom", 24 },
                    { 25, "TELEROOM_GERIATRICS", "Geriatrics TeleRoom", 25 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeleRoom_RoomCode",
                table: "TeleRoom",
                column: "RoomCode",
                unique: true,
                filter: "\"RoomCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TeleRoom_RoomDisplayName",
                table: "TeleRoom",
                column: "RoomDisplayName",
                unique: true,
                filter: "\"RoomDisplayName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TeleRoom_SpecialtyId",
                table: "TeleRoom",
                column: "SpecialtyId",
                unique: true,
                filter: "\"SpecialtyId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TeleSession_TeleRoomId",
                table: "TeleSession",
                column: "TeleRoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeleSession");

            migrationBuilder.DropTable(
                name: "TeleRoom");
        }
    }
}
