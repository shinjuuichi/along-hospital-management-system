using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatientSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MedicalNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    BloodType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Allergy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeverityLevel = table.Column<int>(type: "int", nullable: false),
                    Reaction = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allergy_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Patient",
                columns: new[] { "Id", "BloodType", "Height", "MedicalNumber", "Weight" },
                values: new object[,]
                {
                    { 3, 1, 175, "PT-2025-000003", 70m },
                    { 39, 1, 155, "PT-2025-000039", 48m },
                    { 40, 2, 156, "PT-2025-000040", 49m },
                    { 41, 3, 157, "PT-2025-000041", 50m },
                    { 42, 4, 158, "PT-2025-000042", 51m },
                    { 43, 5, 159, "PT-2025-000043", 52m },
                    { 44, 1, 160, "PT-2025-000044", 53m },
                    { 45, 2, 161, "PT-2025-000045", 54m },
                    { 46, 3, 162, "PT-2025-000046", 55m },
                    { 47, 4, 163, "PT-2025-000047", 56m },
                    { 48, 5, 164, "PT-2025-000048", 57m },
                    { 49, 1, 165, "PT-2025-000049", 58m },
                    { 50, 2, 166, "PT-2025-000050", 59m },
                    { 51, 3, 167, "PT-2025-000051", 60m },
                    { 52, 4, 168, "PT-2025-000052", 61m },
                    { 53, 5, 169, "PT-2025-000053", 62m },
                    { 54, 1, 170, "PT-2025-000054", 63m },
                    { 55, 2, 171, "PT-2025-000055", 64m },
                    { 56, 3, 172, "PT-2025-000056", 65m },
                    { 57, 4, 173, "PT-2025-000057", 66m },
                    { 58, 5, 174, "PT-2025-000058", 67m },
                    { 59, 1, 155, "PT-2025-000059", 68m },
                    { 60, 2, 156, "PT-2025-000060", 69m },
                    { 61, 3, 157, "PT-2025-000061", 70m },
                    { 62, 4, 158, "PT-2025-000062", 71m },
                    { 63, 5, 159, "PT-2025-000063", 72m }
                });

            migrationBuilder.InsertData(
                table: "Allergy",
                columns: new[] { "Id", "Name", "PatientId", "Reaction", "SeverityLevel" },
                values: new object[,]
                {
                    { 1, "Penicillin", 3, "Anaphylaxis", 3 },
                    { 2, "Dust", 3, "Sneezing, watery eyes", 1 },
                    { 3, "Seafood", 3, "Rash, itching", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allergy_PatientId",
                table: "Allergy",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_MedicalNumber",
                table: "Patient",
                column: "MedicalNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allergy");

            migrationBuilder.DropTable(
                name: "Patient");
        }
    }
}
