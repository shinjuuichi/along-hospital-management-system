using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecruitmentSvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class A : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InterviewType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobPosting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requirement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Benefit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmploymentType = table.Column<int>(type: "int", nullable: false),
                    SalaryMin = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CloseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPosting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    ApplyDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ApplicationStatus = table.Column<int>(type: "int", nullable: false),
                    CVUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobPostingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobApplication_JobPosting_JobPostingId",
                        column: x => x.JobPostingId,
                        principalTable: "JobPosting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Interview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    JobApplicationId = table.Column<int>(type: "int", nullable: false),
                    InterviewTypeId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interview_InterviewType_InterviewTypeId",
                        column: x => x.InterviewTypeId,
                        principalTable: "InterviewType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Interview_JobApplication_JobApplicationId",
                        column: x => x.JobApplicationId,
                        principalTable: "JobApplication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "InterviewType",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "Description", "IsDeleted", "ModificationDate", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Initial screening via phone call", false, null, null, "Phone Interview" },
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Technical skills assessment with technical team", false, null, null, "Technical Interview" },
                    { 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Final interview with management", false, null, null, "Final Interview" }
                });

            migrationBuilder.InsertData(
                table: "JobPosting",
                columns: new[] { "Id", "Benefit", "CloseDate", "CreatedBy", "CreationDate", "DeletionDate", "Description", "EmploymentType", "IsDeleted", "ModificationDate", "ModifiedBy", "Requirement", "Role", "SalaryMin", "Status", "Title" },
                values: new object[,]
                {
                    { 1, "Salary 15-20 million VND, insurance, leave, bonuses, and team activities.", new DateOnly(2029, 12, 31), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Manage accounting records, invoices, taxes, and financial reports for daily operations.", 0, false, null, null, "Accounting or finance degree, 2-3 years of experience, and accounting software proficiency.", 7, 50000m, 1, "General Accountant - Xuan Phuong, Hanoi" },
                    { 2, "Salary 15-25 million VND, project bonuses, insurance, leave, and company welfare programs.", new DateOnly(2029, 6, 30), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Coordinate client projects, licensing procedures, and progress reporting with Chinese-speaking stakeholders.", 0, false, null, null, "Bachelor's degree, 3+ years of project management experience, and professional Chinese communication.", 1, 30000m, 0, "Project Manager (Chinese Communication)" },
                    { 3, "Base salary 8-10 million VND, profit bonus, yearly review, and a dynamic workplace.", new DateOnly(2029, 3, 31), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Research products and customers, optimize product pages, and coordinate creative marketing content.", 1, false, null, null, "Marketing-related degree, basic English, research skills, AI tool familiarity, and creativity.", 8, 15000m, 1, "Marketing Staff (Product Fresher)" }
                });

            migrationBuilder.InsertData(
                table: "JobApplication",
                columns: new[] { "Id", "Address", "ApplicationStatus", "ApplyDate", "CVUrl", "DateOfBirth", "Email", "Gender", "JobPostingId", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "123 Main St, District 1, HCMC", 2, new DateOnly(2025, 1, 15), "JobApplication/ba45fada8c194f78b76b3cc74e907b9d.pdf", new DateOnly(1990, 5, 15), "john.smith@email.com", 0, 1, "John Smith", "0912345678" },
                    { 2, "456 Oak Ave, District 3, HCMC", 1, new DateOnly(2025, 1, 20), "JobApplication/ba45fada8c194f78b76b3cc74e907b9d.pdf", new DateOnly(1985, 8, 22), "mquwntran04@gmail.com", 1, 1, "Mary Johnson", "0912345679" },
                    { 3, "789 Pine Rd, District 5, HCMC", 2, new DateOnly(2025, 2, 1), "JobApplication/717a3c36949d4e539b18afb15756e89f.pdf", new DateOnly(1992, 3, 10), "david.lee@email.com", 0, 2, "David Lee", "0912345680" },
                    { 4, "321 Elm St, District 7, HCMC", 1, new DateOnly(2025, 2, 5), "JobApplication/ba45fada8c194f78b76b3cc74e907b9d.pdf", new DateOnly(1988, 11, 25), "sarah.wilson@email.com", 1, 2, "Sarah Wilson", "0912345681" },
                    { 5, "654 Maple Dr, District 9, HCMC", 0, new DateOnly(2025, 2, 10), "JobApplication/ba45fada8c194f78b76b3cc74e907b9d.pdf", new DateOnly(1995, 7, 8), "mquwntran04@gmail.com", 0, 3, "Michael Brown", "0912345682" }
                });

            migrationBuilder.InsertData(
                table: "Interview",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletionDate", "InterviewDate", "InterviewTypeId", "IsDeleted", "JobApplicationId", "ModificationDate", "ModifiedBy", "Note", "Result" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 2, false, 1, null, null, "Excellent technical skills and good communication. Recommended for hire.", 1 },
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 15, 14, 0, 0, 0, DateTimeKind.Unspecified), 1, false, 2, null, null, "Scheduled for technical interview round 2.", 0 },
                    { 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), 3, false, 3, null, null, "Good experience in nursing. Passed both phone and technical interviews.", 1 },
                    { 4, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2029, 2, 20, 11, 0, 0, 0, DateTimeKind.Unspecified), 2, false, 4, null, null, "Awaiting final interview result.", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interview_InterviewTypeId",
                table: "Interview",
                column: "InterviewTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Interview_JobApplicationId",
                table: "Interview",
                column: "JobApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplication_JobPostingId",
                table: "JobApplication",
                column: "JobPostingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interview");

            migrationBuilder.DropTable(
                name: "InterviewType");

            migrationBuilder.DropTable(
                name: "JobApplication");

            migrationBuilder.DropTable(
                name: "JobPosting");
        }
    }
}
