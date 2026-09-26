using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventorySvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    LastImportDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MinQuantity = table.Column<int>(type: "int", nullable: true),
                    MaxQuantity = table.Column<int>(type: "int", nullable: true),
                    SKUCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "LastImportDate", "MaxQuantity", "MinQuantity", "ModificationDate", "ModifiedBy", "Quantity", "SKUCode" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1000, 50, null, null, 500, "PA1BO2050" },
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 800, 30, null, null, 300, "PA1BO3050" },
                    { 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 500, 20, null, null, 120, "AM2BO2050" },
                    { 4, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 500, 30, null, null, 200, "AM2BL1050" },
                    { 5, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 400, 20, null, null, 150, "VC3BO2010" },
                    { 6, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 900, 40, null, null, 480, "PA1BL1250" },
                    { 7, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), 500, 25, null, null, 220, "VC3BO2050" },
                    { 8, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), 350, 20, null, null, 140, "AM3BO3010" },
                    { 9, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 7, 0, 0, 0, 0, DateTimeKind.Utc), 300, 15, null, null, 100, "AM3BO3260" },
                    { 10, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), 250, 15, null, null, 95, "BI5BO1050" },
                    { 11, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), 280, 20, null, null, 130, "DI6BO2050" },
                    { 12, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 420, 30, null, null, 180, "NA4BO2550" },
                    { 13, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), 220, 10, null, null, 75, "NA7BO5010" },
                    { 14, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), 520, 35, null, null, 260, "OR8BO2032" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventory");
        }
    }
}
