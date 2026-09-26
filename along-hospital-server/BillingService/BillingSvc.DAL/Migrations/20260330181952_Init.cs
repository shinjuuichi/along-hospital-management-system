using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BillingSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvoiceStatus = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MedicalHistoryId = table.Column<int>(type: "int", nullable: false),
                    ClinicalMedicalOrderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Charge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ChargeType = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    MedicalServiceId = table.Column<int>(type: "int", nullable: false),
                    ChargeSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Charge_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Refund",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RefundStatus = table.Column<int>(type: "int", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    ClinicalMedicalOrderDetailId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChargeId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refund_Charge_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "Charge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Invoice",
                columns: new[] { "Id", "ClinicalMedicalOrderId", "CreatedBy", "CreationDate", "DeletionDate", "InvoiceNumber", "InvoiceStatus", "IsDeleted", "MedicalHistoryId", "ModificationDate", "ModifiedBy", "PaymentDate", "TransactionId" },
                values: new object[,]
                {
                    { 1, "64f000000000000000000001", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "INV-OPD-20260101-000000", 1, false, 1, null, null, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, null, null, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "INV-OPD-20260102-000000", 1, false, 1, null, null, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, "64f000000000000000000003", null, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "INV-IPD-20260103-000000", 1, false, 2, null, null, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Charge",
                columns: new[] { "Id", "ChargeSnapshot", "ChargeType", "CreatedBy", "CreationDate", "DeletionDate", "InvoiceId", "IsDeleted", "MedicalServiceId", "ModificationDate", "ModifiedBy", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, "{\"medicalServiceName\":\"General Health Check\",\"medicalServiceDescription\":\"Comprehensive periodic health check-up\"}", 0, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, false, 1, null, null, 1, 500m },
                    { 2, "{\"medicalServiceName\":\"Cardiology Consultation\",\"medicalServiceDescription\":\"Heart and vascular health consultation\"}", 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, false, 2, null, null, 1, 700m },
                    { 3, "{\"medicalServiceName\":\"Dermatology Consultation\",\"medicalServiceDescription\":\"Skin health and treatment consultation\"}", 0, null, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, false, 3, null, null, 1, 600m },
                    { 4, "{\"medicalServiceName\":\"General Health Check\",\"medicalServiceDescription\":\"Comprehensive periodic health check-up\"}", 0, null, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, false, 1, null, null, 2, 500m }
                });

            migrationBuilder.InsertData(
                table: "Refund",
                columns: new[] { "Id", "ApprovalDate", "ApprovedBy", "ChargeId", "ClinicalMedicalOrderDetailId", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "Reason", "RefundStatus" },
                values: new object[] { 1, null, null, 2, "64f000000000000000000001", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "Failed to do service due to technical issue", 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Charge_InvoiceId",
                table: "Charge",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_InvoiceNumber",
                table: "Invoice",
                column: "InvoiceNumber",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"InvoiceNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Refund_ChargeId",
                table: "Refund",
                column: "ChargeId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"ChargeId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Refund");

            migrationBuilder.DropTable(
                name: "Charge");

            migrationBuilder.DropTable(
                name: "Invoice");
        }
    }
}
