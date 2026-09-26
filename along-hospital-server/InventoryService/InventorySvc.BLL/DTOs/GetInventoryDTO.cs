using InventorySvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InventorySvc.BLL.DTOs
{
    public class GetInventoryDTO : MapFrom<Inventory>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public DateTime? LastImportDate { get; set; }
        public string? SKUCode { get; set; }
        public GetMedicineDTO? Medicine { get; set; }
    }
}