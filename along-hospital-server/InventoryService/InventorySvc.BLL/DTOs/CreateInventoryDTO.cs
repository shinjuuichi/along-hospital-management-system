using InventorySvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InventorySvc.BLL.DTOs
{
    public class CreateInventoryDTO : MapTo<Inventory>
    {
        public int Quantity { get; set; }
        public DateTime? LastImportDate { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public string? SKUCode { get; set; }
    }
}