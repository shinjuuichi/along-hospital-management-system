using Microsoft.AspNetCore.Http;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Enums;

namespace AttendanceSvc.BLL.DTOs
{
    public class EnrollStaffIdentificationDTO
    {
        [AllowFileType(FileType.Image)]
        public List<IFormFile> Images { get; set; } = [];
    }
}
