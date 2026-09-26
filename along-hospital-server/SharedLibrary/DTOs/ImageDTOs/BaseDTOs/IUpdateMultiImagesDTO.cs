using Microsoft.AspNetCore.Http;

namespace SharedLibrary.DTOs.ImageDTOs.BaseDTOs
{
    public interface IUpdateMultiImagesDTO
    {
        IFormFileCollection? NewImages { get; set; }
        string[]? RemainImages { get; set; }
        string[]? RemoveImages { get; set; }
    }
}
