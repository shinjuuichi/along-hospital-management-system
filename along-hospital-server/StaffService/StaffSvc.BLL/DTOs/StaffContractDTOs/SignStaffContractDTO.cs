using Microsoft.AspNetCore.Http;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Enums;

namespace StaffSvc.BLL.DTOs.StaffContractDTOs
{
    public class SignStaffContractDTO
    {
        [MessageRequiredFile]
        [AllowFileType(FileType.Image)]
        public IFormFile SignatureImageFile { get; set; } = null!;
    }
}