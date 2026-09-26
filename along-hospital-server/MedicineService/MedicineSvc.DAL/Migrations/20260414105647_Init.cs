using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicineSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicineCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicineUnit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineUnit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Images = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MedicineUnitId = table.Column<int>(type: "int", nullable: false),
                    MedicineCategoryId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicine_MedicineCategory_MedicineCategoryId",
                        column: x => x.MedicineCategoryId,
                        principalTable: "MedicineCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Medicine_MedicineUnit_MedicineUnitId",
                        column: x => x.MedicineUnitId,
                        principalTable: "MedicineUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicineUnitOption",
                columns: table => new
                {
                    MedicineUnitId = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineUnitOption", x => new { x.MedicineUnitId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_MedicineUnitOption_MedicineUnit_MedicineUnitId",
                        column: x => x.MedicineUnitId,
                        principalTable: "MedicineUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicineUnitOption_Option_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Option",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OptionValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValueName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnitMultiplier = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionValue_Option_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Option",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicineSKU",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SKUCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineSKU", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineSKU_Medicine_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SKUValue",
                columns: table => new
                {
                    MedicineSKUId = table.Column<int>(type: "int", nullable: false),
                    OptionValueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SKUValue", x => new { x.MedicineSKUId, x.OptionValueId });
                    table.ForeignKey(
                        name: "FK_SKUValue_MedicineSKU_MedicineSKUId",
                        column: x => x.MedicineSKUId,
                        principalTable: "MedicineSKU",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SKUValue_OptionValue_OptionValueId",
                        column: x => x.OptionValueId,
                        principalTable: "OptionValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MedicineCategory",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Used to treat bacterial infections.", "Antibiotics" },
                    { 2, "Used to relieve pain and reduce fever.", "Painkillers" },
                    { 3, "Used to support health and nutrition.", "Vitamins & Supplements" },
                    { 4, "Intravenous fluids and infusion solutions for hydration and therapy.", "Infusion Solutions" }
                });

            migrationBuilder.InsertData(
                table: "MedicineUnit",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Solid oral dosage form pressed into flat shape", "Tablet" },
                    { 2, "Solid dosage form enclosed in gelatin shell", "Capsule" },
                    { 3, "Sweet liquid oral medication", "Syrup" },
                    { 4, "Sterile solution for parenteral administration", "Injection" },
                    { 5, "Sterile liquid for ophthalmic use", "Eye Drops" },
                    { 6, "Semi-solid topical preparation", "Ointment" },
                    { 7, "Fine dry particles for oral or topical use", "Powder" },
                    { 8, "Liquid with undissolved particles", "Suspension" },
                    { 9, "Soluble tablet that fizzes when dissolved", "Effervescent" },
                    { 10, "Aerosol or pump dispenser for topical or oral use", "Spray" },
                    { 11, "Solid dosage form for rectal or vaginal insertion", "Suppository" }
                });

            migrationBuilder.InsertData(
                table: "Option",
                columns: new[] { "Id", "OptionName" },
                values: new object[,]
                {
                    { 1, "PackagingType" },
                    { 2, "PackQuantity" },
                    { 3, "Dosage" },
                    { 4, "Volume" },
                    { 5, "Concentration" }
                });

            migrationBuilder.InsertData(
                table: "Medicine",
                columns: new[] { "Id", "Brand", "CreatedBy", "CreationDate", "DeletionDate", "Images", "IsDeleted", "IsPublic", "MedicineCategoryId", "MedicineUnitId", "ModificationDate", "ModifiedBy", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "Stada", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "[\"Medicine/thuoc-paracetamol-650-mg-mediplantex-3-c1880_1770294316_bdee440d.webp\"]", false, true, 2, 1, null, null, "Paracetamol 500", 1 },
                    { 2, "Medipharma", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "[\"Medicine/amoxicillin_1770294305_24f46c57.webp\"]", false, false, 1, 2, null, null, "Amoxicillin 500", 1 },
                    { 3, "Nature's Way", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "[\"Medicine/VitaminC_1770294282_a543fa89.webp\"]", false, true, 3, 9, null, null, "Vitamin C 1000", 1 },
                    { 4, "Medipharma", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "[\"Medicine/001f8ae49f5449feaed195262defce06.webp\"]", false, false, 1, 3, null, null, "Amoxicillin Syrup", 1 },
                    { 5, "Optimax", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "[\"Medicine/cbd27a962c2c4107bf8a4adcf97f0a75.webp\"]", false, true, 1, 5, null, null, "Bivolet Eye Drops", 1 },
                    { 6, "Schering", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "[\"Medicine/808d7e699e5a4d4cbcdd8eeeadcbebf5.webp\"]", false, false, 2, 6, null, null, "Diprosone Ointment", 1 },
                    { 7, "B Braun", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "[\"Medicine/1b6f35f7a68d4bea831ae57255ed1f6a.webp\"]", false, false, 4, 4, null, null, "NaCl 0.9% Infusion", 1 },
                    { 8, "Vimed", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "[\"Medicine/08f0a64c6ca840d0a0f29d3ed5260ca7.webp\"]", false, true, 2, 7, null, null, "Oresol Powder", 1 }
                });

            migrationBuilder.InsertData(
                table: "MedicineUnitOption",
                columns: new[] { "MedicineUnitId", "OptionId", "IsActive" },
                values: new object[,]
                {
                    { 1, 1, true },
                    { 1, 2, true },
                    { 1, 3, true },
                    { 2, 1, true },
                    { 2, 2, true },
                    { 2, 3, true },
                    { 3, 1, true },
                    { 3, 4, true },
                    { 3, 5, true },
                    { 4, 1, true },
                    { 4, 3, true },
                    { 4, 4, true },
                    { 5, 1, true },
                    { 5, 4, true },
                    { 5, 5, true },
                    { 6, 1, true },
                    { 6, 2, true },
                    { 6, 5, true },
                    { 7, 1, true },
                    { 7, 2, true },
                    { 7, 3, true },
                    { 8, 1, true },
                    { 8, 4, true },
                    { 8, 5, true },
                    { 9, 1, true },
                    { 9, 2, true },
                    { 9, 3, true },
                    { 10, 1, true },
                    { 10, 4, true },
                    { 10, 5, true },
                    { 11, 1, true },
                    { 11, 2, true }
                });

            migrationBuilder.InsertData(
                table: "OptionValue",
                columns: new[] { "Id", "IsActive", "OptionId", "UnitMultiplier", "ValueName" },
                values: new object[,]
                {
                    { 1, true, 1, 1, "Box" },
                    { 2, true, 1, 1, "Blister" },
                    { 3, true, 1, 1, "Bottle" },
                    { 4, true, 1, 1, "Sachet" },
                    { 5, true, 1, 1, "Tube" },
                    { 6, true, 1, 1, "Packet" },
                    { 7, true, 2, 1, "1 per blister" },
                    { 8, true, 2, 6, "6 per blister" },
                    { 9, true, 2, 10, "10 per blister" },
                    { 10, true, 2, 12, "12 per blister" },
                    { 11, true, 2, 20, "20 per box" },
                    { 12, true, 2, 30, "30 per box" },
                    { 13, true, 2, 50, "50 per box" },
                    { 14, true, 2, 100, "100 per box" },
                    { 15, true, 2, 5, "5ml per bottle" },
                    { 16, true, 2, 10, "10ml per bottle" },
                    { 17, true, 2, 30, "30ml per bottle" },
                    { 18, true, 2, 60, "60ml per bottle" },
                    { 19, true, 2, 100, "100ml per bottle" },
                    { 20, true, 3, 1, "10mg" },
                    { 21, true, 3, 1, "80mg" },
                    { 22, true, 3, 1, "100mg" },
                    { 23, true, 3, 1, "250mg" },
                    { 24, true, 3, 1, "325mg" },
                    { 25, true, 3, 1, "500mg" },
                    { 26, true, 3, 1, "650mg" },
                    { 27, true, 3, 1, "1000mg" },
                    { 28, true, 4, 5, "5ml" },
                    { 29, true, 4, 10, "10ml" },
                    { 30, true, 4, 30, "30ml" },
                    { 31, true, 4, 60, "60ml" },
                    { 32, true, 4, 100, "100ml" },
                    { 33, true, 4, 250, "250ml" },
                    { 34, true, 4, 500, "500ml" },
                    { 35, true, 5, 1, "5mg/ml" },
                    { 36, true, 5, 1, "10mg/ml" },
                    { 37, true, 5, 1, "50mg/ml" },
                    { 38, true, 5, 1, "100mg/ml" },
                    { 39, true, 5, 1, "5%" },
                    { 40, true, 5, 1, "10%" }
                });

            migrationBuilder.InsertData(
                table: "MedicineSKU",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "IsActive", "IsDeleted", "MedicineId", "ModificationDate", "ModifiedBy", "Name", "Price", "SKUCode" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 1, null, null, "Box 20 tablets", 1.5m, "PA1BO2050" },
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 1, null, null, "Box 30 tablets", 2m, "PA1BO3050" },
                    { 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 1, null, null, "Blister 12 tablets", 0.8m, "PA1BL1250" },
                    { 4, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 2, null, null, "Box 20 capsules", 2.5m, "AM2BO2050" },
                    { 5, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 2, null, null, "Blister 10 capsules", 1.2m, "AM2BL1050" },
                    { 6, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 3, null, null, "Box 20 tablets", 3m, "VC3BO2050" },
                    { 7, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 3, null, null, "Box 30 tablets", 4.5m, "VC3BO2010" },
                    { 8, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 4, null, null, "Bottle 30ml", 4m, "AM3BO3010" },
                    { 9, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 4, null, null, "Bottle 60ml", 6.5m, "AM3BO3260" },
                    { 10, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 5, null, null, "Bottle 10ml", 3.5m, "BI5BO1050" },
                    { 11, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 6, null, null, "Box 20 units", 2.8m, "DI6BO2050" },
                    { 12, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 4, null, null, "Bottle 250ml", 5m, "NA4BO2550" },
                    { 13, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 7, null, null, "Bottle 500ml", 12m, "NA7BO5010" },
                    { 14, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, 8, null, null, "Box 20 sachets", 0.5m, "OR8BO2032" }
                });

            migrationBuilder.InsertData(
                table: "SKUValue",
                columns: new[] { "MedicineSKUId", "OptionValueId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 11 },
                    { 1, 25 },
                    { 2, 1 },
                    { 2, 12 },
                    { 2, 25 },
                    { 3, 2 },
                    { 3, 10 },
                    { 3, 25 },
                    { 4, 1 },
                    { 4, 11 },
                    { 4, 25 },
                    { 5, 2 },
                    { 5, 9 },
                    { 5, 25 },
                    { 6, 1 },
                    { 6, 11 },
                    { 6, 27 },
                    { 7, 1 },
                    { 7, 12 },
                    { 7, 27 },
                    { 8, 1 },
                    { 8, 17 },
                    { 8, 36 },
                    { 9, 1 },
                    { 9, 18 },
                    { 9, 39 },
                    { 10, 1 },
                    { 10, 29 },
                    { 10, 39 },
                    { 11, 1 },
                    { 11, 11 },
                    { 11, 39 },
                    { 12, 1 },
                    { 12, 33 },
                    { 12, 37 },
                    { 13, 1 },
                    { 13, 34 },
                    { 13, 38 },
                    { 14, 1 },
                    { 14, 11 },
                    { 14, 24 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medicine_MedicineCategoryId",
                table: "Medicine",
                column: "MedicineCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicine_MedicineUnitId",
                table: "Medicine",
                column: "MedicineUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicine_Name",
                table: "Medicine",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineCategory_Name",
                table: "MedicineCategory",
                column: "Name",
                unique: true,
                filter: "\"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineSKU_MedicineId",
                table: "MedicineSKU",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineSKU_SKUCode",
                table: "MedicineSKU",
                column: "SKUCode",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"SKUCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineUnit_Name",
                table: "MedicineUnit",
                column: "Name",
                unique: true,
                filter: "\"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineUnitOption_OptionId",
                table: "MedicineUnitOption",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_OptionName",
                table: "Option",
                column: "OptionName",
                unique: true,
                filter: "\"OptionName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OptionValue_OptionId",
                table: "OptionValue",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionValue_ValueName",
                table: "OptionValue",
                column: "ValueName",
                unique: true,
                filter: "\"ValueName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SKUValue_OptionValueId",
                table: "SKUValue",
                column: "OptionValueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicineUnitOption");

            migrationBuilder.DropTable(
                name: "SKUValue");

            migrationBuilder.DropTable(
                name: "MedicineSKU");

            migrationBuilder.DropTable(
                name: "OptionValue");

            migrationBuilder.DropTable(
                name: "Medicine");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "MedicineCategory");

            migrationBuilder.DropTable(
                name: "MedicineUnit");
        }
    }
}
