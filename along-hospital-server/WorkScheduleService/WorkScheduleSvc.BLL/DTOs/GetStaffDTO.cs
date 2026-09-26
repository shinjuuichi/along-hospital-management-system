namespace WorkScheduleSvc.BLL.DTOs
{
    public class GetStaffDTO
    {
        public int UserId { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public string? Gender { get; set; }

        public string? Role { get; set; }

        public int QualificationId { get; set; }

        public string? QualificationName { get; set; }

        public int SpecialtyId { get; set; }

        public string? SpecialtyName { get; set; }
    }
}