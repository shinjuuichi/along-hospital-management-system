using BillingSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace BillingSvc.DAL.Models
{
    public class Charge : AuditEntity
    {
        [NumberHigherThanOrEqualTo(1)]
        public int Quantity { get; set; }

        [NumberPositive]
        public double UnitPrice { get; set; }

        public ChargeTypeEnum ChargeType { get; set; } = ChargeTypeEnum.Invoice;

        [MessageRequired]
        public int InvoiceId { get; set; }

        [MessageRequired]
        public int MedicalServiceId { get; set; }

        [JsonColumn]
        public virtual ChargeSnapshot? ChargeSnapshot { get; set; }

        public virtual Invoice? Invoice { get; set; }

        public virtual Refund? Refund { get; set; }

        public double GetTotalAmount() => Quantity * UnitPrice;
    }
}