using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductSvc.DAL.Models
{
    public class ProductDetail : Entity
    {
        [Key]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        [MessageMaxLength(500)]
        public string Specification { get; set; } = null!;

        [MessageMaxLength(500)]
        public string? Ingredients { get; set; }

        public double Weight { get; set; }

        public string? Manufacturer { get; set; }

        public string? CountryOfOrigin { get; set; }
    }
}

