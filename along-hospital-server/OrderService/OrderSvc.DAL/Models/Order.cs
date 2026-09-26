using OrderSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace OrderSvc.DAL.Models
{
    public class Order : AuditEntity
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? PaidDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public OrderStatusEnum OrderStatus { get; set; } = OrderStatusEnum.Unpaid;

        [MessageRequired]
        public int PatientId { get; set; }

        public Guid? TransactionId { get; set; }

        [MessageMaxLength(50)]
        public string? VoucherCode { get; set; }

        [NumberHigherThanOrEqualTo(0)]
        public double OriginPrice { get; set; }

        [NumberHigherThanOrEqualTo(0)]
        public double? TotalDiscountAmount { get; set; }

        [NumberHigherThanOrEqualTo(0)]
        public double FinalPrice { get; set; }

        public bool IsPickupAtStore { get; set; } = false;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = [];
    }
}
