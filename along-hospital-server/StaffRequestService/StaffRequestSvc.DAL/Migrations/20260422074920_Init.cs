using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StaffRequestSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaveRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveType = table.Column<int>(type: "int", nullable: false),
                    FromDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ToDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DecidedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeaveUnit = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: true),
                    DecidedBy = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryAdvance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DecidedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DecidedBy = table.Column<int>(type: "int", nullable: true),
                    PayrollId = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryAdvance", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "LeaveRequest",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DecidedAt", "DecidedBy", "DeletionDate", "FromDate", "IsDeleted", "LeaveType", "LeaveUnit", "ModificationDate", "ModifiedBy", "Reason", "ShiftId", "Status", "ToDate" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 5, 10, 0, 0, 0, DateTimeKind.Utc), 1, null, new DateOnly(2026, 3, 10), false, 0, 0, null, null, "Family vacation", null, 1, new DateOnly(2026, 3, 12) },
                    { 2, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new DateOnly(2026, 3, 15), false, 1, 0, null, null, "Medical checkup", null, 0, new DateOnly(2026, 3, 15) },
                    { 3, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 18, 14, 0, 0, 0, DateTimeKind.Utc), 1, null, new DateOnly(2026, 3, 20), false, 3, 1, null, null, "Personal errand", 1, 2, new DateOnly(2026, 3, 20) },
                    { 4, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 25, 9, 0, 0, 0, DateTimeKind.Utc), 1, null, new DateOnly(2026, 4, 1), false, 2, 0, null, null, "Maternity leave", null, 1, new DateOnly(2026, 6, 30) },
                    { 5, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new DateOnly(2026, 3, 25), false, 0, 0, null, null, "Travel plan", null, 0, new DateOnly(2026, 3, 28) }
                });

            migrationBuilder.InsertData(
                table: "SalaryAdvance",
                columns: new[] { "Id", "Amount", "CreatedBy", "CreationDate", "DecidedAt", "DecidedBy", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "PayrollId", "Reason", "Status" },
                values: new object[,]
                {
                    { 1, 88m, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, null, null, null, "Pending advance for family expense", 0 },
                    { 2, 128m, 4, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, null, "Approved advance for rent payment", 1 },
                    { 3, 72m, 4, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, null, "Rejected advance request", 2 },
                    { 4, 60m, 4, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, null, null, null, "Cancelled by doctor before approval", 3 },
                    { 5, 140m, 4, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, 2, "Disbursed advance linked to paid payroll", 4 },
                    { 6, 260m, 2, new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, false, null, null, null, "Advance for monthly leadership expenses rejected after review", 2 },
                    { 7, 88m, 5, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, null, null, null, "Advance for nursing family support pending review", 0 },
                    { 8, 180m, 6, new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, null, "Advance for annual training fee approved by management", 1 },
                    { 9, 112m, 7, new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, null, "Advance for pharmacy licensing cost rejected after review", 2 },
                    { 10, 128m, 8, new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, null, null, null, "Advance for household payment plan cancelled by staff", 3 },
                    { 11, 100m, 9, new DateTime(2025, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, 16, "Advance for campaign travel expense disbursed with payroll", 4 },
                    { 12, 72m, 11, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, null, "Advance for front desk living expense approved by management", 1 },
                    { 13, 80m, 12, new DateTime(2025, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, null, "Advance for hotline family emergency rejected after review", 2 },
                    { 14, 92m, 13, new DateTime(2025, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, null, null, null, "Advance for warehouse transportation cost cancelled by staff", 3 },
                    { 15, 140m, 14, new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, 24, "Advance for specialist practice expense #14 disbursed with payroll", 4 },
                    { 16, 164m, 15, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, null, null, null, "Advance for specialist practice expense #15 pending review", 0 },
                    { 17, 188m, 16, new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, null, "Advance for specialist practice expense #16 approved by management", 1 },
                    { 18, 212m, 17, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, null, "Advance for specialist practice expense #17 rejected after review", 2 },
                    { 19, 236m, 18, new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, null, null, null, "Advance for specialist practice expense #18 cancelled by staff", 3 },
                    { 20, 140m, 19, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, 34, "Advance for specialist practice expense #19 disbursed with payroll", 4 },
                    { 21, 164m, 20, new DateTime(2025, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, null, null, null, "Advance for specialist practice expense #20 pending review", 0 },
                    { 22, 188m, 21, new DateTime(2025, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, null, "Advance for specialist practice expense #21 approved by management", 1 },
                    { 23, 212m, 22, new DateTime(2025, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, null, "Advance for specialist practice expense #22 rejected after review", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveRequest");

            migrationBuilder.DropTable(
                name: "SalaryAdvance");
        }
    }
}
