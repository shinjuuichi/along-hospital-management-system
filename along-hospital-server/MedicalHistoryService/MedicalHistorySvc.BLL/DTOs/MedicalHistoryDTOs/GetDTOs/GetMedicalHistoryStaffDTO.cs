namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs
{
    public class GetMedicalHistoryStaffDTO : GetMedicalHistoryUserDTO
    {
        public string? SignatureImage { get; set; }

        public DateOnly HireDate { get; set; }

        public string? QualificationName { get; set; }

        public string? SpecialtyName { get; set; }
    }
}