using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.MedicineDTOs
{
    public class CreateMedicineFromExcelDTO : MapTo<Medicine>
    {
        public string? Name { get; set; }

        public double Price { get; set; }
    }
}