using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CartSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartDetails",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false),
                    SKUCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetails", x => new { x.CartId, x.SKUCode });
                    table.ForeignKey(
                        name: "FK_CartDetails_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Cart",
                columns: new[] { "Id", "PatientId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 2, 39 },
                    { 3, 40 },
                    { 4, 41 },
                    { 5, 42 },
                    { 6, 43 },
                    { 7, 44 },
                    { 8, 45 },
                    { 9, 46 },
                    { 10, 47 },
                    { 11, 48 },
                    { 12, 49 },
                    { 13, 50 },
                    { 14, 51 },
                    { 15, 52 },
                    { 16, 53 },
                    { 17, 54 },
                    { 18, 55 },
                    { 19, 56 },
                    { 20, 57 },
                    { 21, 58 },
                    { 22, 59 },
                    { 23, 60 },
                    { 24, 61 },
                    { 25, 62 },
                    { 26, 63 }
                });

            migrationBuilder.InsertData(
                table: "CartDetails",
                columns: new[] { "CartId", "SKUCode", "Quantity" },
                values: new object[,]
                {
                    { 1, "AM2BO2050", 3 },
                    { 1, "PA1BO2050", 2 },
                    { 1, "PA1BO3050", 1 },
                    { 2, "AM2BL1050", 1 },
                    { 2, "PA1BO2050", 2 },
                    { 2, "VC3BO2010", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartDetails");

            migrationBuilder.DropTable(
                name: "Cart");
        }
    }
}
