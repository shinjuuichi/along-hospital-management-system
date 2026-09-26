using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.MedicineSkuDTOs
{
    public class UpdateMedicineSKUDTO : MapTo<MedicineSKU>
    {
        public string? Name { get; set; }

        public double Price { get; set; }

        public int? MinQuantity { get; set; }

        public int? MaxQuantity { get; set; }

        public bool IsActive { get; set; }
    }
}
