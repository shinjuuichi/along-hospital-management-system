using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace ProductSvc.DAL.Models
{
    public class Category : EntityWithImage
    {
        [MessageRequired, Unique]
        public string Name { get; set; } = null!;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<Product> Products { get; set; } = [];
    }
}
