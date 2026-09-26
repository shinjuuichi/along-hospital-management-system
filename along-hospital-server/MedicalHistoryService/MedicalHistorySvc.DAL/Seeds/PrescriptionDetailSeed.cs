using MedicalHistorySvc.DAL.Models;
using MedicalHistorySvc.DAL.Models.Snapshots;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace MedicalHistorySvc.DAL.Seeds
{
    public class PrescriptionDetailSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrescriptionDetail>().HasData(
                new PrescriptionDetail
                {
                    PrescriptionId = 1,
                    MedicineId = 1,
                    Dosage = 1,
                    FrequencyPerDay = 3,
                    MedicineSnapshot = new PrescriptionDetailMedicineSnapshot
                    {
                        MedicineName = "Paracetamol",
                        MedicineBrand = "PharmaPlus",
                        MedicineImage = "/images/meds/paracetamol.jpg",
                        MedicineUnit = "Tablet"
                    }
                },
                new PrescriptionDetail
                {
                    PrescriptionId = 2,
                    MedicineId = 2,
                    Dosage = 1,
                    FrequencyPerDay = 1,
                    MedicineSnapshot = new PrescriptionDetailMedicineSnapshot
                    {
                        MedicineName = "Cetirizine",
                        MedicineBrand = "AllerCare",
                        MedicineImage = "/images/meds/cetirizine.jpg",
                        MedicineUnit = "Tablet"
                    }
                }
            );

            return modelBuilder;
        }
    }
}
