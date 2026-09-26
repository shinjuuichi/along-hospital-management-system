using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using StaffSvc.DAL.Models;

namespace StaffSvc.DAL.Seeds
{
    public class SpecialtySeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Specialty>().HasData(
                new Specialty { Id = 1, Name = "Cardiology", Description = "Cardiology focuses on diagnosing, treating, and preventing diseases of the heart and vascular system.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 2, Name = "General Internal Medicine", Description = "General Internal Medicine provides comprehensive long-term care for adults.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 3, Name = "Obstetrics and Gynecology", Description = "OB/GYN focuses on women's reproductive health and pregnancy.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 4, Name = "Orthopedics", Description = "Orthopedics treats musculoskeletal system disorders.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 5, Name = "Pediatrics", Description = "Pediatrics focuses on child and adolescent health.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 6, Name = "Dermatology", Description = "Dermatology treats skin, hair, and nail diseases.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 7, Name = "Neurology", Description = "Neurology treats nervous system disorders.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 8, Name = "Ophthalmology", Description = "Ophthalmology treats eye and vision disorders.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 9, Name = "Otolaryngology (ENT)", Description = "ENT treats ear, nose, and throat conditions.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 10, Name = "Urology", Description = "Urology treats urinary tract and male reproductive system.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 11, Name = "Nephrology", Description = "Nephrology focuses on kidney diseases.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 12, Name = "Pulmonology", Description = "Pulmonology treats lung and respiratory diseases.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 13, Name = "Gastroenterology", Description = "Gastroenterology treats digestive system disorders.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 14, Name = "Endocrinology", Description = "Endocrinology treats hormone disorders.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 15, Name = "Rheumatology", Description = "Rheumatology treats autoimmune and joint diseases.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 16, Name = "Hematology", Description = "Hematology treats blood-related diseases.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 17, Name = "Oncology", Description = "Oncology treats cancer.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 18, Name = "Radiology", Description = "Radiology uses imaging for diagnosis.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 19, Name = "Psychiatry", Description = "Psychiatry treats mental health disorders.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 20, Name = "Rehabilitation Medicine", Description = "Rehabilitation restores physical function.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 21, Name = "Anesthesiology", Description = "Anesthesiology manages anesthesia and pain.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 22, Name = "Emergency Medicine", Description = "Emergency Medicine handles acute conditions.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 23, Name = "Infectious Diseases", Description = "Infectious Diseases treats infections.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 24, Name = "Family Medicine", Description = "Family Medicine provides holistic care.", CreationDate = seedCreationDate, IsMedical = true },
                new Specialty { Id = 25, Name = "Geriatrics", Description = "Geriatrics focuses on elderly care.", CreationDate = seedCreationDate, IsMedical = true }
            );

            return modelBuilder;
        }
    }
}