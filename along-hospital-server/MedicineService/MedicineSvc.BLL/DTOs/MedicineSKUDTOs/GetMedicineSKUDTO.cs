using MedicineSvc.BLL.DTOs.MedicineDTOs;
using MedicineSvc.BLL.DTOs.OptionDTOs;
using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.MedicineSkuDTOs
{
    public class GetMedicineSKUDTO : MapFrom<MedicineSKU>
    {
        public int Id { get; set; }

        public string? SKUCode { get; set; }

        public string? Name { get; set; }

        public double Price { get; set; }

        public double OrigionPrice { get; set; }

        public bool IsActive { get; set; }

        public int MedicineId { get; set; }

        public GetMedicineDTO? Medicine { get; set; }

        public GetInventoryDTO? Inventory { get; set; }

        public bool IsPublic { get; set; }

        public List<GetSKUValueDTO> SKUValues { get; set; } = [];
    }

    public class GetSKUValueDTO : MapFrom<SKUValue>
    {
        public int MedicineSKUId { get; set; }

        public int OptionValueId { get; set; }

        public GetOptionValueDTO? OptionValue { get; set; }
    }
}
