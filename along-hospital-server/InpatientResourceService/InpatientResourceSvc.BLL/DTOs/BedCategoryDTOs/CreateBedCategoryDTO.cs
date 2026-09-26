using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.BedCategoryDTOs
{
    public class CreateBedCategoryDTO : MapTo<BedCategory>
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
