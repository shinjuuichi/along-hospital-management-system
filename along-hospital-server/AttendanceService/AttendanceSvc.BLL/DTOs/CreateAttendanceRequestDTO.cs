using Microsoft.AspNetCore.Http;

namespace AttendanceSvc.BLL.DTOs
{
    public class CreateAttendanceRequestDTO
    {
        public IFormFile? File { get; set; }
    }
}
