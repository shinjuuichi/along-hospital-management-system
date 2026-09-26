using MedicalHistorySvc.DAL.Models;
using SharedLibrary.Commons.Filters;

namespace MedicalHistorySvc.BLL.FilterDTOs
{
    public class MedicalHistoryFilterDTO : FilterDTO
    {
        [FilterField]
        public string? MedicalHistoryStatus { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        public string? MedicalHistoryNumber { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(MedicalHistory.AdmissionDate))]
        public DateOnly? StartDate { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(MedicalHistory.DischargeDate))]
        public DateOnly? EndDate { get; set; }

        public string? DoctorName { get; set; }

        public string? PatientName { get; set; }

        public override int PageSize { get; set; } = 5;
    }
}
