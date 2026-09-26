using SharedLibrary.Commons.EntityAbstractions;

namespace CartSvc.DAL.Models
{
    public class Cart : BaseEntity
    {
        public int PatientId { get; set; }

        public virtual ICollection<CartDetail> CartDetails { get; set; } = [];
    }
}