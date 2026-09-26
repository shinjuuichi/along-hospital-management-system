using MedicineSvc.BLL.DTOs.OptionDTOs;
using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.MedicineUnitOptionDTOs
{
    public class GetMedicineUnitOptionDTO : MapFrom<MedicineUnitOption>
    {
        public bool IsActive { get; set; }
        public GetOptionDTO? Option { get; set; }
    }
}
