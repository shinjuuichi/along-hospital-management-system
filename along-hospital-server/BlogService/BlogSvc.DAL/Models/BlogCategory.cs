using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace BlogSvc.DAL.Models
{
    public class BlogCategory : BaseEntity
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MessageMaxLength(255)]
        public string? Description { get; set; }

        [OnDelete(OnDeleteBehavior.Restrict)]
        public virtual ICollection<Blog> Blogs { get; set; } = [];
    }
}