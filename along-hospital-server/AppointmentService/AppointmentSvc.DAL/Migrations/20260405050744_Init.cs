using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppointmentSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeSlot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false),
                    CapacityPerDoctor = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AppointmentStatus = table.Column<int>(type: "int", nullable: false),
                    AppointmentMeetingType = table.Column<int>(type: "int", nullable: false),
                    AppointmentPaymentStatus = table.Column<int>(type: "int", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MedicalHistoryId = table.Column<int>(type: "int", nullable: true),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    TimeSlotId = table.Column<int>(type: "int", nullable: false),
                    TimeSlotSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_TimeSlot_TimeSlotId",
                        column: x => x.TimeSlotId,
                        principalTable: "TimeSlot",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "TimeSlot",
                columns: new[] { "Id", "CapacityPerDoctor", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "Time" },
                values: new object[,]
                {
                    { 1, 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(7, 0, 0) },
                    { 2, 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(7, 30, 0) },
                    { 3, 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(8, 0, 0) },
                    { 4, 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(8, 30, 0) },
                    { 5, 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(9, 0, 0) },
                    { 6, 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(9, 30, 0) },
                    { 7, 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(10, 0, 0) },
                    { 8, 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(10, 30, 0) },
                    { 9, 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(13, 0, 0) },
                    { 10, 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(13, 30, 0) },
                    { 11, 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(14, 0, 0) },
                    { 12, 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(14, 30, 0) },
                    { 13, 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(15, 0, 0) },
                    { 14, 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(15, 30, 0) },
                    { 15, 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(16, 0, 0) },
                    { 16, 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, new TimeOnly(16, 30, 0) }
                });

            migrationBuilder.InsertData(
                table: "Appointment",
                columns: new[] { "Id", "AppointmentMeetingType", "AppointmentPaymentStatus", "AppointmentStatus", "CancelledDate", "CompletedDate", "CreatedBy", "CreationDate", "Date", "DeletionDate", "IsDeleted", "MedicalHistoryId", "ModificationDate", "ModifiedBy", "PatientId", "Purpose", "SpecialtyId", "TimeSlotId", "TimeSlotSnapshot", "TransactionId" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, null, new DateTime(2025, 1, 1, 10, 30, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 1, 1), null, false, 2, null, null, 3, "Follow-up Consultation", 3, 8, "{\"id\":8,\"time\":\"10:30:00\"}", null },
                    { 2, 1, 2, 2, new DateTime(2025, 1, 2, 13, 30, 0, 0, DateTimeKind.Unspecified), null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 1, 2), null, false, null, null, null, 3, "Neurology Checkup", 2, 10, "{\"id\":10,\"time\":\"13:30:00\"}", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_TimeSlotId",
                table: "Appointment",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlot_Time",
                table: "TimeSlot",
                column: "Time",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Time\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "TimeSlot");
        }
    }
}
