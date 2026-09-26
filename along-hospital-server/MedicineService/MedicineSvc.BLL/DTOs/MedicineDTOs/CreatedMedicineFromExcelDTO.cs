using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.MedicineDTOs
{
    public class CreatedMedicineFromExcelDTO : MapFrom<Medicine>
    {
        public int MedicineId { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
    }
}