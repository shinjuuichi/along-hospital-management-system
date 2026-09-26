using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.QualificationDTOs
{
    public class GetQualificationDTO : MapFrom<Qualification>
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}