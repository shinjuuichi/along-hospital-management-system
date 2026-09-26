using SharedLibrary.Commons.EntityAbstractions;

namespace BillingSvc.DAL.Models
{
    public class ChargeSnapshot : Entity
    {
        public string? MedicalServiceName { get; set; }

        public string? MedicalServiceCode { get; set; }

        public string? MedicalServiceDescription { get; set; }
    }
}