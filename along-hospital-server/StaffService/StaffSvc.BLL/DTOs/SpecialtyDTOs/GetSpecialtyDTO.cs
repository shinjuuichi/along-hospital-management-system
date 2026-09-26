using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.SpecialtyDTOs
{
    public class GetSpecialtyDTO : MapFrom<Specialty>
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool IsMedical { get; set; }
    }
}