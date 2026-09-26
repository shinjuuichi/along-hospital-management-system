using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InpatientResourceSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BedCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomCategoryRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomCategoryRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Floor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FloorNumber = table.Column<int>(type: "int", nullable: false),
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Floor_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomCategoryRoleMapping",
                columns: table => new
                {
                    RoomCategoryRoleId = table.Column<int>(type: "int", nullable: false),
                    RoomCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomCategoryRoleMapping", x => new { x.RoomCategoryRoleId, x.RoomCategoryId });
                    table.ForeignKey(
                        name: "FK_RoomCategoryRoleMapping_RoomCategoryRole_RoomCategoryRoleId",
                        column: x => x.RoomCategoryRoleId,
                        principalTable: "RoomCategoryRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomCategoryRoleMapping_RoomCategory_RoomCategoryId",
                        column: x => x.RoomCategoryId,
                        principalTable: "RoomCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    FloorId = table.Column<int>(type: "int", nullable: false),
                    RoomCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Floor_FloorId",
                        column: x => x.FloorId,
                        principalTable: "Floor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Room_RoomCategory_RoomCategoryId",
                        column: x => x.RoomCategoryId,
                        principalTable: "RoomCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    BedCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bed_BedCategory_BedCategoryId",
                        column: x => x.BedCategoryId,
                        principalTable: "BedCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bed_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BedOccupancy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OccupancyStatus = table.Column<int>(type: "int", nullable: false),
                    TransferNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MedicalHistoryId = table.Column<int>(type: "int", nullable: false),
                    BedId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedOccupancy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BedOccupancy_Bed_BedId",
                        column: x => x.BedId,
                        principalTable: "Bed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BedCategory",
                columns: new[] { "Id", "Code", "CreatedBy", "CreationDate", "DeletionDate", "Description", "IsDeleted", "ModificationDate", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, "BED-STD-001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Basic manual hospital bed", false, null, null, "Standard Bed" },
                    { 2, "BED-ELE-002", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Motorized adjustable hospital bed with remote control", false, null, null, "Electric Bed" },
                    { 3, "BED-ICU-003", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Advanced ICU bed with monitoring capabilities", false, null, null, "ICU Bed" }
                });

            migrationBuilder.InsertData(
                table: "Building",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "Location", "ModificationDate", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Main Campus, North Wing", null, null, "Alpha Building" },
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Main Campus, South Wing", null, null, "Beta Building" },
                    { 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Emergency Wing, East Side", null, null, "Gamma Building" }
                });

            migrationBuilder.InsertData(
                table: "RoomCategory",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "Description", "IsDeleted", "ModificationDate", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Basic hospital room with standard facilities", false, null, null, "Standard Room" },
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Premium room with advanced amenities and privacy", false, null, null, "VIP Room" },
                    { 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Intensive Care Unit for critical patients", false, null, null, "ICU" }
                });

            migrationBuilder.InsertData(
                table: "RoomCategoryRole",
                columns: new[] { "Id", "Role" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 3 },
                    { 3, 4 }
                });

            migrationBuilder.InsertData(
                table: "Floor",
                columns: new[] { "Id", "BuildingId", "CreatedBy", "CreationDate", "DeletionDate", "FloorNumber", "IsDeleted", "ModificationDate", "ModifiedBy" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null },
                    { 2, 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null },
                    { 3, 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "RoomCategoryRoleMapping",
                columns: new[] { "RoomCategoryId", "RoomCategoryRoleId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 2, 2 },
                    { 3, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "Id", "Code", "CreatedBy", "CreationDate", "DeletionDate", "FloorId", "IsDeleted", "ModificationDate", "ModifiedBy", "RoomCategoryId", "SpecialtyId", "Status" },
                values: new object[,]
                {
                    { 1, "A101", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 1, 1, 0 },
                    { 2, "A102", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 2, 2, 0 },
                    { 3, "B201", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 3, 3, 0 },
                    { 4, "A103", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 1, 4, 0 },
                    { 5, "A104", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 1, 5, 0 },
                    { 6, "A105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 1, 6, 0 },
                    { 7, "A106", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 1, 7, 0 },
                    { 8, "A107", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 2, 8, 0 },
                    { 9, "A108", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 1, 9, 0 },
                    { 10, "B202", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 1, 10, 0 },
                    { 11, "B203", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 1, 11, 0 },
                    { 12, "B204", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 1, 12, 0 },
                    { 13, "B205", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 1, 13, 0 },
                    { 14, "B206", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 1, 14, 0 },
                    { 15, "B207", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 1, 15, 0 },
                    { 16, "B208", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 1, 16, 0 },
                    { 17, "B209", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 2, 17, 0 },
                    { 18, "B210", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 1, 18, 0 },
                    { 19, "C101", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, false, null, null, 1, 19, 0 },
                    { 20, "C102", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, false, null, null, 1, 20, 0 },
                    { 21, "C103", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, false, null, null, 3, 21, 0 },
                    { 22, "C104", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, false, null, null, 3, 22, 0 },
                    { 23, "C105", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, false, null, null, 1, 23, 0 },
                    { 24, "C106", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, false, null, null, 1, 24, 0 },
                    { 25, "C107", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, false, null, null, 1, 25, 0 }
                });

            migrationBuilder.InsertData(
                table: "Bed",
                columns: new[] { "Id", "BedCategoryId", "Code", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "RoomId", "Status" },
                values: new object[,]
                {
                    { 1, 1, "A101-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 1, 0 },
                    { 2, 1, "A101-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 1, 2 },
                    { 3, 2, "A102-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 2, 2 },
                    { 4, 2, "A102-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 2, 0 },
                    { 5, 3, "B201-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 3, 0 },
                    { 6, 3, "B201-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 3, 1 },
                    { 7, 1, "A103-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 4, 0 },
                    { 8, 1, "A103-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 4, 0 },
                    { 9, 1, "A104-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 5, 0 },
                    { 10, 1, "A104-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 5, 0 },
                    { 11, 1, "A105-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 6, 0 },
                    { 12, 1, "A105-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 6, 1 },
                    { 13, 1, "A106-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 7, 0 },
                    { 14, 1, "A106-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 7, 0 },
                    { 15, 2, "A107-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 8, 0 },
                    { 16, 2, "A107-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 8, 0 },
                    { 17, 1, "A108-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 9, 0 },
                    { 18, 1, "A108-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 9, 0 },
                    { 19, 1, "B202-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 10, 0 },
                    { 20, 1, "B202-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 10, 0 },
                    { 21, 1, "B203-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 11, 0 },
                    { 22, 1, "B203-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 11, 0 },
                    { 23, 1, "B204-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 12, 0 },
                    { 24, 1, "B204-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 12, 0 },
                    { 25, 1, "B205-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 13, 0 },
                    { 26, 1, "B205-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 13, 1 },
                    { 27, 1, "B206-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 14, 0 },
                    { 28, 1, "B206-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 14, 0 },
                    { 29, 1, "B207-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 15, 0 },
                    { 30, 1, "B207-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 15, 0 },
                    { 31, 1, "B208-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 16, 0 },
                    { 32, 1, "B208-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 16, 0 },
                    { 33, 2, "B209-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 17, 0 },
                    { 34, 2, "B209-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 17, 0 },
                    { 35, 1, "B210-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 18, 0 },
                    { 36, 1, "B210-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 18, 0 },
                    { 37, 1, "C101-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 19, 0 },
                    { 38, 1, "C101-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 19, 0 },
                    { 39, 1, "C102-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 20, 0 },
                    { 40, 1, "C102-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 20, 0 },
                    { 41, 3, "C103-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 21, 0 },
                    { 42, 3, "C103-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 21, 0 },
                    { 43, 3, "C104-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 22, 0 },
                    { 44, 3, "C104-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 22, 0 },
                    { 45, 1, "C105-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 23, 0 },
                    { 46, 1, "C105-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 23, 0 },
                    { 47, 1, "C106-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 24, 0 },
                    { 48, 1, "C106-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 24, 0 },
                    { 49, 1, "C107-01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 25, 0 },
                    { 50, 1, "C107-02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 25, 0 }
                });

            migrationBuilder.InsertData(
                table: "BedOccupancy",
                columns: new[] { "Id", "BedId", "CreatedBy", "CreationDate", "DeletionDate", "FromDateTime", "IsDeleted", "MedicalHistoryId", "ModificationDate", "ModifiedBy", "OccupancyStatus", "ToDateTime", "TransferNote" },
                values: new object[,]
                {
                    { 1, 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3, null, null, 0, null, null },
                    { 2, 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 4, null, null, 0, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bed_BedCategoryId",
                table: "Bed",
                column: "BedCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Bed_Code",
                table: "Bed",
                column: "Code",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Code\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bed_RoomId",
                table: "Bed",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_BedCategory_Code",
                table: "BedCategory",
                column: "Code",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Code\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BedCategory_Name",
                table: "BedCategory",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BedOccupancy_BedId",
                table: "BedOccupancy",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_Building_Name",
                table: "Building",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Floor_BuildingId_FloorNumber",
                table: "Floor",
                columns: new[] { "BuildingId", "FloorNumber" },
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"BuildingId\" IS NOT NULL AND \"FloorNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Room_Code",
                table: "Room",
                column: "Code",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Code\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Room_FloorId",
                table: "Room",
                column: "FloorId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_RoomCategoryId",
                table: "Room",
                column: "RoomCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomCategory_Name",
                table: "RoomCategory",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoomCategoryRole_Role",
                table: "RoomCategoryRole",
                column: "Role",
                unique: true,
                filter: "\"Role\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoomCategoryRoleMapping_RoomCategoryId",
                table: "RoomCategoryRoleMapping",
                column: "RoomCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BedOccupancy");

            migrationBuilder.DropTable(
                name: "RoomCategoryRoleMapping");

            migrationBuilder.DropTable(
                name: "Bed");

            migrationBuilder.DropTable(
                name: "RoomCategoryRole");

            migrationBuilder.DropTable(
                name: "BedCategory");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Floor");

            migrationBuilder.DropTable(
                name: "RoomCategory");

            migrationBuilder.DropTable(
                name: "Building");
        }
    }
}
