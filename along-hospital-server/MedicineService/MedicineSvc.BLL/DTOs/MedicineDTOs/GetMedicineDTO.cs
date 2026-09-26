using MedicineSvc.BLL.DTOs.MedicineCategoryDTOs;
using MedicineSvc.BLL.DTOs.MedicineSkuDTOs;
using MedicineSvc.BLL.DTOs.MedicineUnitDTOs;
using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Text.Json.Serialization;

namespace MedicineSvc.BLL.DTOs.MedicineDTOs
{
    public class GetMedicineDTO : MapFrom<Medicine>
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Brand { get; set; }

        public string[] Images { get; set; } = [];

        public int MedicineUnitId { get; set; }

        public double? DiscountAmount { get; set; }

        public double? FinalPrice { get; set; }

        public int MedicineCategoryId { get; set; }

        public string? Status { get; set; }

        public bool IsPublic { get; set; }

        public GetMedicineUnitDTO? MedicineUnit { get; set; }

        public GetMedicineCategoryDTO? MedicineCategory { get; set; }

        [JsonPropertyName("skus")]
        public List<GetMedicineSKUDTO> SKUs { get; set; } = [];
    }
}