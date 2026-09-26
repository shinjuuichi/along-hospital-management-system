using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.MedicineSkuDTOs
{
    public class CreateMedicineSKUDTO : MapTo<MedicineSKU>
    {
        public string? Name { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public int? MinQuantity { get; set; }

        public int? MaxQuantity { get; set; }

        public int MedicineId { get; set; }

        public List<int> OptionValueIds { get; set; } = [];
    }
}
