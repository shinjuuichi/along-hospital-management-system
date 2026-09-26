using Microsoft.AspNetCore.Http;

namespace SharedLibrary.DTOs.ImageDTOs.BaseDTOs
{
    public interface ICreateMultiImagesDTO
    {
        IFormFileCollection? NewImages { get; set; }
    }
}
