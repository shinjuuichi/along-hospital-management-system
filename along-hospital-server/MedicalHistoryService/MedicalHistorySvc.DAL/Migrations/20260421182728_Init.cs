using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicalHistorySvc.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComplaintSummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    WeekOfYear = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintSummary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalHistoryNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FollowUpAppointmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    MedicalHistoryStatus = table.Column<int>(type: "int", nullable: false),
                    MedicalHistoryType = table.Column<int>(type: "int", nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DischargeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: true),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    PatientSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Complaint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplaintTopic = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Response = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ComplaintType = table.Column<int>(type: "int", nullable: false),
                    ComplaintResolveStatus = table.Column<int>(type: "int", nullable: false),
                    MedicalHistoryId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complaint_MedicalHistory_MedicalHistoryId",
                        column: x => x.MedicalHistoryId,
                        principalTable: "MedicalHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prescription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MedicationDays = table.Column<int>(type: "int", nullable: false),
                    MedicalHistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescription_MedicalHistory_MedicalHistoryId",
                        column: x => x.MedicalHistoryId,
                        principalTable: "MedicalHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionDetail",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    Dosage = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FrequencyPerDay = table.Column<int>(type: "int", nullable: false),
                    MedicineSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionDetail", x => new { x.PrescriptionId, x.MedicineId });
                    table.ForeignKey(
                        name: "FK_PrescriptionDetail_Prescription_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ComplaintSummary",
                columns: new[] { "Id", "Summary", "WeekOfYear", "Year" },
                values: new object[] { 1, "In the first week of 2026, we received 10 complaints: 6 negative, 3 neutral, and 1 positive.", 1, 2026 });

            migrationBuilder.InsertData(
                table: "MedicalHistory",
                columns: new[] { "Id", "AdmissionDate", "Diagnosis", "DischargeDate", "DoctorId", "FollowUpAppointmentDate", "MedicalHistoryNumber", "MedicalHistoryStatus", "MedicalHistoryType", "PatientId", "PatientSnapshot", "SpecialtyId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Common cold and mild fever.", new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateOnly(2025, 1, 16), "MH-OP-20250101-000000", 2, 0, 3, "{\"phone\":\"0360000003\",\"email\":\"patient@example.com\",\"name\":\"Patient User\",\"dateOfBirth\":\"1992-08-15\",\"gender\":\"Male\",\"medicalNumber\":\"PT-2025-000003\",\"height\":175,\"weight\":70,\"bloodType\":\"O\",\"allergies\":[{\"name\":\"Penicillin\",\"severityLevel\":\"Severe\",\"reaction\":\"Anaphylaxis\",\"id\":1},{\"name\":\"Dust\",\"severityLevel\":\"Mild\",\"reaction\":\"Sneezing, watery eyes\",\"id\":2}],\"id\":3}", 1 },
                    { 2, new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Muscle pain after exercise.", new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateOnly(2025, 1, 26), "MH-OP-20250116-000000", 2, 0, 3, "{\"phone\":\"0360000003\",\"email\":\"patient@example.com\",\"name\":\"Patient User\",\"dateOfBirth\":\"1992-08-15\",\"gender\":\"Male\",\"medicalNumber\":\"PT-2025-000003\",\"height\":178,\"weight\":69,\"bloodType\":\"O\",\"allergies\":[{\"name\":\"Seafood\",\"severityLevel\":\"Moderate\",\"reaction\":\"Rash, itching\",\"id\":1}],\"id\":3}", 2 },
                    { 3, new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pneumonia requiring inpatient care.", null, 4, new DateOnly(2025, 2, 10), "MH-IP-20250131-000000", 1, 1, 3, "{\"phone\":\"0360000003\",\"email\":\"patient@example.com\",\"name\":\"Patient User\",\"dateOfBirth\":\"1992-08-15\",\"gender\":\"Male\",\"medicalNumber\":\"PT-2025-000003\",\"height\":178,\"weight\":69,\"bloodType\":\"O\",\"allergies\":[{\"name\":\"Penicillin\",\"severityLevel\":\"Severe\",\"reaction\":\"Anaphylaxis\",\"id\":1},{\"name\":\"Dust\",\"severityLevel\":\"Mild\",\"reaction\":\"Sneezing, watery eyes\",\"id\":2}],\"id\":3}", 3 },
                    { 4, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Post-surgery recovery for appendectomy.", null, 4, new DateOnly(2025, 2, 20), "MH-IP-20250205-000000", 1, 1, 3, "{\"phone\":\"0360000003\",\"email\":\"patient@example.com\",\"name\":\"Patient User\",\"dateOfBirth\":\"1992-08-15\",\"gender\":\"Male\",\"medicalNumber\":\"PT-2025-000003\",\"height\":178,\"weight\":69,\"bloodType\":\"O\",\"allergies\":[{\"name\":\"Seafood\",\"severityLevel\":\"Moderate\",\"reaction\":\"Rash, itching\",\"id\":1}],\"id\":3}", 4 }
                });

            migrationBuilder.InsertData(
                table: "Complaint",
                columns: new[] { "Id", "ComplaintResolveStatus", "ComplaintTopic", "ComplaintType", "Content", "CreatedBy", "CreationDate", "DeletionDate", "IsDeleted", "MedicalHistoryId", "ModificationDate", "ModifiedBy", "Response" },
                values: new object[,]
                {
                    { 1, 2, 0, 2, "The waiting time was too long.", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 1, null, null, "We are sorry for the delay. We'll improve scheduling." },
                    { 2, 3, 2, 1, "Doctor was very attentive and professional.", null, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 2, null, null, "Thank you for your feedback." }
                });

            migrationBuilder.InsertData(
                table: "Prescription",
                columns: new[] { "Id", "DoctorNote", "MedicalHistoryId", "MedicationDays" },
                values: new object[,]
                {
                    { 1, "Take medicine after meals.", 1, 5 },
                    { 2, "Drink plenty of water.", 2, 7 }
                });

            migrationBuilder.InsertData(
                table: "PrescriptionDetail",
                columns: new[] { "MedicineId", "PrescriptionId", "Dosage", "FrequencyPerDay", "MedicineSnapshot" },
                values: new object[,]
                {
                    { 1, 1, 1m, 3, "{\"medicineName\":\"Paracetamol\",\"medicineBrand\":\"PharmaPlus\",\"medicineImage\":\"/images/meds/paracetamol.jpg\",\"medicineUnit\":\"Tablet\"}" },
                    { 2, 2, 1m, 1, "{\"medicineName\":\"Cetirizine\",\"medicineBrand\":\"AllerCare\",\"medicineImage\":\"/images/meds/cetirizine.jpg\",\"medicineUnit\":\"Tablet\"}" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Complaint_MedicalHistoryId",
                table: "Complaint",
                column: "MedicalHistoryId",
                unique: true,
                filter: "\"IsDeleted\" = 0 AND \"MedicalHistoryId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_MedicalHistoryNumber",
                table: "MedicalHistory",
                column: "MedicalHistoryNumber",
                unique: true,
                filter: "\"MedicalHistoryNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_MedicalHistoryId",
                table: "Prescription",
                column: "MedicalHistoryId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Complaint");

            migrationBuilder.DropTable(
                name: "ComplaintSummary");

            migrationBuilder.DropTable(
                name: "PrescriptionDetail");

            migrationBuilder.DropTable(
                name: "Prescription");

            migrationBuilder.DropTable(
                name: "MedicalHistory");
        }
    }
}
