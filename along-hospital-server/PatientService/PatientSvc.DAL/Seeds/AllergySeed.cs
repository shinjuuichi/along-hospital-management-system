using Microsoft.EntityFrameworkCore;
using PatientSvc.DAL.Enums;
using PatientSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace PatientSvc.DAL.Seeds
{
    public class AllergySeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Allergy>().HasData(
                new Allergy
                {
                    Id = 1,
                    Name = "Penicillin",
                    SeverityLevel = SeverityLevelEnum.Severe,
                    Reaction = "Anaphylaxis",
                    PatientId = 3
                },
                new Allergy
                {
                    Id = 2,
                    Name = "Dust",
                    SeverityLevel = SeverityLevelEnum.Mild,
                    Reaction = "Sneezing, watery eyes",
                    PatientId = 3
                },
                new Allergy
                {
                    Id = 3,
                    Name = "Seafood",
                    SeverityLevel = SeverityLevelEnum.Moderate,
                    Reaction = "Rash, itching",
                    PatientId = 3
                }
            );

            return modelBuilder;
        }
    }
}
