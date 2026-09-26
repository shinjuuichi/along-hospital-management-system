using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.MedicineUnitDTOs
{
    public class CreateMedicineUnitDTO : MapTo<MedicineUnit>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public List<int> OptionIds { get; set; } = [];
    }
}
