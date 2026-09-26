using MedicineSvc.DAL.Models;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.DTOs.ImageDTOs.BaseDTOs;
using SharedLibrary.Enums;

namespace MedicineSvc.BLL.DTOs.MedicineDTOs
{
    public class CreateMedicineDTO : MapTo<Medicine>, ICreateMultiImagesDTO
    {
        public string? Name { get; set; }

        public string? Brand { get; set; }

        public int MedicineUnitId { get; set; }

        public int MedicineCategoryId { get; set; }

        public bool IsPublic { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFileCollection? NewImages { get; set; }
    }
}
