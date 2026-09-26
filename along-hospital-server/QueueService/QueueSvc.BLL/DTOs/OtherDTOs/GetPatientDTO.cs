namespace QueueSvc.BLL.DTOs.OtherDTOs
{
    public class GetPatientDTO
    {
        // Auth Data
        public string? Phone { get; set; }

        public string? Email { get; set; }

        // User Data
        public string? Name { get; set; }

        public string? Image { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        // Patient Data
        public string? MedicalNumber { get; set; }

        public int? Height { get; set; }

        public double? Weight { get; set; }

        public string? BloodType { get; set; }
    }
}