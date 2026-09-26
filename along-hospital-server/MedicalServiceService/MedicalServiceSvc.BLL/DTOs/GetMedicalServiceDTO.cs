using MedicalServiceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicalServiceSvc.BLL.DTOs
{
    public class GetMedicalServiceDTO : MapFrom<MedicalService>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public string? Code { get; set; }
        public int SpecialtyId { get; set; }
        public string? SpecialtyName { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}