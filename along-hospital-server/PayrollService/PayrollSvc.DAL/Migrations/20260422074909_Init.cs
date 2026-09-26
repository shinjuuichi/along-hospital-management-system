using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PayrollSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllowanceType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemGenerated = table.Column<bool>(type: "bit", nullable: false),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    InsuranceSubject = table.Column<int>(type: "int", nullable: false),
                    AllowanceQuantitySourceEnum = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowanceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeductionType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemGenerated = table.Column<bool>(type: "bit", nullable: false),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    InsuranceSubject = table.Column<int>(type: "int", nullable: false),
                    DeductionQuantitySourceEnum = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeductionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GlobalTaxConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalDeductionAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DependentDeductionAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ReferenceBaseSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SocialInsuranceRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    HealthInsuranceRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UnemploymentInsuranceRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalTaxConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    TotalWorkedMinutes = table.Column<int>(type: "int", nullable: false),
                    OvertimeMinutes = table.Column<int>(type: "int", nullable: false),
                    LateMinutes = table.Column<int>(type: "int", nullable: false),
                    EarlyLeaveMinutes = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    GlobalTaxConfigSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionalWageSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffContractSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryAdvanceSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxBracketSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxBracket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxBracket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayrollPolicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PayrollPolicyStatus = table.Column<int>(type: "int", nullable: false),
                    AllowanceTypeId = table.Column<int>(type: "int", nullable: true),
                    DeductionTypeId = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollPolicy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollPolicy_AllowanceType_AllowanceTypeId",
                        column: x => x.AllowanceTypeId,
                        principalTable: "AllowanceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollPolicy_DeductionType_DeductionTypeId",
                        column: x => x.DeductionTypeId,
                        principalTable: "DeductionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Allowance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: false),
                    PayrollId = table.Column<int>(type: "int", nullable: false),
                    PayrollPolicyId = table.Column<int>(type: "int", nullable: true),
                    AllowanceTypeId = table.Column<int>(type: "int", nullable: false),
                    AllowanceTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allowance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allowance_AllowanceType_AllowanceTypeId",
                        column: x => x.AllowanceTypeId,
                        principalTable: "AllowanceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Allowance_PayrollPolicy_PayrollPolicyId",
                        column: x => x.PayrollPolicyId,
                        principalTable: "PayrollPolicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Allowance_Payroll_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payroll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deduction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeductionTypeId = table.Column<int>(type: "int", nullable: false),
                    DeductionTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayrollId = table.Column<int>(type: "int", nullable: false),
                    PayrollPolicyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deduction_DeductionType_DeductionTypeId",
                        column: x => x.DeductionTypeId,
                        principalTable: "DeductionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deduction_PayrollPolicy_PayrollPolicyId",
                        column: x => x.PayrollPolicyId,
                        principalTable: "PayrollPolicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deduction_Payroll_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payroll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayrollPolicyStaff",
                columns: table => new
                {
                    PayrollPolicyId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollPolicyStaff", x => new { x.PayrollPolicyId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_PayrollPolicyStaff_PayrollPolicy_PayrollPolicyId",
                        column: x => x.PayrollPolicyId,
                        principalTable: "PayrollPolicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AllowanceType",
                columns: new[] { "Id", "AllowanceQuantitySourceEnum", "CreatedBy", "CreationDate", "DeletionDate", "Description", "InsuranceSubject", "IsDeleted", "IsPercentage", "IsSystemGenerated", "IsTaxable", "ModificationDate", "ModifiedBy", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, false, true, true, null, null, "Overtime", 1000000m },
                    { 2, 0, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, false, false, false, false, null, null, "Phu cap an trua", 500000m }
                });

            migrationBuilder.InsertData(
                table: "DeductionType",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeductionQuantitySourceEnum", "DeletionDate", "Description", "InsuranceSubject", "IsDeleted", "IsPercentage", "IsSystemGenerated", "IsTaxable", "ModificationDate", "ModifiedBy", "Name", "Price" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, 0, false, false, true, false, null, null, "Late Penalty", 2000m },
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null, 0, false, false, true, false, null, null, "Early Leave Penalty", 2000m },
                    { 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, null, 0, false, false, false, false, null, null, "Phi cong doan", 100000m }
                });

            migrationBuilder.InsertData(
                table: "GlobalTaxConfig",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "DependentDeductionAmount", "HealthInsuranceRate", "IsDeleted", "ModificationDate", "ModifiedBy", "PersonalDeductionAmount", "ReferenceBaseSalary", "SocialInsuranceRate", "UnemploymentInsuranceRate" },
                values: new object[] { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 176m, 0.015m, false, null, null, 440m, 94m, 0.08m, 0.01m });

            migrationBuilder.InsertData(
                table: "Payroll",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "EarlyLeaveMinutes", "GlobalTaxConfigSnapshot", "IsDeleted", "LateMinutes", "ModificationDate", "ModifiedBy", "Month", "OvertimeMinutes", "QrCode", "RegionalWageSnapshot", "SalaryAdvanceSnapshot", "StaffContractSnapshot", "StaffId", "StaffSnapshot", "Status", "TaxBracketSnapshot", "TotalWorkedMinutes", "TransactionId", "Year" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 1, 0, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":48,\"insuranceSalaryRate\":1}", 4, "{\"dependentQuantity\":0,\"bankCode\":\"MBBank\",\"accountNumber\":\"0762826608\"}", 0, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 1, null, 2025 },
                    { 2, null, new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 2, 180, null, "{\"monthlyWage\":198,\"region\":1}", "{\"id\":5,\"amount\":140,\"reason\":\"Disbursed advance linked to paid payroll\",\"createdBy\":4,\"creationDate\":\"2025-02-04T00:00:00Z\"}", "{\"hourlyRate\":55,\"insuranceSalaryRate\":1}", 4, "{\"dependentQuantity\":0,\"bankCode\":\"MBBank\",\"accountNumber\":\"0762826608\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9600, new Guid("11111111-2222-3333-4444-555555555555"), 2025 },
                    { 3, null, new DateTime(2025, 3, 31, 0, 2, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 120, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":42,\"insuranceSalaryRate\":1}", 2, "{\"dependentQuantity\":0,\"bankCode\":\"Vietcombank\",\"accountNumber\":\"9704360000000002\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9510, new Guid("00000000-0000-0000-0000-000000000003"), 2025 },
                    { 4, null, new DateTime(2025, 4, 30, 0, 2, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 4, 180, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":42,\"insuranceSalaryRate\":1}", 2, "{\"dependentQuantity\":0,\"bankCode\":\"Vietcombank\",\"accountNumber\":\"9704360000000002\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9450, null, 2025 },
                    { 5, null, new DateTime(2025, 3, 31, 0, 4, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 0, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":55,\"insuranceSalaryRate\":1}", 4, "{\"dependentQuantity\":1,\"bankCode\":\"Techcombank\",\"accountNumber\":\"9704070000000004\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9420, new Guid("00000000-0000-0000-0000-000000000005"), 2025 },
                    { 6, null, new DateTime(2025, 4, 30, 0, 4, 0, 0, DateTimeKind.Utc), null, 10, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 60, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":55,\"insuranceSalaryRate\":1}", 4, "{\"dependentQuantity\":1,\"bankCode\":\"Techcombank\",\"accountNumber\":\"9704070000000004\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9360, null, 2025 },
                    { 7, null, new DateTime(2025, 3, 31, 0, 5, 0, 0, DateTimeKind.Utc), null, 10, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 60, null, "{\"monthlyWage\":154,\"region\":3}", null, "{\"hourlyRate\":18,\"insuranceSalaryRate\":1}", 5, "{\"dependentQuantity\":0,\"bankCode\":\"BIDV\",\"accountNumber\":\"9704180000000005\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9375, new Guid("00000000-0000-0000-0000-000000000007"), 2025 },
                    { 8, null, new DateTime(2025, 4, 30, 0, 5, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 4, 120, null, "{\"monthlyWage\":154,\"region\":3}", null, "{\"hourlyRate\":18,\"insuranceSalaryRate\":1}", 5, "{\"dependentQuantity\":0,\"bankCode\":\"BIDV\",\"accountNumber\":\"9704180000000005\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9315, null, 2025 },
                    { 9, null, new DateTime(2025, 3, 31, 0, 6, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 3, 120, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":30,\"insuranceSalaryRate\":1}", 6, "{\"dependentQuantity\":2,\"bankCode\":\"VietinBank\",\"accountNumber\":\"9704150000000006\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9330, new Guid("00000000-0000-0000-0000-000000000009"), 2025 },
                    { 10, null, new DateTime(2025, 4, 30, 0, 6, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 180, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":30,\"insuranceSalaryRate\":1}", 6, "{\"dependentQuantity\":2,\"bankCode\":\"VietinBank\",\"accountNumber\":\"9704150000000006\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9270, null, 2025 },
                    { 11, null, new DateTime(2025, 3, 31, 0, 7, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 180, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":24,\"insuranceSalaryRate\":1}", 7, "{\"dependentQuantity\":1,\"bankCode\":\"ACB\",\"accountNumber\":\"9704160000000007\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9600, new Guid("00000000-0000-0000-0000-000000000011"), 2025 },
                    { 12, null, new DateTime(2025, 4, 30, 0, 7, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 0, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":24,\"insuranceSalaryRate\":1}", 7, "{\"dependentQuantity\":1,\"bankCode\":\"ACB\",\"accountNumber\":\"9704160000000007\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9540, null, 2025 },
                    { 13, null, new DateTime(2025, 3, 31, 0, 8, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 0, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":27,\"insuranceSalaryRate\":1}", 8, "{\"dependentQuantity\":0,\"bankCode\":\"TPBank\",\"accountNumber\":\"9704230000000008\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9555, new Guid("00000000-0000-0000-0000-000000000013"), 2025 },
                    { 14, null, new DateTime(2025, 4, 30, 0, 8, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 4, 60, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":27,\"insuranceSalaryRate\":1}", 8, "{\"dependentQuantity\":0,\"bankCode\":\"TPBank\",\"accountNumber\":\"9704230000000008\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9495, null, 2025 },
                    { 15, null, new DateTime(2025, 3, 31, 0, 9, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 3, 60, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":22,\"insuranceSalaryRate\":1}", 9, "{\"dependentQuantity\":0,\"bankCode\":\"VPBank\",\"accountNumber\":\"9704320000000009\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9510, new Guid("00000000-0000-0000-0000-000000000015"), 2025 },
                    { 16, null, new DateTime(2025, 4, 30, 0, 9, 0, 0, DateTimeKind.Utc), null, 10, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 120, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":22,\"insuranceSalaryRate\":1}", 9, "{\"dependentQuantity\":0,\"bankCode\":\"VPBank\",\"accountNumber\":\"9704320000000009\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9450, null, 2025 },
                    { 17, null, new DateTime(2025, 3, 31, 0, 11, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 180, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":16,\"insuranceSalaryRate\":1}", 11, "{\"dependentQuantity\":1,\"bankCode\":\"Agribank\",\"accountNumber\":\"9704050000000011\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9420, new Guid("00000000-0000-0000-0000-000000000017"), 2025 },
                    { 18, null, new DateTime(2025, 4, 30, 0, 11, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 4, 0, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":16,\"insuranceSalaryRate\":1}", 11, "{\"dependentQuantity\":1,\"bankCode\":\"Agribank\",\"accountNumber\":\"9704050000000011\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9360, null, 2025 },
                    { 19, null, new DateTime(2025, 3, 31, 0, 12, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 3, 0, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":17,\"insuranceSalaryRate\":1}", 12, "{\"dependentQuantity\":0,\"bankCode\":\"MSB\",\"accountNumber\":\"9704260000000012\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9375, new Guid("00000000-0000-0000-0000-000000000019"), 2025 },
                    { 20, null, new DateTime(2025, 4, 30, 0, 12, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 60, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":17,\"insuranceSalaryRate\":1}", 12, "{\"dependentQuantity\":0,\"bankCode\":\"MSB\",\"accountNumber\":\"9704260000000012\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9315, null, 2025 },
                    { 21, null, new DateTime(2025, 3, 31, 0, 13, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 60, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":19,\"insuranceSalaryRate\":1}", 13, "{\"dependentQuantity\":2,\"bankCode\":\"OCB\",\"accountNumber\":\"9704480000000013\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9330, new Guid("00000000-0000-0000-0000-000000000021"), 2025 },
                    { 22, null, new DateTime(2025, 4, 30, 0, 13, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 120, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":19,\"insuranceSalaryRate\":1}", 13, "{\"dependentQuantity\":2,\"bankCode\":\"OCB\",\"accountNumber\":\"9704480000000013\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9270, null, 2025 },
                    { 23, null, new DateTime(2025, 3, 31, 0, 14, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 120, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":44,\"insuranceSalaryRate\":1}", 14, "{\"dependentQuantity\":0,\"bankCode\":\"MBBank\",\"accountNumber\":\"9704010000000014\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9600, new Guid("00000000-0000-0000-0000-000000000023"), 2025 },
                    { 24, null, new DateTime(2025, 4, 30, 0, 14, 0, 0, DateTimeKind.Utc), null, 10, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 4, 180, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":44,\"insuranceSalaryRate\":1}", 14, "{\"dependentQuantity\":0,\"bankCode\":\"MBBank\",\"accountNumber\":\"9704010000000014\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9540, null, 2025 },
                    { 25, null, new DateTime(2025, 3, 31, 0, 15, 0, 0, DateTimeKind.Utc), null, 10, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 3, 180, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":47,\"insuranceSalaryRate\":1}", 15, "{\"dependentQuantity\":1,\"bankCode\":\"Vietcombank\",\"accountNumber\":\"9704020000000015\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9555, new Guid("00000000-0000-0000-0000-000000000025"), 2025 },
                    { 26, null, new DateTime(2025, 4, 30, 0, 15, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 0, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":47,\"insuranceSalaryRate\":1}", 15, "{\"dependentQuantity\":1,\"bankCode\":\"Vietcombank\",\"accountNumber\":\"9704020000000015\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9495, null, 2025 },
                    { 27, null, new DateTime(2025, 3, 31, 0, 16, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 0, null, "{\"monthlyWage\":154,\"region\":3}", null, "{\"hourlyRate\":50,\"insuranceSalaryRate\":1}", 16, "{\"dependentQuantity\":2,\"bankCode\":\"Techcombank\",\"accountNumber\":\"9704030000000016\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9510, new Guid("00000000-0000-0000-0000-000000000027"), 2025 },
                    { 28, null, new DateTime(2025, 4, 30, 0, 16, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 60, null, "{\"monthlyWage\":154,\"region\":3}", null, "{\"hourlyRate\":50,\"insuranceSalaryRate\":1}", 16, "{\"dependentQuantity\":2,\"bankCode\":\"Techcombank\",\"accountNumber\":\"9704030000000016\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9450, null, 2025 },
                    { 29, null, new DateTime(2025, 3, 31, 0, 17, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 60, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":53,\"insuranceSalaryRate\":1}", 17, "{\"dependentQuantity\":0,\"bankCode\":\"BIDV\",\"accountNumber\":\"9704040000000017\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9465, new Guid("00000000-0000-0000-0000-000000000029"), 2025 },
                    { 30, null, new DateTime(2025, 4, 30, 0, 17, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 4, 120, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":53,\"insuranceSalaryRate\":1}", 17, "{\"dependentQuantity\":0,\"bankCode\":\"BIDV\",\"accountNumber\":\"9704040000000017\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9405, null, 2025 },
                    { 31, null, new DateTime(2025, 3, 31, 0, 18, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 3, 120, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":56,\"insuranceSalaryRate\":1}", 18, "{\"dependentQuantity\":1,\"bankCode\":\"VietinBank\",\"accountNumber\":\"9704050000000018\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9420, new Guid("00000000-0000-0000-0000-000000000031"), 2025 },
                    { 32, null, new DateTime(2025, 4, 30, 0, 18, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 180, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":56,\"insuranceSalaryRate\":1}", 18, "{\"dependentQuantity\":1,\"bankCode\":\"VietinBank\",\"accountNumber\":\"9704050000000018\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9360, null, 2025 },
                    { 33, null, new DateTime(2025, 3, 31, 0, 19, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 180, null, "{\"monthlyWage\":154,\"region\":3}", null, "{\"hourlyRate\":44,\"insuranceSalaryRate\":1}", 19, "{\"dependentQuantity\":2,\"bankCode\":\"ACB\",\"accountNumber\":\"9704060000000019\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9375, new Guid("00000000-0000-0000-0000-000000000033"), 2025 },
                    { 34, null, new DateTime(2025, 4, 30, 0, 19, 0, 0, DateTimeKind.Utc), null, 10, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 0, null, "{\"monthlyWage\":154,\"region\":3}", null, "{\"hourlyRate\":44,\"insuranceSalaryRate\":1}", 19, "{\"dependentQuantity\":2,\"bankCode\":\"ACB\",\"accountNumber\":\"9704060000000019\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9315, null, 2025 },
                    { 35, null, new DateTime(2025, 3, 31, 0, 20, 0, 0, DateTimeKind.Utc), null, 10, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 0, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":47,\"insuranceSalaryRate\":1}", 20, "{\"dependentQuantity\":0,\"bankCode\":\"TPBank\",\"accountNumber\":\"9704070000000020\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9330, new Guid("00000000-0000-0000-0000-000000000035"), 2025 },
                    { 36, null, new DateTime(2025, 4, 30, 0, 20, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 4, 60, null, "{\"monthlyWage\":198,\"region\":1}", null, "{\"hourlyRate\":47,\"insuranceSalaryRate\":1}", 20, "{\"dependentQuantity\":0,\"bankCode\":\"TPBank\",\"accountNumber\":\"9704070000000020\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9270, null, 2025 },
                    { 37, null, new DateTime(2025, 3, 31, 0, 21, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 15, null, null, 3, 60, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":50,\"insuranceSalaryRate\":1}", 21, "{\"dependentQuantity\":1,\"bankCode\":\"VPBank\",\"accountNumber\":\"9704080000000021\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9600, new Guid("00000000-0000-0000-0000-000000000037"), 2025 },
                    { 38, null, new DateTime(2025, 4, 30, 0, 21, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 120, null, "{\"monthlyWage\":176,\"region\":2}", null, "{\"hourlyRate\":50,\"insuranceSalaryRate\":1}", 21, "{\"dependentQuantity\":1,\"bankCode\":\"VPBank\",\"accountNumber\":\"9704080000000021\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9540, null, 2025 },
                    { 39, null, new DateTime(2025, 3, 31, 0, 22, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 3, 120, null, "{\"monthlyWage\":154,\"region\":3}", null, "{\"hourlyRate\":53,\"insuranceSalaryRate\":1}", 22, "{\"dependentQuantity\":2,\"bankCode\":\"Agribank\",\"accountNumber\":\"9704090000000022\"}", 3, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9555, new Guid("00000000-0000-0000-0000-000000000039"), 2025 },
                    { 40, null, new DateTime(2025, 4, 30, 0, 22, 0, 0, DateTimeKind.Utc), null, 0, "{\"referenceBaseSalary\":94,\"personalDeductionAmount\":440,\"dependentDeductionAmount\":176,\"socialInsuranceRate\":0.08,\"healthInsuranceRate\":0.015,\"unemploymentInsuranceRate\":0.01}", false, 0, null, null, 4, 180, null, "{\"monthlyWage\":154,\"region\":3}", null, "{\"hourlyRate\":53,\"insuranceSalaryRate\":1}", 22, "{\"dependentQuantity\":2,\"bankCode\":\"Agribank\",\"accountNumber\":\"9704090000000022\"}", 2, "[{\"fromAmount\":0,\"taxRate\":0.05},{\"fromAmount\":200,\"taxRate\":0.1},{\"fromAmount\":400,\"taxRate\":0.15},{\"fromAmount\":720,\"taxRate\":0.2},{\"fromAmount\":1280,\"taxRate\":0.25},{\"fromAmount\":2080,\"taxRate\":0.3},{\"fromAmount\":3200,\"taxRate\":0.35}]", 9495, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "TaxBracket",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "FromAmount", "IsDeleted", "ModificationDate", "ModifiedBy", "TaxRate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, false, null, null, 0.05m },
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 200m, false, null, null, 0.1m },
                    { 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 400m, false, null, null, 0.15m },
                    { 4, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 720m, false, null, null, 0.2m },
                    { 5, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1280m, false, null, null, 0.25m },
                    { 6, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2080m, false, null, null, 0.3m },
                    { 7, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 3200m, false, null, null, 0.35m }
                });

            migrationBuilder.InsertData(
                table: "Allowance",
                columns: new[] { "Id", "AllowanceTypeId", "AllowanceTypeName", "IsTaxable", "Note", "PayrollId", "PayrollPolicyId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 2, 2, "Phu cap an trua", false, "Meal allowance", 3, null, 1, 14m },
                    { 3, 1, "Overtime", true, "Overtime support", 3, null, 1, 7m },
                    { 4, 2, "Phu cap an trua", false, "Meal allowance", 4, null, 1, 14m },
                    { 5, 2, "Phu cap an trua", false, "Meal allowance", 5, null, 1, 14m },
                    { 6, 2, "Phu cap an trua", false, "Meal allowance", 6, null, 1, 14m },
                    { 7, 1, "Overtime", true, "Overtime support", 6, null, 1, 7m },
                    { 8, 2, "Phu cap an trua", false, "Meal allowance", 7, null, 1, 14m },
                    { 9, 2, "Phu cap an trua", false, "Meal allowance", 8, null, 1, 14m },
                    { 10, 2, "Phu cap an trua", false, "Meal allowance", 9, null, 1, 14m },
                    { 11, 1, "Overtime", true, "Overtime support", 9, null, 1, 7m },
                    { 12, 2, "Phu cap an trua", false, "Meal allowance", 10, null, 1, 14m },
                    { 13, 2, "Phu cap an trua", false, "Meal allowance", 11, null, 1, 14m },
                    { 14, 2, "Phu cap an trua", false, "Meal allowance", 12, null, 1, 14m },
                    { 15, 1, "Overtime", true, "Overtime support", 12, null, 1, 7m },
                    { 16, 2, "Phu cap an trua", false, "Meal allowance", 13, null, 1, 14m },
                    { 17, 2, "Phu cap an trua", false, "Meal allowance", 14, null, 1, 14m },
                    { 18, 2, "Phu cap an trua", false, "Meal allowance", 15, null, 1, 14m },
                    { 19, 1, "Overtime", true, "Overtime support", 15, null, 1, 7m },
                    { 20, 2, "Phu cap an trua", false, "Meal allowance", 16, null, 1, 14m },
                    { 21, 2, "Phu cap an trua", false, "Meal allowance", 17, null, 1, 14m },
                    { 22, 2, "Phu cap an trua", false, "Meal allowance", 18, null, 1, 14m },
                    { 23, 1, "Overtime", true, "Overtime support", 18, null, 1, 7m },
                    { 24, 2, "Phu cap an trua", false, "Meal allowance", 19, null, 1, 14m },
                    { 25, 2, "Phu cap an trua", false, "Meal allowance", 20, null, 1, 14m },
                    { 26, 2, "Phu cap an trua", false, "Meal allowance", 21, null, 1, 14m },
                    { 27, 1, "Overtime", true, "Overtime support", 21, null, 1, 7m },
                    { 28, 2, "Phu cap an trua", false, "Meal allowance", 22, null, 1, 14m },
                    { 29, 2, "Phu cap an trua", false, "Meal allowance", 23, null, 1, 14m },
                    { 30, 2, "Phu cap an trua", false, "Meal allowance", 24, null, 1, 14m },
                    { 31, 1, "Overtime", true, "Overtime support", 24, null, 1, 7m },
                    { 32, 2, "Phu cap an trua", false, "Meal allowance", 25, null, 1, 14m },
                    { 33, 2, "Phu cap an trua", false, "Meal allowance", 26, null, 1, 14m },
                    { 34, 2, "Phu cap an trua", false, "Meal allowance", 27, null, 1, 14m },
                    { 35, 1, "Overtime", true, "Overtime support", 27, null, 1, 7m },
                    { 36, 2, "Phu cap an trua", false, "Meal allowance", 28, null, 1, 14m },
                    { 37, 2, "Phu cap an trua", false, "Meal allowance", 29, null, 1, 14m },
                    { 38, 2, "Phu cap an trua", false, "Meal allowance", 30, null, 1, 14m },
                    { 39, 1, "Overtime", true, "Overtime support", 30, null, 1, 7m },
                    { 40, 2, "Phu cap an trua", false, "Meal allowance", 31, null, 1, 14m },
                    { 41, 2, "Phu cap an trua", false, "Meal allowance", 32, null, 1, 14m },
                    { 42, 2, "Phu cap an trua", false, "Meal allowance", 33, null, 1, 14m },
                    { 43, 1, "Overtime", true, "Overtime support", 33, null, 1, 7m },
                    { 44, 2, "Phu cap an trua", false, "Meal allowance", 34, null, 1, 14m },
                    { 45, 2, "Phu cap an trua", false, "Meal allowance", 35, null, 1, 14m },
                    { 46, 2, "Phu cap an trua", false, "Meal allowance", 36, null, 1, 14m },
                    { 47, 1, "Overtime", true, "Overtime support", 36, null, 1, 7m },
                    { 48, 2, "Phu cap an trua", false, "Meal allowance", 37, null, 1, 14m },
                    { 49, 2, "Phu cap an trua", false, "Meal allowance", 38, null, 1, 14m },
                    { 50, 2, "Phu cap an trua", false, "Meal allowance", 39, null, 1, 14m },
                    { 51, 1, "Overtime", true, "Overtime support", 39, null, 1, 7m },
                    { 52, 2, "Phu cap an trua", false, "Meal allowance", 40, null, 1, 14m }
                });

            migrationBuilder.InsertData(
                table: "Deduction",
                columns: new[] { "Id", "DeductionTypeId", "DeductionTypeName", "IsTaxable", "Note", "PayrollId", "PayrollPolicyId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 4, 1, "Late Penalty", false, "Late 5 mins", 4, null, 1, 1m },
                    { 9, 1, "Late Penalty", false, "Late 5 mins", 8, null, 1, 1m },
                    { 14, 1, "Late Penalty", false, "Late 5 mins", 12, null, 1, 1m },
                    { 19, 1, "Late Penalty", false, "Late 5 mins", 16, null, 1, 1m },
                    { 24, 1, "Late Penalty", false, "Late 5 mins", 20, null, 1, 1m },
                    { 29, 1, "Late Penalty", false, "Late 5 mins", 24, null, 1, 1m },
                    { 34, 1, "Late Penalty", false, "Late 5 mins", 28, null, 1, 1m },
                    { 39, 1, "Late Penalty", false, "Late 5 mins", 32, null, 1, 1m },
                    { 44, 1, "Late Penalty", false, "Late 5 mins", 36, null, 1, 1m },
                    { 49, 1, "Late Penalty", false, "Late 5 mins", 40, null, 1, 1m }
                });

            migrationBuilder.InsertData(
                table: "PayrollPolicy",
                columns: new[] { "Id", "AllowanceTypeId", "CreatedBy", "CreationDate", "DeductionTypeId", "DeletionDate", "EndDate", "IsDeleted", "ModificationDate", "ModifiedBy", "Name", "PayrollPolicyStatus", "StartDate" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, new DateOnly(2025, 12, 31), false, null, null, "Standard Policy 2025", 0, new DateOnly(2025, 1, 1) },
                    { 2, 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateOnly(2026, 12, 31), false, null, null, "Policy An Trua 2026", 0, new DateOnly(2026, 1, 1) }
                });

            migrationBuilder.InsertData(
                table: "Allowance",
                columns: new[] { "Id", "AllowanceTypeId", "AllowanceTypeName", "IsTaxable", "Note", "PayrollId", "PayrollPolicyId", "Quantity", "UnitPrice" },
                values: new object[] { 1, 1, "Overtime", true, null, 1, 2, 1, 5m });

            migrationBuilder.InsertData(
                table: "Deduction",
                columns: new[] { "Id", "DeductionTypeId", "DeductionTypeName", "IsTaxable", "Note", "PayrollId", "PayrollPolicyId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, "Late Penalty", false, "Late 10 mins", 1, 1, 2, 1m },
                    { 2, 3, "Phi cong doan", false, "Monthly union fee", 3, 1, 1, 2m },
                    { 3, 3, "Phi cong doan", false, "Monthly union fee", 4, 1, 1, 2m },
                    { 5, 3, "Phi cong doan", false, "Monthly union fee", 5, 1, 1, 2m },
                    { 6, 3, "Phi cong doan", false, "Monthly union fee", 6, 1, 1, 2m },
                    { 7, 3, "Phi cong doan", false, "Monthly union fee", 7, 1, 1, 2m },
                    { 8, 3, "Phi cong doan", false, "Monthly union fee", 8, 1, 1, 2m },
                    { 10, 3, "Phi cong doan", false, "Monthly union fee", 9, 1, 1, 2m },
                    { 11, 3, "Phi cong doan", false, "Monthly union fee", 10, 1, 1, 2m },
                    { 12, 3, "Phi cong doan", false, "Monthly union fee", 11, 1, 1, 2m },
                    { 13, 3, "Phi cong doan", false, "Monthly union fee", 12, 1, 1, 2m },
                    { 15, 3, "Phi cong doan", false, "Monthly union fee", 13, 1, 1, 2m },
                    { 16, 3, "Phi cong doan", false, "Monthly union fee", 14, 1, 1, 2m },
                    { 17, 3, "Phi cong doan", false, "Monthly union fee", 15, 1, 1, 2m },
                    { 18, 3, "Phi cong doan", false, "Monthly union fee", 16, 1, 1, 2m },
                    { 20, 3, "Phi cong doan", false, "Monthly union fee", 17, 1, 1, 2m },
                    { 21, 3, "Phi cong doan", false, "Monthly union fee", 18, 1, 1, 2m },
                    { 22, 3, "Phi cong doan", false, "Monthly union fee", 19, 1, 1, 2m },
                    { 23, 3, "Phi cong doan", false, "Monthly union fee", 20, 1, 1, 2m },
                    { 25, 3, "Phi cong doan", false, "Monthly union fee", 21, 1, 1, 2m },
                    { 26, 3, "Phi cong doan", false, "Monthly union fee", 22, 1, 1, 2m },
                    { 27, 3, "Phi cong doan", false, "Monthly union fee", 23, 1, 1, 2m },
                    { 28, 3, "Phi cong doan", false, "Monthly union fee", 24, 1, 1, 2m },
                    { 30, 3, "Phi cong doan", false, "Monthly union fee", 25, 1, 1, 2m },
                    { 31, 3, "Phi cong doan", false, "Monthly union fee", 26, 1, 1, 2m },
                    { 32, 3, "Phi cong doan", false, "Monthly union fee", 27, 1, 1, 2m },
                    { 33, 3, "Phi cong doan", false, "Monthly union fee", 28, 1, 1, 2m },
                    { 35, 3, "Phi cong doan", false, "Monthly union fee", 29, 1, 1, 2m },
                    { 36, 3, "Phi cong doan", false, "Monthly union fee", 30, 1, 1, 2m },
                    { 37, 3, "Phi cong doan", false, "Monthly union fee", 31, 1, 1, 2m },
                    { 38, 3, "Phi cong doan", false, "Monthly union fee", 32, 1, 1, 2m },
                    { 40, 3, "Phi cong doan", false, "Monthly union fee", 33, 1, 1, 2m },
                    { 41, 3, "Phi cong doan", false, "Monthly union fee", 34, 1, 1, 2m },
                    { 42, 3, "Phi cong doan", false, "Monthly union fee", 35, 1, 1, 2m },
                    { 43, 3, "Phi cong doan", false, "Monthly union fee", 36, 1, 1, 2m },
                    { 45, 3, "Phi cong doan", false, "Monthly union fee", 37, 1, 1, 2m },
                    { 46, 3, "Phi cong doan", false, "Monthly union fee", 38, 1, 1, 2m },
                    { 47, 3, "Phi cong doan", false, "Monthly union fee", 39, 1, 1, 2m },
                    { 48, 3, "Phi cong doan", false, "Monthly union fee", 40, 1, 1, 2m }
                });

            migrationBuilder.InsertData(
                table: "PayrollPolicyStaff",
                columns: new[] { "PayrollPolicyId", "StaffId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 1, 7 },
                    { 1, 8 },
                    { 1, 9 },
                    { 1, 11 },
                    { 1, 12 },
                    { 1, 13 },
                    { 1, 14 },
                    { 1, 15 },
                    { 1, 16 },
                    { 1, 17 },
                    { 1, 18 },
                    { 1, 19 },
                    { 1, 20 },
                    { 1, 21 },
                    { 1, 22 },
                    { 2, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allowance_AllowanceTypeId",
                table: "Allowance",
                column: "AllowanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Allowance_PayrollId",
                table: "Allowance",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Allowance_PayrollPolicyId",
                table: "Allowance",
                column: "PayrollPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_DeductionTypeId",
                table: "Deduction",
                column: "DeductionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_PayrollId",
                table: "Deduction",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_PayrollPolicyId",
                table: "Deduction",
                column: "PayrollPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollPolicy_AllowanceTypeId",
                table: "PayrollPolicy",
                column: "AllowanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollPolicy_DeductionTypeId",
                table: "PayrollPolicy",
                column: "DeductionTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allowance");

            migrationBuilder.DropTable(
                name: "Deduction");

            migrationBuilder.DropTable(
                name: "GlobalTaxConfig");

            migrationBuilder.DropTable(
                name: "PayrollPolicyStaff");

            migrationBuilder.DropTable(
                name: "TaxBracket");

            migrationBuilder.DropTable(
                name: "Payroll");

            migrationBuilder.DropTable(
                name: "PayrollPolicy");

            migrationBuilder.DropTable(
                name: "AllowanceType");

            migrationBuilder.DropTable(
                name: "DeductionType");
        }
    }
}
