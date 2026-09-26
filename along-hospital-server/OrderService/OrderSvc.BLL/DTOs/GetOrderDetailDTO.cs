using OrderSvc.DAL.Models;
using OrderSvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Mappers;

namespace OrderSvc.BLL.DTOs
{
    public class GetOrderDetailDTO : MapFrom<OrderDetail>
    {
        public string? SKUCode { get; set; }
        public double? DiscountAmount { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public MedicineSnapshotDTO? MedicineSnapshot { get; set; }
    }

    public class MedicineSnapshotDTO : MapFrom<MedicineSnapshot>
    {
        public string? MedicineName { get; set; }
        public string? MedicineBrand { get; set; }
        public string[] MedicineImages { get; set; } = [];
        public string? MedicineUnit { get; set; }
    }
}
