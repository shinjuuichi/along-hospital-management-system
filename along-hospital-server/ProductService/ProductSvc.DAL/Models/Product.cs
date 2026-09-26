using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace ProductSvc.DAL.Models
{
    public class Product : EntityWithMultiImages
    {
        [MessageRequired]
        [MessageMaxLength(100)]
        [Unique]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public double Price { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ProductDetail? ProductDetail { get; set; }
    }
}
