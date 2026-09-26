using MedicalHistorySvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.GetDTOs
{
    public class GetPrescriptionDetailDTO : MapFrom<PrescriptionDetail>
    {
        public int PrescriptionId { get; set; }

        public int MedicineId { get; set; }

        public double Dosage { get; set; }

        public int FrequencyPerDay { get; set; }

        public GetPrescriptionDetailMedicineSnapshotDTO? MedicineSnapshot { get; set; }
    }
}