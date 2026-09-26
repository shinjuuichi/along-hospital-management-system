using Microsoft.AspNetCore.Http;

namespace SharedLibrary.DTOs.ImageDTOs.BaseDTOs
{
    public interface IUploadImageDTO
    {
        IFormFile? Image { get; set; }
    }
}
