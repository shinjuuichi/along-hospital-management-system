using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VoucherCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OriginPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FinalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsPickupAtStore = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    SKUCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    MedicineSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => new { x.OrderId, x.SKUCode });
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "DeliveryDate", "FinalPrice", "IsDeleted", "IsPickupAtStore", "ModificationDate", "ModifiedBy", "OrderDate", "OrderStatus", "OriginPrice", "PaidDate", "PatientId", "TotalDiscountAmount", "TransactionId", "VoucherCode" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 5.5m, false, false, null, null, new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 5.5m, null, 3, 0m, null, null },
                    { 2, null, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 2.55m, false, true, null, null, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3m, new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 0.45m, null, null },
                    { 3, null, new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 7.1m, false, false, null, null, new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 8m, new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 0.9m, null, "PV-WELCOME10" },
                    { 4, null, new DateTime(2025, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1.2m, false, true, null, null, new DateTime(2025, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1.2m, null, 3, 0m, null, null },
                    { 5, null, new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 4.05m, false, false, null, null, new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4.5m, new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 0.45m, null, null },
                    { 6, null, new DateTime(2025, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0m, false, true, null, null, new DateTime(2025, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3m, new DateTime(2025, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3m, null, "PV-FIRST5" }
                });

            migrationBuilder.InsertData(
                table: "OrderDetail",
                columns: new[] { "OrderId", "SKUCode", "DiscountAmount", "MedicineSnapshot", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, "AM2BO2050", 0m, "{\"medicineName\":\"Amoxicillin 500\",\"medicineBrand\":\"Medipharma\",\"medicineImages\":[\"Medicine/amoxicillin_1770294305_24f46c57.webp\"],\"medicineUnit\":\"Capsule\"}", 1, 2.5m },
                    { 1, "PA1BO2050", 0m, "{\"medicineName\":\"Paracetamol 500\",\"medicineBrand\":\"Stada\",\"medicineImages\":[\"Medicine/thuoc-paracetamol-650-mg-mediplantex-3-c1880_1770294316_bdee440d.webp\"],\"medicineUnit\":\"Tablet\"}", 2, 1.5m },
                    { 2, "VC3BO2050", 0.45m, "{\"medicineName\":\"Vitamin C 1000\",\"medicineBrand\":\"Nature\\u0027s Way\",\"medicineImages\":[\"Medicine/VitaminC_1770294282_a543fa89.webp\"],\"medicineUnit\":\"Effervescent\"}", 1, 3m },
                    { 3, "PA1BO3050", 0m, "{\"medicineName\":\"Paracetamol 500\",\"medicineBrand\":\"Stada\",\"medicineImages\":[\"Medicine/thuoc-paracetamol-650-mg-mediplantex-3-c1880_1770294316_bdee440d.webp\"],\"medicineUnit\":\"Tablet\"}", 1, 2m },
                    { 3, "VC3BO2050", 0.45m, "{\"medicineName\":\"Vitamin C 1000\",\"medicineBrand\":\"Nature\\u0027s Way\",\"medicineImages\":[\"Medicine/VitaminC_1770294282_a543fa89.webp\"],\"medicineUnit\":\"Effervescent\"}", 2, 3m },
                    { 4, "AM2BO1050", 0m, "{\"medicineName\":\"Amoxicillin 500\",\"medicineBrand\":\"Medipharma\",\"medicineImages\":[\"Medicine/amoxicillin_1770294305_24f46c57.webp\"],\"medicineUnit\":\"Capsule\"}", 1, 1.2m },
                    { 5, "PA1BO2050", 0m, "{\"medicineName\":\"Paracetamol 500\",\"medicineBrand\":\"Stada\",\"medicineImages\":[\"Medicine/thuoc-paracetamol-650-mg-mediplantex-3-c1880_1770294316_bdee440d.webp\"],\"medicineUnit\":\"Tablet\"}", 1, 1.5m },
                    { 5, "VC3BO2050", 0.45m, "{\"medicineName\":\"Vitamin C 1000\",\"medicineBrand\":\"Nature\\u0027s Way\",\"medicineImages\":[\"Medicine/VitaminC_1770294282_a543fa89.webp\"],\"medicineUnit\":\"Effervescent\"}", 1, 3m },
                    { 6, "VC3BO2050", 3m, "{\"medicineName\":\"Vitamin C 1000\",\"medicineBrand\":\"Nature\\u0027s Way\",\"medicineImages\":[\"Medicine/VitaminC_1770294282_a543fa89.webp\"],\"medicineUnit\":\"Effervescent\"}", 1, 3m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
