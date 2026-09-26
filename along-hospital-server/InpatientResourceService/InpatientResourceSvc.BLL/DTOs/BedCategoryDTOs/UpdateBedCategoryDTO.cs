using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.BedCategoryDTOs
{
    public class UpdateBedCategoryDTO : MapTo<BedCategory>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}