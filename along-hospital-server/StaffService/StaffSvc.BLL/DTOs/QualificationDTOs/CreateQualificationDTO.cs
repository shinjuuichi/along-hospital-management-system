using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.QualificationDTOs
{
    public class CreateQualificationDTO : MapTo<Qualification>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}