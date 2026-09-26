using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.SpecialtyDTOs
{
    public class UpdateSpecialtyDTO : MapTo<Specialty>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool IsMedical { get; set; }
    }
}