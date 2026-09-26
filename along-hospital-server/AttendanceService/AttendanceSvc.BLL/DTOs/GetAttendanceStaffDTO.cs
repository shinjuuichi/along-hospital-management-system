namespace AttendanceSvc.BLL.DTOs
{
    public class GetAttendanceStaffDTO
    {
        public int Id { get; set; }

        public DateOnly HireDate { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }
    }
}