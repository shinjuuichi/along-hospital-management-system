using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SupplierSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApprovedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Import",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ImportRequestId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Import", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Import_ImportRequest_ImportRequestId",
                        column: x => x.ImportRequestId,
                        principalTable: "ImportRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImportRequestDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportRequestId = table.Column<int>(type: "int", nullable: false),
                    SKUCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestQuantity = table.Column<int>(type: "int", nullable: false),
                    MedicineSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportRequestDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportRequestDetail_ImportRequest_ImportRequestId",
                        column: x => x.ImportRequestId,
                        principalTable: "ImportRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImportDetail",
                columns: table => new
                {
                    ImportId = table.Column<int>(type: "int", nullable: false),
                    SKUCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportDetail", x => new { x.ImportId, x.SKUCode });
                    table.ForeignKey(
                        name: "FK_ImportDetail_Import_ImportId",
                        column: x => x.ImportId,
                        principalTable: "Import",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ImportRequest",
                columns: new[] { "Id", "ApprovedByUserId", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "RequestDate", "Status" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, new DateOnly(2026, 3, 3), 4 },
                    { 2, 2, null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, new DateOnly(2026, 3, 3), 4 },
                    { 3, 3, null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, new DateOnly(2026, 3, 3), 4 },
                    { 4, null, null, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, new DateOnly(2026, 3, 8), 0 }
                });

            migrationBuilder.InsertData(
                table: "Supplier",
                columns: new[] { "Id", "Address", "CreatedBy", "CreationDate", "DeletionDate", "Email", "IsDeleted", "ModificationDate", "ModifiedBy", "Name", "Note", "Phone" },
                values: new object[,]
                {
                    { 1, "123 Health St, Wellness City", null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "contact@medisupply.example", false, null, null, "MediSupply Co", "Primary supplier for general medicines", "0987284222" },
                    { 2, "45 Pharmacy Ave, Caretown", null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "sales@pharmadirect.example", false, null, null, "PharmaDirect Ltd", "Fast delivery partner", "0987282222" },
                    { 3, "9 Clinic Rd, Healborough", null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "support@healthplus.example", false, null, null, "HealthPlus Distributors", "Specializes in OTC products", "09872382211" }
                });

            migrationBuilder.InsertData(
                table: "Import",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "ImportDate", "ImportRequestId", "IsDeleted", "ManagerId", "ModificationDate", "ModifiedBy", "Note", "SupplierId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, null, null, "Seed Import 1", 1 },
                    { 2, null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 2, null, null, "Seed Import 2", 2 },
                    { 3, null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, 3, null, null, "Seed Import 3", 3 }
                });

            migrationBuilder.InsertData(
                table: "ImportRequestDetail",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "ImportRequestId", "IsDeleted", "MedicineSnapshot", "ModificationDate", "ModifiedBy", "RequestQuantity", "SKUCode" },
                values: new object[,]
                {
                    { 1, 13, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, "{\"medicineName\":\"Paracetamol\",\"unitPrice\":100}", null, null, 100, "PA1BO2050" },
                    { 2, 13, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, "{\"medicineName\":\"Amoxicillin\",\"unitPrice\":100}", null, null, 200, "AM2BO2050" },
                    { 3, 13, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, false, "{\"medicineName\":\"Vitamin C\",\"unitPrice\":100}", null, null, 300, "VC3BO2010" },
                    { 4, 13, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, false, "{\"medicineName\":\"Paracetamol\"}", null, null, 150, "PA1BO3050" }
                });

            migrationBuilder.InsertData(
                table: "ImportDetail",
                columns: new[] { "ImportId", "SKUCode", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, "PA1BO2050", 100, 100m },
                    { 2, "AM2BO2050", 200, 100m },
                    { 3, "VC3BO2010", 300, 100m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Import_ImportRequestId",
                table: "Import",
                column: "ImportRequestId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"ImportRequestId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ImportRequestDetail_ImportRequestId",
                table: "ImportRequestDetail",
                column: "ImportRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Email",
                table: "Supplier",
                column: "Email",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Name",
                table: "Supplier",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Phone",
                table: "Supplier",
                column: "Phone",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Phone\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportDetail");

            migrationBuilder.DropTable(
                name: "ImportRequestDetail");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Import");

            migrationBuilder.DropTable(
                name: "ImportRequest");
        }
    }
}
