using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.MedicineCategoryDTOs
{
    public class UpsertMedicineCategoryDTO : MapTo<MedicineCategory>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}