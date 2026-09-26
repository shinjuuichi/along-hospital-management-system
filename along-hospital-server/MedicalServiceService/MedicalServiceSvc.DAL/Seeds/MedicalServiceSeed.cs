using MedicalServiceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SharedLibrary.Commons.Settings;

namespace MedicalServiceSvc.DAL.Seeds
{
    public class MedicalServiceSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicalService>().HasData(
                new MedicalService
                {
                    Id = 1,
                    Name = "General Health Check",
                    Description = "Comprehensive periodic health check-up",
                    Price = 0.1,
                    IsActive = true,
                    Code = MedicalServiceCodeConstants.GENERAL_HEALTH_CHECK_CODE,
                    SpecialtyId = 1,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicalService
                {
                    Id = 2,
                    Name = "Cardiology Consultation",
                    Description = "Heart and vascular health consultation",
                    Price = 0.1,
                    IsActive = true,
                    Code = MedicalServiceCodeConstants.CARDIOLOGY_CONSULTATION_CODE,
                    SpecialtyId = 2,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicalService
                {
                    Id = 3,
                    Name = "Dermatology Examination",
                    Description = "Skin care and dermatological check",
                    Price = 0.1,
                    IsActive = true,
                    Code = MedicalServiceCodeConstants.DERMATOLOGY_EXAMINATION_CODE,
                    SpecialtyId = 3,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicalService
                {
                    Id = 4,
                    Name = "Infusion Service",
                    Description = "Intravenous infusion therapy service",
                    Price = 0.15,
                    IsActive = true,
                    Code = MedicalServiceCodeConstants.INFUSION_SERVICE_CODE,
                    SpecialtyId = 1,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicalService
                {
                    Id = 5,
                    Name = "Telehealth Appointment",
                    Description = "Online consultation service for telehealth appointments",
                    Price = 0.1,
                    IsActive = true,
                    Code = MedicalServiceCodeConstants.TELEHEALTH_APPOINTMENT_CODE,
                    SpecialtyId = 2,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicalService
                {
                    Id = 6,
                    Name = "Standard Bed Daily Charge",
                    Description = "Daily inpatient charge for a standard hospital bed",
                    Price = 10,
                    IsActive = true,
                    Code = MedicalServiceCodeConstants.STANDARD_BED_CHARGE_CODE,
                    SpecialtyId = 1,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicalService
                {
                    Id = 7,
                    Name = "Electric Bed Daily Charge",
                    Description = "Daily inpatient charge for an electric hospital bed",
                    Price = 5,
                    IsActive = true,
                    Code = MedicalServiceCodeConstants.ELECTRIC_BED_CHARGE_CODE,
                    SpecialtyId = 1,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicalService
                {
                    Id = 8,
                    Name = "ICU Bed Daily Charge",
                    Description = "Daily inpatient charge for an ICU hospital bed",
                    Price = 20,
                    IsActive = true,
                    Code = MedicalServiceCodeConstants.ICU_BED_CHARGE_CODE,
                    SpecialtyId = 1,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                }
            );

            return modelBuilder;
        }
    }
}
//trigger cd