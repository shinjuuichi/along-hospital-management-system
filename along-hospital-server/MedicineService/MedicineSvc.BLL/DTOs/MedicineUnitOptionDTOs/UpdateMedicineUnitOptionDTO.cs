using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.MedicineUnitOptionDTOs
{
    public class UpdateMedicineUnitOptionDTO : MapTo<MedicineUnitOption>
    {
        public int MedicineUnitId { get; set; }
        public int OptionId { get; set; }
        public bool IsActive { get; set; }
    }
}