using Microsoft.EntityFrameworkCore;
using PatientSvc.DAL.Enums;
using PatientSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace PatientSvc.DAL.Seeds
{
    public class PatientSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            List<Patient> patients =
            [
                new Patient
                {
                    Id = 3,
                    MedicalNumber = "PT-2025-000003",
                    Height = 175,
                    Weight = 70,
                    BloodType = BloodTypeEnum.O
                }
            ];

            for (var patientIndex = 0; patientIndex < 25; patientIndex++)
            {
                var patientId = 39 + patientIndex;

                patients.Add(new Patient
                {
                    Id = patientId,
                    MedicalNumber = $"PT-2025-{patientId:000000}",
                    Height = 155 + (patientIndex % 20),
                    Weight = 48 + patientIndex,
                    BloodType = (BloodTypeEnum)((patientIndex % 5) + 1)
                });
            }

            modelBuilder.Entity<Patient>().HasData(patients.ToArray());

            return modelBuilder;
        }
    }
}
