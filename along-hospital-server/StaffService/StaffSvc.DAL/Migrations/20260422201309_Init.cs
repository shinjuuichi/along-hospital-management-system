using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StaffSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Qualification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionalWage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    MonthlyWage = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalWage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsMedical = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaffCertificateType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ScopeOfPractice = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffCertificateType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaffGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BankCode = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DependentQuantity = table.Column<int>(type: "int", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    QualificationId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_Qualification_QualificationId",
                        column: x => x.QualificationId,
                        principalTable: "Qualification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staff_Specialty_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffCertificate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificateNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssuedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiredDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IssuedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StaffCertificateTypeId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffCertificate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffCertificate_StaffCertificateType_StaffCertificateTypeId",
                        column: x => x.StaffCertificateTypeId,
                        principalTable: "StaffCertificateType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffCertificate_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffContract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContractType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    HourlyRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    WorkingHoursPerWeek = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SignedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SignatureImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    InsuranceSalaryRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    RegionalWageId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffContract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffContract_RegionalWage_RegionalWageId",
                        column: x => x.RegionalWageId,
                        principalTable: "RegionalWage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffContract_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffGroupMember",
                columns: table => new
                {
                    StaffGroupId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffGroupMember", x => new { x.StaffGroupId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_StaffGroupMember_StaffGroup_StaffGroupId",
                        column: x => x.StaffGroupId,
                        principalTable: "StaffGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffGroupMember_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Qualification",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "Description", "IsDeleted", "ModificationDate", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Basic licensed doctor", false, null, null, "General Practitioner" },
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Specialist certification level I", false, null, null, "Specialist Level I" },
                    { 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Specialist certification level II", false, null, null, "Specialist Level II" },
                    { 4, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Master degree in medicine", false, null, null, "Master of Medicine" },
                    { 5, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "PhD in medical science", false, null, null, "Doctor of Philosophy" },
                    { 6, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Doctor in residency training", false, null, null, "Resident Doctor" },
                    { 7, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Senior consulting doctor", false, null, null, "Consultant Physician" },
                    { 8, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Certified surgical doctor", false, null, null, "Surgeon" },
                    { 9, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Teaching medical staff", false, null, null, "Medical Lecturer" },
                    { 10, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Medical research specialist", false, null, null, "Clinical Researcher" },
                    { 11, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Specialist in public health", false, null, null, "Public Health Specialist" },
                    { 12, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Healthcare management professional", false, null, null, "Hospital Administrator" },
                    { 13, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Medical laboratory technology specialist", false, null, null, "Medical Technologist" }
                });

            migrationBuilder.InsertData(
                table: "RegionalWage",
                columns: new[] { "Id", "Code", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "MonthlyWage" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 5000000m },
                    { 2, 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 4500000m },
                    { 3, 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, 4000000m }
                });

            migrationBuilder.InsertData(
                table: "Specialty",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "Description", "IsDeleted", "IsMedical", "ModificationDate", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cardiology focuses on diagnosing, treating, and preventing diseases of the heart and vascular system.", false, true, null, null, "Cardiology" },
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "General Internal Medicine provides comprehensive long-term care for adults.", false, true, null, null, "General Internal Medicine" },
                    { 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "OB/GYN focuses on women's reproductive health and pregnancy.", false, true, null, null, "Obstetrics and Gynecology" },
                    { 4, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Orthopedics treats musculoskeletal system disorders.", false, true, null, null, "Orthopedics" },
                    { 5, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Pediatrics focuses on child and adolescent health.", false, true, null, null, "Pediatrics" },
                    { 6, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dermatology treats skin, hair, and nail diseases.", false, true, null, null, "Dermatology" },
                    { 7, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Neurology treats nervous system disorders.", false, true, null, null, "Neurology" },
                    { 8, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ophthalmology treats eye and vision disorders.", false, true, null, null, "Ophthalmology" },
                    { 9, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "ENT treats ear, nose, and throat conditions.", false, true, null, null, "Otolaryngology (ENT)" },
                    { 10, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Urology treats urinary tract and male reproductive system.", false, true, null, null, "Urology" },
                    { 11, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nephrology focuses on kidney diseases.", false, true, null, null, "Nephrology" },
                    { 12, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Pulmonology treats lung and respiratory diseases.", false, true, null, null, "Pulmonology" },
                    { 13, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Gastroenterology treats digestive system disorders.", false, true, null, null, "Gastroenterology" },
                    { 14, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Endocrinology treats hormone disorders.", false, true, null, null, "Endocrinology" },
                    { 15, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Rheumatology treats autoimmune and joint diseases.", false, true, null, null, "Rheumatology" },
                    { 16, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hematology treats blood-related diseases.", false, true, null, null, "Hematology" },
                    { 17, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Oncology treats cancer.", false, true, null, null, "Oncology" },
                    { 18, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Radiology uses imaging for diagnosis.", false, true, null, null, "Radiology" },
                    { 19, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Psychiatry treats mental health disorders.", false, true, null, null, "Psychiatry" },
                    { 20, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Rehabilitation restores physical function.", false, true, null, null, "Rehabilitation Medicine" },
                    { 21, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Anesthesiology manages anesthesia and pain.", false, true, null, null, "Anesthesiology" },
                    { 22, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Emergency Medicine handles acute conditions.", false, true, null, null, "Emergency Medicine" },
                    { 23, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Infectious Diseases treats infections.", false, true, null, null, "Infectious Diseases" },
                    { 24, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Family Medicine provides holistic care.", false, true, null, null, "Family Medicine" },
                    { 25, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Geriatrics focuses on elderly care.", false, true, null, null, "Geriatrics" }
                });

            migrationBuilder.InsertData(
                table: "StaffCertificateType",
                columns: new[] { "Id", "Name", "ScopeOfPractice" },
                values: new object[,]
                {
                    { 1, "BLS Provider", "Basic Life Support for clinical staff" },
                    { 2, "ACLS Provider", "Advanced cardiovascular life support" },
                    { 3, "PALS Provider", "Pediatric advanced life support" }
                });

            migrationBuilder.InsertData(
                table: "StaffGroup",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "ModificationDate", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "Doctors" },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "Nurses" },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, "TeleHealth Staff" }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "AccountNumber", "BankCode", "CreatedBy", "CreationDate", "DeletionDate", "DependentQuantity", "IsDeleted", "ModificationDate", "ModifiedBy", "QualificationId", "SpecialtyId", "Status" },
                values: new object[,]
                {
                    { 2, "9704360000000002", 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, null, null, 2, 2, 0 },
                    { 4, "9704070000000004", 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 4, 4, 0 },
                    { 5, "9704180000000005", 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, null, null, 5, 5, 0 },
                    { 6, "9704150000000006", 4, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 6, 6, 0 },
                    { 7, "9704160000000007", 5, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 7, 7, 0 },
                    { 8, "9704230000000008", 6, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, null, null, 8, 8, 0 },
                    { 9, "9704320000000009", 7, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, null, null, 9, 9, 0 },
                    { 11, "9704050000000011", 8, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 11, 11, 0 },
                    { 12, "9704260000000012", 9, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, null, null, 12, 12, 0 },
                    { 13, "9704480000000013", 10, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 13, 13, 0 },
                    { 14, "9704010000000014", 0, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, null, null, 1, 1, 0 },
                    { 15, "9704020000000015", 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 2, 2, 0 },
                    { 16, "9704030000000016", 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 3, 3, 0 },
                    { 17, "9704040000000017", 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, null, null, 4, 4, 0 },
                    { 18, "9704050000000018", 4, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 5, 5, 0 },
                    { 19, "9704060000000019", 5, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 6, 6, 0 },
                    { 20, "9704070000000020", 6, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, null, null, 7, 7, 0 },
                    { 21, "9704080000000021", 7, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, false, null, null, 8, 8, 0 },
                    { 22, "9704090000000022", 8, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, false, null, null, 9, 9, 0 }
                });

            migrationBuilder.InsertData(
                table: "StaffCertificate",
                columns: new[] { "Id", "CertificateNo", "CreatedBy", "CreationDate", "DeletionDate", "ExpiredDate", "IsDeleted", "IssuedBy", "IssuedDate", "ModificationDate", "ModifiedBy", "Reason", "StaffCertificateTypeId", "StaffId", "Status" },
                values: new object[,]
                {
                    { 2, "ACLS-002", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2025, 6, 10), false, "American Heart Association", new DateOnly(2023, 6, 10), null, null, null, 2, 4, 0 },
                    { 3, "PALS-003", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2024, 9, 5), false, "American Heart Association", new DateOnly(2022, 9, 5), null, null, null, 3, 5, 1 }
                });

            migrationBuilder.InsertData(
                table: "StaffContract",
                columns: new[] { "Id", "ContractCode", "ContractType", "CreatedBy", "CreationDate", "DeletionDate", "EndDate", "HourlyRate", "InsuranceSalaryRate", "IsDeleted", "ModificationDate", "ModifiedBy", "RegionalWageId", "SignatureImage", "SignedDate", "StaffId", "StartDate", "Status", "WorkingHoursPerWeek" },
                values: new object[,]
                {
                    { 2, "HD002", 0, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2030, 12, 31), 2.2m, 1m, false, null, null, 2, null, new DateOnly(2024, 6, 1), 2, new DateOnly(2024, 6, 1), 0, 40 },
                    { 4, "HD004", 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2030, 12, 31), 3m, 1m, false, null, null, 1, null, new DateOnly(2024, 3, 1), 4, new DateOnly(2024, 3, 1), 0, 40 },
                    { 5, "HD005", 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2030, 12, 31), 1.4m, 1m, false, null, null, 3, null, new DateOnly(2024, 9, 1), 5, new DateOnly(2024, 9, 1), 0, 40 },
                    { 6, "HD006", 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 4m, 1m, false, null, null, 1, null, new DateOnly(2023, 5, 1), 6, new DateOnly(2023, 5, 1), 0, 40 },
                    { 7, "HD007", 0, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2030, 12, 31), 2m, 1m, false, null, null, 2, null, new DateOnly(2024, 10, 1), 7, new DateOnly(2024, 10, 1), 0, 40 },
                    { 8, "HD008", 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2030, 12, 31), 3.2m, 1m, false, null, null, 1, null, new DateOnly(2024, 2, 1), 8, new DateOnly(2024, 2, 1), 0, 40 },
                    { 9, "HD009", 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3.8m, 1m, false, null, null, 2, null, new DateOnly(2023, 8, 15), 9, new DateOnly(2023, 8, 15), 0, 40 },
                    { 11, "HD011", 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2030, 12, 31), 2.8m, 1m, false, null, null, 1, null, new DateOnly(2024, 4, 1), 11, new DateOnly(2024, 4, 1), 0, 40 },
                    { 12, "HD012", 0, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2030, 12, 31), 2.08m, 1m, false, null, null, 2, null, new DateOnly(2024, 11, 1), 12, new DateOnly(2024, 11, 1), 0, 40 },
                    { 13, "HD013", 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 3.6m, 1m, false, null, null, 1, null, new DateOnly(2023, 12, 1), 13, new DateOnly(2023, 12, 1), 0, 40 }
                });

            migrationBuilder.InsertData(
                table: "StaffGroupMember",
                columns: new[] { "StaffGroupId", "StaffId" },
                values: new object[,]
                {
                    { 1, 4 },
                    { 2, 5 },
                    { 3, 4 },
                    { 3, 12 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Qualification_Name",
                table: "Qualification",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RegionalWage_Code",
                table: "RegionalWage",
                column: "Code",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Code\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Specialty_Name",
                table: "Specialty",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_QualificationId",
                table: "Staff",
                column: "QualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_SpecialtyId",
                table: "Staff",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffCertificate_CertificateNo",
                table: "StaffCertificate",
                column: "CertificateNo",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"CertificateNo\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StaffCertificate_StaffCertificateTypeId",
                table: "StaffCertificate",
                column: "StaffCertificateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffCertificate_StaffId",
                table: "StaffCertificate",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffCertificateType_Name",
                table: "StaffCertificateType",
                column: "Name",
                unique: true,
                filter: "\"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StaffContract_ContractCode",
                table: "StaffContract",
                column: "ContractCode",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"ContractCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StaffContract_RegionalWageId",
                table: "StaffContract",
                column: "RegionalWageId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffContract_StaffId",
                table: "StaffContract",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffGroup_Name",
                table: "StaffGroup",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"Name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StaffGroupMember_StaffId",
                table: "StaffGroupMember",
                column: "StaffId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaffCertificate");

            migrationBuilder.DropTable(
                name: "StaffContract");

            migrationBuilder.DropTable(
                name: "StaffGroupMember");

            migrationBuilder.DropTable(
                name: "StaffCertificateType");

            migrationBuilder.DropTable(
                name: "RegionalWage");

            migrationBuilder.DropTable(
                name: "StaffGroup");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Qualification");

            migrationBuilder.DropTable(
                name: "Specialty");
        }
    }
}
