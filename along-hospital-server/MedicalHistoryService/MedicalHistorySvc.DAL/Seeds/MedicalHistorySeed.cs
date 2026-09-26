using MedicalHistorySvc.DAL.Enums;
using MedicalHistorySvc.DAL.Models;
using MedicalHistorySvc.DAL.Models.Snapshots;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace MedicalHistorySvc.DAL.Seeds
{
    public class MedicalHistorySeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            DateTime seedDateTime = new(2025, 1, 1);

            modelBuilder.Entity<MedicalHistory>().HasData(
                new MedicalHistory
                {
                    Id = 1,
                    MedicalHistoryNumber = $"MH-OP-{seedDateTime:yyyyMMdd}-{seedDateTime.Ticks % 1000000:D6}",
                    Diagnosis = "Common cold and mild fever.",
                    FollowUpAppointmentDate = DateOnly.FromDateTime(seedDateTime.AddDays(15)),
                    MedicalHistoryStatus = MedicalHistoryStatusEnum.Completed,
                    MedicalHistoryType = MedicalHistoryTypeEnum.Outpatient,
                    AdmissionDate = seedDateTime,
                    DischargeDate = seedDateTime.AddDays(2),
                    PatientId = 3,
                    DoctorId = 4,
                    SpecialtyId = 1,
                    PatientSnapshot = new PatientSnapshot()
                    {
                        Id = 3,
                        Email = "patient@example.com",
                        Phone = "0360000003",
                        Name = "Patient User",
                        DateOfBirth = new DateOnly(1992, 8, 15),
                        Gender = "Male",
                        Image = null,
                        MedicalNumber = "PT-2025-000003",
                        Height = 175,
                        Weight = 70,
                        BloodType = "O",
                        Allergies = [
                             new AllergySnapshot
                             {
                                 Id = 1,
                                 Name = "Penicillin",
                                 SeverityLevel = "Severe",
                                 Reaction = "Anaphylaxis",
                             },
                             new AllergySnapshot
                             {
                                 Id = 2,
                                 Name = "Dust",
                                 SeverityLevel = "Mild",
                                 Reaction = "Sneezing, watery eyes",
                             },
                        ]
                    },
                },
                new MedicalHistory
                {
                    Id = 2,
                    MedicalHistoryNumber = $"MH-OP-{seedDateTime.AddDays(15):yyyyMMdd}-{(seedDateTime.AddDays(15)).Ticks % 1000000:D6}",
                    Diagnosis = "Muscle pain after exercise.",
                    FollowUpAppointmentDate = DateOnly.FromDateTime(seedDateTime.AddDays(25)),
                    MedicalHistoryStatus = MedicalHistoryStatusEnum.Completed,
                    MedicalHistoryType = MedicalHistoryTypeEnum.Outpatient,
                    AdmissionDate = seedDateTime.AddDays(15),
                    DischargeDate = seedDateTime.AddDays(17),
                    PatientId = 3,
                    DoctorId = 4,
                    SpecialtyId = 2,
                    PatientSnapshot = new PatientSnapshot()
                    {
                        Id = 3,
                        Email = "patient@example.com",
                        Phone = "0360000003",
                        Name = "Patient User",
                        DateOfBirth = new DateOnly(1992, 8, 15),
                        Gender = "Male",
                        Image = null,
                        MedicalNumber = "PT-2025-000003",
                        Height = 178,
                        Weight = 69,
                        BloodType = "O",
                        Allergies = [
                            new AllergySnapshot
                            {
                                Id = 1,
                                Name = "Seafood",
                                SeverityLevel = "Moderate",
                                Reaction = "Rash, itching",
                            }
                        ]
                    }
                },
                new MedicalHistory
                {
                    Id = 3,
                    MedicalHistoryNumber = $"MH-IP-{seedDateTime.AddDays(30):yyyyMMdd}-{(seedDateTime.AddDays(30)).Ticks % 1000000:D6}",
                    Diagnosis = "Pneumonia requiring inpatient care.",
                    FollowUpAppointmentDate = DateOnly.FromDateTime(seedDateTime.AddDays(40)),
                    MedicalHistoryStatus = MedicalHistoryStatusEnum.Draft,
                    MedicalHistoryType = MedicalHistoryTypeEnum.Inpatient,
                    AdmissionDate = seedDateTime.AddDays(30),
                    DischargeDate = null,
                    PatientId = 3,
                    DoctorId = 4,
                    SpecialtyId = 3,
                    PatientSnapshot = new PatientSnapshot()
                    {
                        Id = 3,
                        Email = "patient@example.com",
                        Phone = "0360000003",
                        Name = "Patient User",
                        DateOfBirth = new DateOnly(1992, 8, 15),
                        Gender = "Male",
                        Image = null,
                        MedicalNumber = "PT-2025-000003",
                        Height = 178,
                        Weight = 69,
                        BloodType = "O",
                        Allergies = [
                            new AllergySnapshot
                            {
                                Id = 1,
                                Name = "Penicillin",
                                SeverityLevel = "Severe",
                                Reaction = "Anaphylaxis",
                            },
                            new AllergySnapshot
                            {
                                Id = 2,
                                Name = "Dust",
                                SeverityLevel = "Mild",
                                Reaction = "Sneezing, watery eyes",
                            },
                        ]
                    },
                },
                new MedicalHistory
                {
                    Id = 4,
                    MedicalHistoryNumber = $"MH-IP-{seedDateTime.AddDays(35):yyyyMMdd}-{(seedDateTime.AddDays(35)).Ticks % 1000000:D6}",
                    Diagnosis = "Post-surgery recovery for appendectomy.",
                    FollowUpAppointmentDate = DateOnly.FromDateTime(seedDateTime.AddDays(50)),
                    MedicalHistoryStatus = MedicalHistoryStatusEnum.Draft,
                    MedicalHistoryType = MedicalHistoryTypeEnum.Inpatient,
                    AdmissionDate = seedDateTime.AddDays(35),
                    DischargeDate = null,
                    PatientId = 3,
                    DoctorId = 4,
                    SpecialtyId = 4,
                    PatientSnapshot = new PatientSnapshot()
                    {
                        Id = 3,
                        Email = "patient@example.com",
                        Phone = "0360000003",
                        Name = "Patient User",
                        DateOfBirth = new DateOnly(1992, 8, 15),
                        Gender = "Male",
                        Image = null,
                        MedicalNumber = "PT-2025-000003",
                        Height = 178,
                        Weight = 69,
                        BloodType = "O",
                        Allergies = [
                            new AllergySnapshot
                            {
                                Id = 1,
                                Name = "Seafood",
                                SeverityLevel = "Moderate",
                                Reaction = "Rash, itching",
                            }
                        ]
                    },
                }
            );

            return modelBuilder;
        }
    }
}
