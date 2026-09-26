using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using StaffSvc.DAL.Models;

namespace StaffSvc.DAL.Seeds
{
    public class QualificationSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Qualification>().HasData(
                new Qualification { Id = 1, Name = "General Practitioner", Description = "Basic licensed doctor", CreationDate = seedCreationDate },
                new Qualification { Id = 2, Name = "Specialist Level I", Description = "Specialist certification level I", CreationDate = seedCreationDate },
                new Qualification { Id = 3, Name = "Specialist Level II", Description = "Specialist certification level II", CreationDate = seedCreationDate },
                new Qualification { Id = 4, Name = "Master of Medicine", Description = "Master degree in medicine", CreationDate = seedCreationDate },
                new Qualification { Id = 5, Name = "Doctor of Philosophy", Description = "PhD in medical science", CreationDate = seedCreationDate },
                new Qualification { Id = 6, Name = "Resident Doctor", Description = "Doctor in residency training", CreationDate = seedCreationDate },
                new Qualification { Id = 7, Name = "Consultant Physician", Description = "Senior consulting doctor", CreationDate = seedCreationDate },
                new Qualification { Id = 8, Name = "Surgeon", Description = "Certified surgical doctor", CreationDate = seedCreationDate },
                new Qualification { Id = 9, Name = "Medical Lecturer", Description = "Teaching medical staff", CreationDate = seedCreationDate },
                new Qualification { Id = 10, Name = "Clinical Researcher", Description = "Medical research specialist", CreationDate = seedCreationDate },
                new Qualification { Id = 11, Name = "Public Health Specialist", Description = "Specialist in public health", CreationDate = seedCreationDate },
                new Qualification { Id = 12, Name = "Hospital Administrator", Description = "Healthcare management professional", CreationDate = seedCreationDate },
                new Qualification { Id = 13, Name = "Medical Technologist", Description = "Medical laboratory technology specialist", CreationDate = seedCreationDate }
            );

            return modelBuilder;
        }
    }
}