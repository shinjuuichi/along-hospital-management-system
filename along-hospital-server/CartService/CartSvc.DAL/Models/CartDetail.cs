using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace CartSvc.DAL.Models
{
    [PrimaryKey(nameof(CartId), nameof(SKUCode))]
    public class CartDetail : Entity
    {
        public int CartId { get; set; }

        public string SKUCode { get; set; } = string.Empty;

        [NumberHigherThanOrEqualTo(1)]
        public int Quantity { get; set; }

        public virtual Cart? Cart { get; set; }
    }
}
