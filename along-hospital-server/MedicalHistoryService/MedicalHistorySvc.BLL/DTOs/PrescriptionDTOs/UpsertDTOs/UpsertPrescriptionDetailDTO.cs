using MedicalHistorySvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Text.Json.Serialization;

namespace MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.UpsertDTOs
{
    public class UpsertPrescriptionDetailDTO : MapTo<PrescriptionDetail>
    {
        public int MedicineId { get; set; }

        public double Dosage { get; set; }

        public int FrequencyPerDay { get; set; }

        [JsonIgnore]
        public UpsertPrescriptionDetailMedicineSnapshotDTO? MedicineSnapshot { get; set; }
    }
}