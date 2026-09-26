using MedicalHistorySvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace MedicalHistorySvc.DAL.Seeds
{
    public class PrescriptionSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prescription>().HasData(
                new Prescription
                {
                    Id = 1,
                    DoctorNote = "Take medicine after meals.",
                    MedicationDays = 5,
                    MedicalHistoryId = 1,
                },
                new Prescription
                {
                    Id = 2,
                    DoctorNote = "Drink plenty of water.",
                    MedicationDays = 7,
                    MedicalHistoryId = 2,
                }
            );

            return modelBuilder;
        }
    }
}
